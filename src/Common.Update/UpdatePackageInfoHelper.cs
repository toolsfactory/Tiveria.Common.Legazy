using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using Tiveria.Common.Extensions;
using System.IO;

namespace Tiveria.Common.Update
{
    public static class UpdatePackageInfoHelper
    {
        public static bool CreateSHA512HashForFile(string filename, out string hashbase64)
        {
            hashbase64 = "";

            if (!File.Exists(filename))
            {
                return false;
            }

            try
            {
                {
                    Byte[] hash = null;
                    using (Stream inputStream = File.OpenRead(filename))
                    {
                        using (HashAlgorithm sha = new SHA512Cng())
                        {
                            hash = sha.ComputeHash(inputStream);
                        }
                    }
                    hashbase64 = Convert.ToBase64String(hash);
                    return true;
                }
            }
            catch
            {
                return false;
            }


        }

    }
}
