using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace Tiveria.Common
{
    public static class PathUtils
    {
        public static string MakeRelativePath(string basepath, string file)
        {
            System.Uri uri1 = new Uri(basepath);
            System.Uri uri2 = new Uri(file);

            Uri relativeUri = uri1.MakeRelativeUri(uri2);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }

        public static bool IsPathRelativeTo(string basepath, string file)
        {
            if (!System.IO.Path.IsPathRooted(basepath))
                return false;

            return file.StartsWith(basepath);
        }

        private static string pathValidatorExpression = "^[^" + string.Join("", Array.ConvertAll(Path.GetInvalidPathChars(), x => Regex.Escape(x.ToString()))) + "]+$";
        private static Regex pathValidator = new Regex(pathValidatorExpression, RegexOptions.Compiled);

        private static string fileNameValidatorExpression = "^[^" + string.Join("", Array.ConvertAll(Path.GetInvalidFileNameChars(), x => Regex.Escape(x.ToString()))) + "]+$";
        private static Regex fileNameValidator = new Regex(fileNameValidatorExpression, RegexOptions.Compiled);

        private static string pathCleanerExpression = "[" + string.Join("", Array.ConvertAll(Path.GetInvalidPathChars(), x => Regex.Escape(x.ToString()))) + "]";
        private static Regex pathCleaner = new Regex(pathCleanerExpression, RegexOptions.Compiled);

        private static string fileNameCleanerExpression = "[" + string.Join("", Array.ConvertAll(Path.GetInvalidFileNameChars(), x => Regex.Escape(x.ToString()))) + "]";
        private static Regex fileNameCleaner = new Regex(fileNameCleanerExpression, RegexOptions.Compiled);

        public static bool ValidatePath(string path)
        {
            return pathValidator.IsMatch(path);
        }

        public static bool ValidateFileName(string fileName)
        {
            return fileNameValidator.IsMatch(fileName);
        }

        public static string CleanPath(string path)
        {
            return pathCleaner.Replace(path, "");
        }

        public static string CleanFileName(string fileName)
        {
            return fileNameCleaner.Replace(fileName, "");
        }

        public static string CleanPathAndFileName(string both)
        {
            both = pathCleaner.Replace(both, "");
            return fileNameCleaner.Replace(both, "");
        }

    }
}
