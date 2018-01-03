using System;
using System.Collections.ObjectModel;
using System.IO;
using SharpSvn;

namespace Tiveria.Common.Subversion
{
    public class SVNWrapper
    {
        private SvnClient _SvnClient;
        private int _RepositoryRevision;
        private string _Username;
        private string _Password;
        private Logging.ILogger _Logger;

        public SVNWrapper(Tiveria.Common.Logging.ILogManager logManager)
        {
            _Logger = logManager.GetLogger("SVNWrapper");
            _Logger.Info("Creating instance");
            _Username = "";
            _Password = "";
            _RepositoryRevision = -1;
            _SvnClient = new SvnClient();
            _SvnClient.Conflict += new EventHandler<SvnConflictEventArgs>(ConflictHandler);
        }

        void ConflictHandler(object sender, SvnConflictEventArgs e)
        {
            e.Choice = SvnAccept.MineFull;
            System.Diagnostics.Debug.WriteLine(e);
        }

        public SVNWrapper(Tiveria.Common.Logging.ILogManager logManager, string username, string password) : this(logManager)
        {
            _Username = username;
            _Password = password;
            _SvnClient.Authentication.DefaultCredentials = new System.Net.NetworkCredential(_Username, _Password);
        }

        public int RepositoryRevision
        {
            get { return _RepositoryRevision; }
        }

        public string RepositoryUri;

        public string WorkingCopyPath;

        public bool IsSvnControlled(string path)
        {
            if (!Directory.Exists(path) && !File.Exists(path))
                return false;

            try
            {
                Collection<SvnStatusEventArgs> statuses;
                if (_SvnClient.GetStatus(path, out statuses) && ((statuses.Count == 0)  || (statuses[0].LocalContentStatus != SvnStatus.NotVersioned)))
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        public bool RefreshWorkingCopy()
        {
            _Logger.Info("Starting to refresh the working copy folder");
            if (!IsSvnControlled(WorkingCopyPath))
            {
                _Logger.Info("No working copy folder or currently not under SVN control. Checking out content");
                SvnCheckOutArgs checkoutArgs = new SvnCheckOutArgs();
                checkoutArgs.Depth = SvnDepth.Infinity;
                checkoutArgs.Notify += new EventHandler<SvnNotifyEventArgs>(CheckoutNotificationHandler);
                try
                {
                    SvnUpdateResult result;
                    bool returncode = _SvnClient.CheckOut(SvnUriTarget.FromString(RepositoryUri), WorkingCopyPath, out result);
                    if (returncode)
                    {
                        _RepositoryRevision = (int)result.Revision;
                        _Logger.Info(String.Format("Sucessfully checked out revision {0} from {1} to {2}", _RepositoryRevision, RepositoryUri, WorkingCopyPath));
                    }
                }
                catch (Exception ex)
                {
                    _Logger.Fatal(String.Format("Checkout from {0} to {1} failed! - {2}", RepositoryUri, WorkingCopyPath, ex));
                    return false;
                }
            }
            else 
            {
                _Logger.Info("Updating working copy folder");
                SvnUpdateArgs updateArgs = new SvnUpdateArgs();
                updateArgs.Depth = SvnDepth.Infinity;
                updateArgs.Notify += new EventHandler<SvnNotifyEventArgs>(CheckoutNotificationHandler);
                updateArgs.Conflict += new EventHandler<SvnConflictEventArgs>(ConflictHandler);
                try
                {
                    SvnUpdateResult result;
                    bool returncode = _SvnClient.Update(WorkingCopyPath, updateArgs, out result);
                    if (!returncode || (result.Revision < 0))
                    {
                        _Logger.Error(String.Format("Updating from {0} to {1} failed!", RepositoryUri, WorkingCopyPath));
                        return false;
                    }
                    _RepositoryRevision = (int)result.Revision;
                    _Logger.Info(String.Format("Sucessfully updated  to revision {0}", _RepositoryRevision));
                }
                catch (Exception ex)
                {
                    _Logger.Fatal(String.Format("Checkout from {0} to {1} failed! - {2}", RepositoryUri, WorkingCopyPath, ex));
                    return false;
                }
            }
            return true;
        }

        private void CheckoutNotificationHandler(object sender, SvnNotifyEventArgs e)
        {
            switch (e.Action)
            {
                case SvnNotifyAction.UpdateAdd: _Logger.Info(String.Format(" + added file: {0}", e.FullPath));
                    break;
                case SvnNotifyAction.UpdateDelete: _Logger.Info(String.Format(" - deleted file: {0}", e.FullPath));
                    break;
                case SvnNotifyAction.UpdateUpdate: _Logger.Info(String.Format(" * updated file: {0}", e.FullPath));
                    break;
            }
        }

        public Collection<SvnStatusEventArgs> WorkingCopyFilesStatus()
        {
            Collection<SvnStatusEventArgs> result = null;
            try
            {
                _SvnClient.GetStatus(WorkingCopyPath, out result);
            }
            catch (Exception ex)
            { 
            }
            return result;
        }

        public bool CommitWorkingCopy()
        {
            SvnCommitArgs commitArgs = new SvnCommitArgs();
            commitArgs.Notify += new EventHandler<SvnNotifyEventArgs>(CommitNotifyHandler);
            commitArgs.Depth = SvnDepth.Infinity;
            commitArgs.LogMessage = "user commit: " + _Username;
            //commitArgs.ThrowOnError = false;
            //commitArgs.SvnError += new EventHandler<SvnErrorEventArgs>(commitArgs_SvnError);
            try
            {
                SvnCommitResult result;
                bool returnCode = _SvnClient.Commit(WorkingCopyPath, commitArgs, out result);
                if (returnCode)
                {
                    _RepositoryRevision = (int)result.Revision;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        void commitArgs_SvnError(object sender, SvnErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Exception);
        }

        void CommitNotifyHandler(object sender, SvnNotifyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Action.ToString() + " - " + e.FullPath);
        }

        public int GetRepositoryFiles(out Collection<SvnListEventArgs> apiList, string subPath)
        {
            SvnListArgs listArgs = new SvnListArgs();
            listArgs.Depth = SvnDepth.Children;

            bool returnCode = _SvnClient.GetList(new SvnUriTarget(subPath, SvnRevision.Head), listArgs, out apiList);

            if (returnCode)
            {
                return apiList.Count;
            }

            return -1;
        }
    }
}