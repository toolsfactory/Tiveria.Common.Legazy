using System;
using System.Collections.Generic;
using System.Linq;
using Tiveria.Common.Bootstrapper.Core;

namespace Tiveria.Common.Bootstrapper.Plugins
{
    public class BootstrapperStartupTaskPlugin : IBootstrapperPlugin
    {
        public void Initialize(IBootstrapperContext context)
        {
        }

        public void Startup(IBootstrapperContext context)
        {
            var instances = context.GetInstancesOfTypesImplementing<IBootstrapperStartupTask>();
            var enriched = EnrichTaskInstances(instances);
            enriched.OrderBy(a => a.Position).ToList().ForEach(a => a.Task.OnStartup(context.Bag));
        }

        public void Shutdown(IBootstrapperContext context)
        {
        }

        private List<StartupTaskExecutionParameters> EnrichTaskInstances(List<IBootstrapperStartupTask> instances)
        {
            var result = new List<StartupTaskExecutionParameters>();
            foreach(var task in instances)
            {
                result.Add(EnrichWithAttributeValues(task));
            }
            return result;
        }

        private StartupTaskExecutionParameters EnrichWithAttributeValues(IBootstrapperStartupTask task)
        {
            var attribute = task.GetType().GetCustomAttributes(false).FirstOrDefault(a => a is TaskAttribute) as TaskAttribute;
            if (attribute == null)
            {
                return new StartupTaskExecutionParameters() { Delay = 0, Group = "Default", Position = int.MaxValue, Task = task };
            }
            else
            {
                return new StartupTaskExecutionParameters() { Delay = 0, Group = "Default" /*attribute.DelayStartBy, Group = attribute.Group */, Position = attribute.Position, Task = task };
            }
        }
    }

}