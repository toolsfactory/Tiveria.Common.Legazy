using System;
using System.Collections.Generic;
using System.Linq;
using Tiveria.Common.Bootstrapper.Core;

namespace Tiveria.Common.Bootstrapper.Plugins
{
    public class BootstrapperShutdownTaskPlugin : IBootstrapperPlugin
    {
        public void Initialize(IBootstrapperContext context)
        {
        }

        public void Startup(IBootstrapperContext context)
        {
        }

        public void Shutdown(IBootstrapperContext context)
        {
            var instances = context.GetInstancesOfTypesImplementing<IBootstrapperShutdownTask>();
            var enriched = EnrichTaskInstances(instances);
            enriched.OrderBy(a => a.Position).ToList().ForEach(a => a.Task.OnShutdown(context.Bag));
        }

        private List<ShutdownTaskExecutionParameters> EnrichTaskInstances(List<IBootstrapperShutdownTask> instances)
        {
            var result = new List<ShutdownTaskExecutionParameters>();
            foreach (var task in instances)
            {
                result.Add(EnrichWithAttributeValues(task));
            }
            return result;
        }

        private ShutdownTaskExecutionParameters EnrichWithAttributeValues(IBootstrapperShutdownTask task)
        {
            var attribute = task.GetType().GetCustomAttributes(false).FirstOrDefault(a => a is TaskAttribute) as TaskAttribute;
            if (attribute == null)
            {
                return new ShutdownTaskExecutionParameters() { Delay = 0, Group = "Default", Position = int.MaxValue, Task = task };
            }
            else
            {
                return new ShutdownTaskExecutionParameters() { Delay = 0, Group = "Default" /*attribute.DelayStartBy, Group = attribute.Group */, Position = attribute.Position, Task = task };
            }
        }

    }
}
