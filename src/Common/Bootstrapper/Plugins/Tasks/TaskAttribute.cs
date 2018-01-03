using System;
using System.Collections.Generic;
using System.Linq;
using Tiveria.Common.Bootstrapper.Core;

namespace Tiveria.Common.Bootstrapper.Plugins
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskAttribute : Attribute
    {
        public int Position { get; set; }
//        public int DelayStartBy { get; set; }
//        public string Group { get; set; }

        public TaskAttribute()
        {
            Position = int.MaxValue;
//            DelayStartBy = 0;
//            Group = "Default";
        }
    }
}
