using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Linq;
using System.ComponentModel;

namespace Tiveria.Common.Extensions
{
    public static class GuidExtensions
    { 
        public static string ToBase64(this Guid guid)
        {
            string encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded
                .Replace("/", "_")
                .Replace("+", "-");
            return encoded.Substring(0, 22);
        }

        public static Guid  FromBase64(this Guid guid, string base64)
        {
            base64 = base64
                .Replace("_", "/")
                .Replace("-", "+");
            byte[] buffer = Convert.FromBase64String(base64 + "==");
            return new Guid(buffer);
        }
    }
}
