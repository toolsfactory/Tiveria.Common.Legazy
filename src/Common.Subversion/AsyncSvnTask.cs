using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSvn;
using System.Threading;
using System.Threading.Tasks;
using Tiveria.Common.Extensions;

namespace Tiveria.Common.Subversion
{
    public delegate void RepositoryUpdateResult(bool success, bool canceled);

    public class TaskFinishedEventArgs : EventArgs
    {
        public bool Success { get; private set; }
        public TaskFinishedEventArgs(bool success)
        { Success = success; }
    }

    public class AsyncSvnTask
    {
        private SvnClient _SvnClient  = new SvnClient();
        private CancellationTokenSource _CTS;

        public string LocalPath { get; private set; }
        public string RepositoryUrl { get; private set; }

        public event EventHandler<TaskFinishedEventArgs> TaskFinished;
        #region OnTaskFinished
        /// <summary>
        /// Triggers the TaskFinished event.
        /// </summary>
        public virtual void OnTaskFinished(TaskFinishedEventArgs ea)
        {
            if (TaskFinished != null)
                TaskFinished(this, ea);
        }
        #endregion

        public AsyncSvnTask(string localpath, string repositoryUrl)
        {
            LocalPath = localpath;
            RepositoryUrl = repositoryUrl;
            _SvnClient.Cancel += _SvnClient_Cancel;
        }

        public void SetSecurityCredentials(string username = "", string password = "")
        {
            if (username.IsNullOrWhiteSpace())
                _SvnClient.Authentication.DefaultCredentials = null;
            else
                _SvnClient.Authentication.DefaultCredentials = new System.Net.NetworkCredential(username, password);
        }

        public void Cancel()
        {
            _CTS.Cancel();
        }

        void _SvnClient_Cancel(object sender, SvnCancelEventArgs e)
        {
            e.Cancel = _CTS.Token.IsCancellationRequested;
        }


        Task task;
        public void  UpdateToRepositoryRevision(int revision)
        {
            _CTS = new CancellationTokenSource(); 
            task = new Task(() =>
                {
                    DoRepositoryCheckout(revision);
                }, _CTS.Token);
            task.Start();
        }

        public void LookIntoTask()
        {
            Console.WriteLine(task);
            task.Wait();
            task.Dispose();
        }

        private void DoRepositoryCheckout(int revision)
        {
            bool result = false;
            try
            {
                TryCleanUp();
                result = TryUpdate(revision);

                if (!result)
                    result = FullCheckout(revision);
            }
            catch (Exception ex) { }
            OnTaskFinished(new TaskFinishedEventArgs(result));
        }

        private void TryCleanUp()
        {
            try
            {
                if (System.IO.Directory.Exists(LocalPath))
                    _SvnClient.CleanUp(LocalPath);
            }
            catch { }
        }

        private bool TryUpdate(int revision)
        {
            try
            {
                SvnUpdateResult updateResult;
                var args = new SvnUpdateArgs();
                args.Revision = revision;

                CreateFolder();
                var result = _SvnClient.Update(LocalPath, args, out updateResult);
                return result && updateResult.HasRevision;
            }
            catch (SvnSystemException)
            { return false; }
        }

        private bool FullCheckout(int revision)
        {
            CleanFolder();
            var target = new SvnUriTarget(RepositoryUrl, revision);
            var result = _SvnClient.CheckOut(target, LocalPath);
            return result;
        }

        private void CreateFolder()
        {
            System.IO.Directory.CreateDirectory(LocalPath);
        }

        private void CleanFolder()
        {
            try
            {
                if (System.IO.Directory.Exists(LocalPath))
                    System.IO.Directory.Delete(LocalPath);
                CreateFolder();
            }
            catch { }
        }

    }

}
