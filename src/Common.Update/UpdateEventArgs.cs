using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiveria.Common.Update
{
    public enum UpdateDownloadError
    {
        None,
        FileNotFound,
        DownloadInterrupted,
        SignatureMissmatch
    }

    public class UpdateFoundEventArgs : EventArgs
    {
        public Version FoundVersion;
        public string FoundTitle;
    }

    public class UpdateDownloadEventArgs : EventArgs
    {
        public long TotalFileSize
        { get; private set; }

        public long CurrentFileSize
        { get; private set; }

        public string DownloadState
        { get; private set; }

        public UpdateDownloadEventArgs(long totalFileSize, long currentFileSize, string state="")
        {
            TotalFileSize = totalFileSize;
            CurrentFileSize = currentFileSize;
            DownloadState = state;
        }

        public UpdateDownloadEventArgs(string state)
        {
            DownloadState = state;
        }
    }

    public class UpdateDownloadErrorEventArgs : EventArgs
    {
        public UpdateDownloadError Error
        { get; private set; }

        public string ErrorText
        { get; private set; }

        public UpdateDownloadErrorEventArgs(UpdateDownloadError error, string errortext)
        {
            Error = error;
            ErrorText = ErrorText;
        }
    }
}
