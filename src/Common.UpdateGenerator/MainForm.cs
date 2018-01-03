using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Tiveria.Common.Extensions;
using Tiveria.Common.Update;

namespace Tiveria.Common.UpdateGenerator
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private RSACryptoServiceProvider _PrivateKey = new RSACryptoServiceProvider();
        private UpdateEngine _Engine = null;

        public MainForm()
        {
            InitializeComponent();
            InitSkinGallery();
            Console.WriteLine(testToolStripMenuItem.ShortcutKeys);
        }
        void InitSkinGallery()
        {
            //SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        private void iNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            eTitle.Text = "";
            eDescription.Text = "";
            eReleaseLink.Text = "";
            ePublishDate.EditValue = "";
            eUpdateFile.Text = "";
        }

        internal void AppendLog(string text)
        {
            if (eLog.InvokeRequired)
                eLog.Invoke(new Action<String>(AppendLog), text);
            else
            {
                eLog.Text = eLog.Text + text + Environment.NewLine;
                eLog.ScrollToCaret();
            }
        }

        private void eKeyFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            AppendLog("Loading keyfile...");
            eKeyFile.Text = "";
            ep.ClearErrors();
            if (dlgSelectKey.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string keytext = "";
                    using (StreamReader reader = new StreamReader(dlgSelectKey.FileName))
                    {
                        keytext = reader.ReadToEnd();
                    }
                    _PrivateKey.FromXmlString(keytext);
                    eKeyFile.Text = dlgSelectKey.FileName;
                    bCreate.Enabled = true;
                    bVerify.Enabled = !String.IsNullOrWhiteSpace(eVerDescriptionFile.Text);
                    AppendLog("Keyfile loaded");
                }
                catch
                {
                    AppendLog(string.Format("Error loading keyfile '{0}'", dlgSelectKey.FileName));
                    ep.SetError(eKeyFile, "Error loading keyfile");
                    bCreate.Enabled = false;
                    bVerify.Enabled = false;
                }
            }
        }

        private void bCreate_Click(object sender, EventArgs e)
        {
            AppendLog("Creating update file");

            if (!CheckFields())
                return;

            if(dlgSaveDescriptionFile.ShowDialog()== System.Windows.Forms.DialogResult.OK)
                CreateXMLFile(dlgSaveDescriptionFile.FileName);
        }


        private bool CheckField(System.Windows.Forms.Control control, bool check, string errortext)
        {
            if (!check)
            {
                ep.SetError(control, errortext);
                AppendLog(errortext);
            }
            return check;
        }

        private bool CheckFields()
        {
            ep.ClearErrors();
            bool result = CheckField(eKeyFile,    !String.IsNullOrWhiteSpace(eKeyFile.Text), "No keyfile loaded") &&
                          CheckField(eUpdateFile, !String.IsNullOrWhiteSpace(eUpdateFile.Text), "No updatefile specified") &&
                          CheckField(eUpdateFileLink, !String.IsNullOrWhiteSpace(eUpdateFileLink.Text), "No updatefile link specified") &&
                          CheckField(eTitle, !String.IsNullOrWhiteSpace(eTitle.Text), "No title specified") &&
                          CheckField(eDescription,!String.IsNullOrWhiteSpace(eDescription.Text), "No description specified") &&
                          CheckField(eNewVersion, !String.IsNullOrWhiteSpace(eNewVersion.Text), "No new version specified") &&
                          CheckField(ePublishDate,!String.IsNullOrWhiteSpace(ePublishDate.Text), "No publish date specified");
            return result;
        }

        private void CreateXMLFile(string filename)
        {
            XmlDocument xdoc;
            string hashbase64;
            if (!UpdatePackageInfoHelper.CreateSHA512HashForFile(eUpdateFile.Text, out hashbase64))
                return;

            try
            {
                AppendLog("Creating xml...");
                UpdatePackageInfo ud = new UpdatePackageInfo();
                ud.Title = eTitle.Text;
                ud.Description = eDescription.Text;
                ud.Version = eNewVersion.Text;
                ud.PublishDate = (DateTime)ePublishDate.EditValue;
                ud.ReleaseNotesUri = eReleaseLink.Text;
                ud.UpdateFileName = Path.GetFileName(eUpdateFile.Text);
                ud.UpdateFileUri = eUpdateFileLink.Text;
                ud.UpdateFileSignature = hashbase64;

                ud.ToFile(filename, _PrivateKey.ToXmlString(true));

            }
            catch (Exception ex)
            {
                AppendLog("Error creating updatefile");
                AppendLog(ex.Message);
                return;
            }

            AppendLog("--------------------");
        }

        private void eVerDescriptionFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (dlgSelectUpdateDescFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                eVerDescriptionFile.Text = dlgSelectUpdateDescFile.FileName;
                bVerify.Enabled = true;
            }
        }

        private void bClearLog_Click(object sender, EventArgs e)
        {
            eLog.Text = "";
        }

        private void eUpdateFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (dlgSelectUpdateFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                eUpdateFile.Text = dlgSelectUpdateFile.FileName;
            }
        }

        private void eVerUpdateFile_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (dlgSelectUpdateFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                eVerUpdateFile.Text = dlgSelectUpdateFile.FileName;
            }
        }

        private void bVerify_Click(object sender, EventArgs e)
        {
            UpdatePackageInfo desc;
            string hashbase64;
            AppendLog("Verifying Signature");
            eVerDescriptionFile.Text = dlgSelectUpdateDescFile.FileName;
            
            try
            {
                desc = UpdatePackageInfo.FromFile(dlgSelectUpdateDescFile.FileName, _PrivateKey.ToXmlString(false));

                if (desc!=null)
                {
                    AppendLog("OK - xml signature is ok");
                    iXmlSignature.Text = "Xml signature is OK";
                    iXmlSignature.ImageIndex = 10;
                }
                else
                {
                    AppendLog("ERROR - xml signature missmatch");
                    iXmlSignature.Text = "Xml signature is NOT OK";
                    iXmlSignature.ImageIndex = 11;
                    iUpdateFileSignature.Text = "Not tested";
                    iUpdateFileSignature.ImageIndex = 9;
                    return;
                }
                    

                if (!UpdatePackageInfoHelper.CreateSHA512HashForFile(eVerUpdateFile.Text, out hashbase64))
                {
                    AppendLog("Could not create hash for updatefile - only verifying xml!");
                    iUpdateFileSignature.Text = "Cannot be tested (file error)";
                    iUpdateFileSignature.ImageIndex = 9;
                }
                else
                {
                    if (desc.UpdateFileSignature == hashbase64)
                    {
                        AppendLog("Hash is OK");
                        iUpdateFileSignature.Text = "Update file signatures match";
                        iUpdateFileSignature.ImageIndex = 10;
                    }
                    else
                    {
                        AppendLog("Hash is NOT OK");
                        iUpdateFileSignature.Text = "Update file signatures DO NOT match";
                        iUpdateFileSignature.ImageIndex = 11;
                    }
                }

            }
            catch
            { AppendLog("Error verifying signature"); }
        }

        private void iOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dlgSelectUpdateDescFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                eVerDescriptionFile.Text = dlgSelectUpdateDescFile.FileName;
                UpdatePackageInfo desc = UpdatePackageInfo.FromFile(dlgSelectUpdateDescFile.FileName);
                if (desc != null)
                {
                    eTitle.Text = desc.Title;
                    eDescription.Text = desc.Description;
                    eReleaseLink.Text = desc.ReleaseNotesUri;
                    ePublishDate.EditValue = desc.PublishDate;
                    eNewVersion.Text = desc.Version;
                    eUpdateFile.Text = "";
                }
            }
        }

        private void iCreateKeys_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (dlgSaveKeys.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AppendLog("Creating keyfiles...");
                if (_PrivateKey != null)
                    _PrivateKey.Dispose();
                _PrivateKey = new RSACryptoServiceProvider();
                eKeyFile.Text = "";
                try
                {
                    using (StreamWriter sw = new StreamWriter(dlgSaveKeys.FileName))
                    {
                        AppendLog("Writing private keyfile...");
                        sw.Write(_PrivateKey.ToXmlString(true));
                    }

                    string pubkeyfile = System.IO.Path.ChangeExtension(dlgSaveKeys.FileName, "pub");
                    using (StreamWriter sw = new StreamWriter(pubkeyfile))
                    {
                        AppendLog("Writing public keyfile...");
                        sw.Write(_PrivateKey.ToXmlString(false));
                    }
                    eKeyFile.Text = dlgSaveKeys.FileName;
                }
                catch
                {
                    AppendLog("Error creating keyfiles");
                }
            }
        }

        void engine_UpdatedownloadStarting(object sender, EventArgs e)
        {
            eLog.Invoke(new Action(() =>
            {
                lProgress.Text = "Starting...";
                progressBar.EditValue = 0;
            }));
        }

        void engine_UpdateDownloadFinished(object sender, EventArgs e)
        {
            AppendLog("Download finished");
        }

        void engine_UpdateDownloadProgressChanged(object sender, UpdateDownloadEventArgs e)
        {
            eLog.Invoke(new Action(() =>
            {
                progressBar.Properties.Maximum = (int) e.TotalFileSize;
                progressBar.EditValue = e.CurrentFileSize;
                lProgress.Text = String.Format("{0} of {1} downloaded. Status: {2}", e.CurrentFileSize, e.TotalFileSize, e.DownloadState);
            }));
        }

        void engine_UpdateFound(object sender, UpdateFoundEventArgs e)
        {
            AppendLog(e.FoundTitle);            
        }

        private void eFolder_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    eFolder.Text = dlg.SelectedPath;
            }
        }

        private void bCreateEngine_Click(object sender, EventArgs e)
        {
            if (_Engine != null)
                _Engine.Dispose();

            _Engine = new UpdateEngine(eUpdateDownloadTestUrl.Text, new Version(eUpdateDownloadCheckVersion.Text));
            if (cbCheckSignatures.Checked)
                _Engine.RsaPublicKey = _PrivateKey.ToXmlString(false);

            _Engine.Logger = new InternalLogger() { Form = this };
            _Engine.DownloadFolder = eFolder.Text;
            _Engine.UpdateFound += engine_UpdateFound;
            _Engine.UpdateDownloadProgressChanged += engine_UpdateDownloadProgressChanged;
            _Engine.UpdateDownloadFinished += engine_UpdateDownloadFinished;
            _Engine.UpdateDownloadStarting += engine_UpdatedownloadStarting;

            bStartUpdateCheck.Enabled = true;
            bOneTimeCheck.Enabled = true;
            bDownloadAvailable.Enabled = true;
            bMonitoringOnly.Enabled = true;
        }

        private void bStartUpdateCheck_Click(object sender, EventArgs e)
        {
            if (_Engine == null)
                return;
            _Engine.AutoDownload = true;
            var canstart = _Engine.StartMonitoring(cbInitialCheck.Checked, TimeSpan.FromSeconds(45));
            if (!canstart)
                AppendLog("Engine already monitoring");
        }

        private void bOneTimeCheck_Click(object sender, EventArgs e)
        {
            if (_Engine == null)
                return;
            var canstart = _Engine.SearchOnce();
            if (!canstart)
                AppendLog("Engine already monitoring");
        }

        private void bMonitoringOnly_Click(object sender, EventArgs e)
        {
            if (_Engine == null)
                return;
            _Engine.AutoDownload = true;
            var canstart = _Engine.StartMonitoring();
            if (!canstart)
                AppendLog("Engine already monitoring");
        }

        private void bStopMonitoring_Click(object sender, EventArgs e)
        {
            if (_Engine == null)
                return;
            _Engine.StopMonitoring();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            eFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        }

        private void bDownloadAvailable_Click(object sender, EventArgs e)
        {
            if (_Engine == null)
                return;

            var downloading = _Engine.DownloadUpdate();
            if (!downloading)
                AppendLog("Download not started");
        }

    }

    internal class InternalLogger : Logging.ILogger
    {
        public MainForm Form;

        public void Debug(object message)
        {
            Form.AppendLog("Debug: " + message);
        }

        public void Debug(object message, Exception exception)
        {
            Form.AppendLog("Debug: " + message);
            Form.AppendLog("Debug Exception: " + exception);
        }

        public void Info(object message)
        {
            Form.AppendLog("Info: " + message);
        }

        public void Info(object message, Exception exception)
        {
            Form.AppendLog("Info: " + message);
            Form.AppendLog("Info Exception: " + exception);
        }

        public void Warn(object message)
        {
            Form.AppendLog("Warn: " + message);
        }

        public void Warn(object message, Exception exception)
        {
            Form.AppendLog("Warn: " + message);
            Form.AppendLog("Warn Exception: " + exception);
        }

        public void Error(object message)
        {
            Form.AppendLog("Error: " + message);
        }

        public void Error(object message, Exception exception)
        {
            Form.AppendLog("Error: " + message);
            Form.AppendLog("Error Exception: " + exception);
        }

        public void Fatal(object message)
        {
            Form.AppendLog("Fatal: " + message);
        }

        public void Fatal(object message, Exception exception)
        {
            Form.AppendLog("Fatal: " + message);
            Form.AppendLog("Fatal Exception: " + exception);
        }

        public bool IsDebugEnabled
        {
            get { return true; }
        }

        public bool IsInfoEnabled
        {
            get { return true; }
        }

        public bool IsWarnEnabled
        {
            get { return true; }
        }

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsFatalEnabled
        {
            get { return true; }
        }
    }
}


