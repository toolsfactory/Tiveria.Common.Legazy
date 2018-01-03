using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Tiveria.Common.Update
{
    public class UpdateEngine : IDisposable
    {
        readonly TimeSpan DEFAULTCHECKFREQUENCY = TimeSpan.FromMinutes(15);
        #region private Members
        private BackgroundWorker _MonitoringWorker;
        private BackgroundWorker _DownloadWorker;
        private FileDownloader _Downloader;
        private bool _InitialCheck;
        private bool _IsOneTimeCheck;
        private bool _IsMonitoring;
        private EventWaitHandle _MonitoringExitHandle;
        private Logging.ILogger _Logger;
        private string _UpdatePackageInfoUrl;
        private string _RsaPublicKey;
        private TimeSpan _CheckFrequency;
        private Version _CurrentAppVersion;
        private bool _AcceptAnySSLCertificate;
        #endregion

        #region Public Events
        public event EventHandler MonitoringStarted;
        public event EventHandler MonitoringFinished;
        public event EventHandler<UpdateFoundEventArgs> UpdateFound;
        public event EventHandler<UpdateDownloadEventArgs> UpdateDownloadProgressChanged;
        public event EventHandler UpdateDownloadStarting;
        public event EventHandler UpdateDownloadFinished;
        public event EventHandler<UpdateDownloadErrorEventArgs> UpdateDownloadError;
        #endregion

        #region Public Interface
        #region Properties
        public bool AcceptAnySSLCertificate
        { 
            get { return _AcceptAnySSLCertificate;} 
            set{_AcceptAnySSLCertificate=value;}
        }

        public Boolean IsMonitoring
        {
            get
            {
                return _IsMonitoring;
            }
        }

        public Tiveria.Common.Logging.ILogger Logger
        {
            get { return _Logger; }
            set
            {
                if (value == _Logger)
                    return;
                _Logger = value;
            }
        }

        public UpdatePackageInfo AvailableUpdatePackageInfo
        { get; private set; }

        public bool UpdateAvailable
        {
            get
            {
                return AvailableUpdatePackageInfo != null;
            }
        }

        public bool AutoDownload
        { get; set; }

        public string DownloadFolder
        { get; set; }

        public string RsaPublicKey
        {
            get
            {
                return _RsaPublicKey;
            }
            set
            {
                _RsaPublicKey = value;
            }
        }

        public bool IsUpdateDownloaded
        { get { return !String.IsNullOrWhiteSpace(UpdateFileName); } }

        public string UpdateFileName
        { get; private set; }
        #endregion

        #region Constructors & Disposal
        public UpdateEngine(string updateurl)
            : this (updateurl, null)
        { }

        public UpdateEngine(string updateurl,Version currentversion)
        {
            ReportStatusInfo("Initializing UpdateEngine");

            // Set received parameters
            _UpdatePackageInfoUrl = updateurl;
            _CurrentAppVersion = currentversion;
            _IsMonitoring = false;

            // Set other parameters to initial default values
            _AcceptAnySSLCertificate = false;
            _CheckFrequency = DEFAULTCHECKFREQUENCY;

            //set Properties to initial values
            AutoDownload = false;
            DownloadFolder = "";
            UpdateFileName = "";

            // create the workers
            _MonitoringWorker = new BackgroundWorker();
            _MonitoringWorker.DoWork += _MonitoringWorker_DoWork;
            _DownloadWorker = new BackgroundWorker();
            _DownloadWorker.DoWork += _DownloadWorker_DoWork;

            // build the wait handle
            _MonitoringExitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

        public void Dispose()
        {
            StopMonitoring();
        }
        #endregion

        #region Background Monitoring Start & Stop
        public bool StartMonitoring()
        {
            return StartMonitoring(true);
        }

        public bool StartMonitoring(bool initialcheck)
        {
            return StartMonitoring(initialcheck, DEFAULTCHECKFREQUENCY);
        }

        public bool StartMonitoring(bool initialcheck, TimeSpan checkfrequency)
        {
            // Check if already monitoring and return if so
            if (_IsMonitoring)
                return false;

            // set signal that monitoring is ongoing
            _IsMonitoring = true;

            // Set Monitoring rules
            _IsOneTimeCheck = false;
            _InitialCheck = initialcheck;
            _CheckFrequency = checkfrequency;

            // Check if folder is available
            if (String.IsNullOrWhiteSpace(DownloadFolder))
                throw new ArgumentException("DownloadFolder not specified");

            try
            {
                if (!System.IO.Directory.Exists(DownloadFolder))
                    System.IO.Directory.CreateDirectory(DownloadFolder);
            }
            catch
            {
                throw new ArgumentException("DownloadFolder cannot be created!");
            }

            if (!Common.DirectoryUtils.HaveWritePermissionsForFolder(DownloadFolder))
                throw new ArgumentException("DownloadFolder cannot be accessed!");

            _MonitoringWorker.DoWork += _MonitoringWorker_DoWork;
            _MonitoringWorker.RunWorkerAsync();
            return true;
        }

        public void StopMonitoring()
        {
            _MonitoringExitHandle.Set();
        }
        #endregion

        #region Manual Search and Dowload
        public bool SearchOnce()
        {
            // Check if already monitoring and return if so
            if (_IsMonitoring)
                return false;

            // set signal that monitoring is ongoing
            _IsMonitoring = true;

            // Set Monitoring rules
            _IsOneTimeCheck = true;
            _InitialCheck = true;
            UpdateFileName = "";

            _MonitoringWorker.RunWorkerAsync();

            return true;
        }

        public bool DownloadUpdate()
        {
            if (_DownloadWorker.IsBusy)
                return false;

            if (!UpdateAvailable)
                return false;

            UpdateFileName = "";
            _DownloadWorker.RunWorkerAsync();
            return true;
        }

        public void StopDownload()
        {
            if (_Downloader != null)
                _Downloader.Cancel();
        }
        #endregion
        #endregion

        #region Implementations
        #region Update Check & Download
        private bool CheckForApplicableUpdate(out UpdatePackageInfo updateinfo)
        {
            bool updateAvailable = false;
            // Start the check algorithm
            ReportStatusInfo("Checking for UpdatePackageInfo");

            // init the Loader
            UpdatePackageInfoLoader loader = new UpdatePackageInfoLoader(_UpdatePackageInfoUrl, _RsaPublicKey);
            loader.Logger = _Logger;

            // check if any updates are available
            try
            {
                updateAvailable = loader.IsUpdateInfoAvailable();
                updateinfo = loader.UpdateInfo;
            }
            catch (Exception e)
            {
                // Report the error
                ReportStatusError("Error during Update download: " + e.Message, e);
                updateinfo = null;
            }

            // Check if in general any updateinfo was found
            if (updateinfo == null)
            {
                ReportStatusInfo("No version information in app cast found");
                return false;
            }


            ReportStatusInfo("Version found:" + updateinfo.Version);

            // check if the version is really newer
            Version version = new Version(updateinfo.Version);

            if (version <= _CurrentAppVersion)
            {
                ReportStatusInfo("Currently installed version is up to date. Update will not be applied");
                return false;
            }

            // ok the found update should be installed
            return true;
        }

        /// <summary>
        /// This method will be executed as worker thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _MonitoringWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // initialize the locally needed variables
            bool doContinueMonitoring = true;
            bool updateAvailable = false;
            bool isInitialRun = true;
            bool exitSignal = false;
            UpdatePackageInfo updateinfo = null;

            //Now trigger the Start event
            OnMonitoringStarted(this, null);

            while (doContinueMonitoring)
            {
                doContinueMonitoring = !_IsOneTimeCheck;
                if ((!isInitialRun) || (isInitialRun && !_InitialCheck))
                {
                    // create the events array with the exit event in
                    WaitHandle[] handles = new WaitHandle[1] { _MonitoringExitHandle };

                    // Wait until exit signal or timeout
                    ReportStatusInfo(String.Format("Pausing update search vor {0} minutes.", _CheckFrequency.TotalMinutes));
                    int waitResult = WaitHandle.WaitAny(handles, _CheckFrequency);

                    // check wait result
                    switch (waitResult)
                    {
                        case 0: exitSignal = true;
                                break;
                        case WaitHandle.WaitTimeout:    ReportStatusInfo(String.Format("Waited {0} minutes", _CheckFrequency.TotalMinutes));
                                break;
                        default: ReportStatusInfo("Unknown event. Ignoring...");
                                break;
                    }
                }

                // check if we have an exit Signal
                if (exitSignal)
                {
                    ReportStatusInfo("Got exit signal");                        
                    break;
                }

                // Now Check for an update
                updateAvailable = CheckForApplicableUpdate(out updateinfo);

                if (updateAvailable)
                {
                    AvailableUpdatePackageInfo = updateinfo;
                    OnUpdateFound(this, new UpdateFoundEventArgs() { FoundTitle = updateinfo.Title, FoundVersion = new Version(updateinfo.Version) });
                }

                if (updateAvailable && AutoDownload && !_IsOneTimeCheck)
                {
                    _DownloadWorker_DoWork(null, null);
                }

                // modify the loop controller variables
                doContinueMonitoring &= !updateAvailable;
                // Initial rune is now definitly over
                isInitialRun = false;
            }

            OnMonitoringFinished(this, null);
            _IsMonitoring = false;
        }

        private void _DownloadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // try to download the file
            UpdateFileName = DownloadUpdateFile(AvailableUpdatePackageInfo);

            // check whether we got a file back (indicated by receiving a filename)
            if (String.IsNullOrEmpty(UpdateFileName))
            {
                // if not, report an error and return
                AvailableUpdatePackageInfo = null;
                ReportStatusError("Update could not be downloaded");
                OnUpdateDownloadError(this, new UpdateDownloadErrorEventArgs(Update.UpdateDownloadError.DownloadInterrupted, "Update could not be downloaded!"));
                return;
            }

            // now check if signature matches
            if (!VerifyUpdateFile(UpdateFileName, AvailableUpdatePackageInfo.UpdateFileSignature))
            {
                // if not, report an error and return
                AvailableUpdatePackageInfo = null;
                System.IO.File.Delete(UpdateFileName);
                UpdateFileName = "";
                ReportStatusError("Update found & downloaded but invalid signature");
                OnUpdateDownloadError(this, new UpdateDownloadErrorEventArgs(Update.UpdateDownloadError.SignatureMissmatch, "Signature could not be verified!"));
                return;
            }

            // everything seems to be ok. Send download ok event
            ReportStatusInfo("File successfully downloaded");
            OnUpdateDownloadFinished(this, new EventArgs());
        }


        private bool VerifyUpdateFile(string filename, string expectedsignature)
        {
            string filehash;
            UpdatePackageInfoHelper.CreateSHA512HashForFile(filename, out filehash);
            return (expectedsignature == filehash);
        } 

        private string  DownloadUpdateFile(UpdatePackageInfo updateinfo)
        {
            _Downloader = new FileDownloader();
            _Downloader.DownloadStarting += downloader_DownloadStarting;
            _Downloader.ProgressChanged += downloader_ProgressChanged;
            try
            {
                _Downloader.Download(updateinfo.UpdateFileUri, DownloadFolder);
                return _Downloader.DownloadingTo;
            }
            catch
            {
                return "";
            }
        }

        void downloader_DownloadStarting(object sender, EventArgs e)
        {
            OnUpdatedownloadStarting(this, e);
        }

        void downloader_DownloadComplete(object sender, EventArgs e)
        {
            OnUpdateDownloadFinished(this, e);
        }

        void downloader_ProgressChanged(object sender, DownloadEventArgs e)
        {
            OnUpdateDownloadProgressChanged(this, new UpdateDownloadEventArgs(e.TotalFileSize, e.CurrentFileSize, e.DownloadState));
        }

        private bool RemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (_AcceptAnySSLCertificate)
            {
                // verify if we talk about our app cast dll 
                HttpWebRequest req = sender as HttpWebRequest;
                if (req == null)
                    return (certificate is X509Certificate2) ? ((X509Certificate2)certificate).Verify() : false;

                // if so just return our trust 
                if (req.RequestUri.Equals(new Uri(_UpdatePackageInfoUrl)))
                    return true;
                else
                    return (certificate is X509Certificate2) ? ((X509Certificate2)certificate).Verify() : false;
            }
            else
            {
                // check our cert                 
                return (certificate is X509Certificate2) ? ((X509Certificate2)certificate).Verify() : false;
            }
        }
        #endregion

        #region Eventhandlers
        protected virtual void OnMonitoringStarted(object sender, EventArgs e)
        {
            EventHandler handler = MonitoringStarted;
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void OnMonitoringFinished(object sender, EventArgs e)
        {
            EventHandler handler = MonitoringFinished;
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void OnUpdatedownloadStarting(object sender, EventArgs e)
        {
            EventHandler handler = UpdateDownloadStarting;
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void OnUpdateDownloadProgressChanged(object sender, UpdateDownloadEventArgs e)
        {
            EventHandler<UpdateDownloadEventArgs> handler = UpdateDownloadProgressChanged;
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void OnUpdateDownloadFinished(object sender, EventArgs e)
        {
            EventHandler handler = UpdateDownloadFinished;
            if (handler != null)
                handler(sender, e);
        }

        public virtual void OnUpdateDownloadError(object sender, UpdateDownloadErrorEventArgs e)
        {
            EventHandler<UpdateDownloadErrorEventArgs> handler = UpdateDownloadError;
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void OnUpdateFound(object sender, UpdateFoundEventArgs e)
        {
            EventHandler<UpdateFoundEventArgs> handler = UpdateFound;
            if (handler != null)
                handler(sender, e);
        }
        #endregion

        #region Reporting helpers
        private void ReportStatusInfo(string info)
        {
            if ((_Logger != null) && (_Logger.IsInfoEnabled))
                _Logger.Info(info);
        }

        private void ReportStatusError(string info)
        {
            if ((_Logger != null) && (_Logger.IsErrorEnabled))
                _Logger.Error(info);
        }

        private void ReportStatusError(string info, Exception ex)
        {
            if ((_Logger != null) && (_Logger.IsErrorEnabled))
                _Logger.Error(info, ex);
        }
        #endregion

        #endregion
    }
}
