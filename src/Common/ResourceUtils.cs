using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace Tiveria.Common
{
    public static class ResourceUtils
    {
        public static string GetTextResource(Assembly assembly, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            try
            {
                // load a specific stream within the assembly
                Stream stream = assembly.GetManifestResourceStream(name);
                // make a text reader out of it
                StreamReader textReader = new StreamReader(stream);
                // and read the text ...
                return textReader.ReadToEnd();
            }
            catch
            {
                return "";
            }
        }

        public static Image GetImageResource(Assembly assembly, string name, bool pattern = false)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            try
            {
                name = FindResource(assembly, name, pattern);
                if (String.IsNullOrWhiteSpace(name))
                    return null;

                // load a specific stream within the assembly
                Stream stream = assembly.GetManifestResourceStream(name);
                // make a text reader out of it
                Image resImage = new Bitmap(stream);
                // and read the text ...
                return resImage;
            }
            catch
            {
                return null;
            }
        }

        public static string FindResource(Assembly assembly, string name, bool pattern)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            string[] resources = assembly.GetManifestResourceNames();
            foreach (string res in resources)
            {
                if ((res == name) || ((res.EndsWith(name) && pattern)))
                    return res;
            }
            return null;
        }
    }
}
