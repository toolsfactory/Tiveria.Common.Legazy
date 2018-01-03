using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tiveria.Common.Extensions;

namespace Tiveria.Common.Bootstrapper
{
    public static class BootstrapperConfigurationExtensions
    {
        public static IBootstrapperConfiguration IncludeAssemblies(this IBootstrapperConfiguration config, string mask, string path = null, bool includeSubDirs = false)
        {
            if (String.IsNullOrWhiteSpace(path))
                path = AppDomain.CurrentDomain.BaseDirectory;

            var files = System.IO.Directory.EnumerateFiles(path, mask, includeSubDirs ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    config.IncludeAssembly(assembly);
                }
                catch { }
            }
            return config;
        }

        public static IBootstrapperConfiguration IncludeAssemblies(this IBootstrapperConfiguration config, string mask, IList<string> paths)
        {
            if (paths == null || paths.Count == 0)
                return config;

            var files = GetAllFiles(mask, paths);
            foreach (var file in files)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    config.IncludeAssembly(assembly);
                }
                catch { }
            }
            return config;
        }

        private static IEnumerable<string> GetAllFiles(string mask, IList<string> paths)
        {
            var files = new List<string>();
            foreach (var path in paths)
                files.AddRange(System.IO.Directory.EnumerateFiles(path, mask));
            return files;
        }

    }
}
