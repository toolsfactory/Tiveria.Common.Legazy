using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;

namespace Tiveria.Common
{
    public class DirectoryUtils
    {
        private static readonly IdentityReferenceCollection groups = WindowsIdentity.GetCurrent().Groups;
        private static readonly string sidCurrentUser = WindowsIdentity.GetCurrent().User.Value;

        public static bool IsDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return ((attr & FileAttributes.Directory) == FileAttributes.Directory);
        }

        public static bool HaveWritePermissionsForFolder(string path)
        {
            string folder = IsDirectory(path) ? path : Path.GetDirectoryName(path);
            var rules = Directory.GetAccessControl(folder).GetAccessRules(true, true, typeof(SecurityIdentifier));

            bool allowwrite = false;
            bool denywrite = false;
            foreach (FileSystemAccessRule rule in rules)
            {
                if (rule.AccessControlType == AccessControlType.Deny &&
                    (rule.FileSystemRights & FileSystemRights.WriteData) == FileSystemRights.WriteData &&
                    (groups.Contains(rule.IdentityReference) || rule.IdentityReference.Value == sidCurrentUser)
                    )
                {
                    denywrite = true;
                }
                if (rule.AccessControlType == AccessControlType.Allow &&
                    (rule.FileSystemRights & FileSystemRights.WriteData) == FileSystemRights.WriteData &&
                    (groups.Contains(rule.IdentityReference) || rule.IdentityReference.Value == sidCurrentUser)
                    )
                {
                    allowwrite = true;
                }
            }

            // If we have both allow and deny permissions, the deny takes precident.
            if (allowwrite && !denywrite)
                return true;

            return false;
        }

        public static void CleanFolder(string path)
        {
            try
            {
                ForcedDeleteDirectory(path);
                System.IO.Directory.CreateDirectory(path);
            }
            catch { }
        }

        public static void ForcedDeleteDirectory(string path, bool recursive = true)
        {
            if (!System.IO.Directory.Exists(path))
                return;

            var dir = new System.IO.DirectoryInfo(path);
            RemoveReadOnlyFlags(dir, recursive);
            dir.Delete(recursive);
        }

        private static void RemoveReadOnlyFlags(System.IO.DirectoryInfo dir, bool recursive = true)
        {
            foreach (var file in dir.GetFiles())
                if (file.IsReadOnly)
                    file.IsReadOnly = false;

            if (recursive)
                RemoveReadOnlyFlagsInSubDirectories(dir, recursive);
        }

        private static void RemoveReadOnlyFlagsInSubDirectories(System.IO.DirectoryInfo dir, bool recursive)
        {
            foreach (var subdir in dir.GetDirectories())
                RemoveReadOnlyFlags(subdir, recursive);
        }   
    }
}
