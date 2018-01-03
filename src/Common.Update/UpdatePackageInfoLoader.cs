using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Tiveria.Common.Update
{
    public class UpdatePackageInfoLoader
    {
        private string _Url = "";
        private string _RsaKey = "";
        private UpdatePackageInfo _currentUpdatePackageInfo = null;
        private IWebProxy _Proxy = null;
        private Exception _LastException = null;
        private Tiveria.Common.Logging.ILogger _Logger;

        /// <summary>
        /// Proxy to be used for http and ftp requests.
        /// </summary>
        public IWebProxy Proxy
        {
            get { return _Proxy; }
            set { _Proxy = value; }
        }

        public UpdatePackageInfo UpdateInfo
        { 
            get { return _currentUpdatePackageInfo; } 
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

        public UpdatePackageInfoLoader(string url, string rsakey)
        {
            _RsaKey = rsakey;
            _Url = url;
        }

        public bool IsUpdateInfoAvailable()
        {
            string xmldata;
            _LastException = null;

            ReportStatusInfo("Starting IsUpdateInfoAvailable");
            try
            {
                // Generate the Request object based on the url type
                ReportStatusInfo("Creating request");
                WebRequest request = GetRequest(_Url);

                // request the cast and build the stream
                using (WebResponse response = request.GetResponse())
                {
                    // now use a stream to read data from the server
                    ReportStatusInfo("Reading Response");
                    using (Stream inputstream = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(inputstream);
                        xmldata = sr.ReadToEnd();
                    }
                }

                // finally try to parse the data
                ReportStatusInfo("Deserializing result");
                if (String.IsNullOrEmpty(_RsaKey))
                    _currentUpdatePackageInfo = UpdatePackageInfo.FromXml(xmldata);
                else
                    _currentUpdatePackageInfo = UpdatePackageInfo.FromXml(xmldata, _RsaKey);
                return true;
            }
            catch (WebException ex)
            {
                ReportStatusError("Could not download info package. Checking reason");
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    // Check if File just doesnt exist
                    if ((ex.Response is HttpWebResponse) && (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound))
                        return false;

                    if ((ex.Response is FtpWebResponse) && (((FtpWebResponse)ex.Response).StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable))
                        return false;
                }
                ReportStatusError("Unplaned WebException", ex);
                _LastException = ex;
                return false;
            }
            catch (Exception ex)
            {
                ReportStatusError("Unplaned Exception", ex);
                _LastException = ex;
                return false;
            }
        }
            
        private WebRequest GetRequest(string url)
        {
            WebRequest request = WebRequest.Create(url);
            if (request is HttpWebRequest)
            {
                request.Credentials = CredentialCache.DefaultCredentials;
                Uri result = request.Proxy.GetProxy(new Uri("http://www.google.com"));
            }

            request.Proxy = _Proxy;

            return request;
        }

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

    }
}