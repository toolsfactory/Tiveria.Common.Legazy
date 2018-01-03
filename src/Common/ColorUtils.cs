using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;

namespace Tiveria.Common
{
    static public class ColorUtils
    {
        private static readonly Dictionary<string, Color> dictionary = 
            typeof(Color).GetProperties(BindingFlags.Public |  
                                        BindingFlags.Static) 
                         .Where(prop => prop.PropertyType == typeof(Color)) 
                         .ToDictionary(prop => prop.Name.ToLower(), 
                                       prop => (Color) prop.GetValue(null, null)); 
 
        public static Color FromHex(string hex)
        {
            return FromHex(hex, Color.Empty);
        }

        private static Color FromHex(string hex, Color defaultColor)
        {
            try
            {
                return ColorTranslator.FromHtml(hex);
            }
            catch
            {
                return defaultColor; 
            }
        }

        public static Color FromName(string name)
        {
            return FromName(name, Color.Empty);
        }

        public static Color FromName(string name, Color defaultColor)
        {
            if (dictionary.ContainsKey(name.ToLower()))
                return dictionary[name.ToLower()];
            else
                return defaultColor;
        }

        public static Color FromString(string text, Color defaultColor)
        {
            if (text.StartsWith("#"))
                return FromHex(text, defaultColor);
            else
                return FromName(text, defaultColor);
        }
        
        public static Color FromString(string text)
        {
            return FromString(text, Color.Empty);
        }
    } 
}
