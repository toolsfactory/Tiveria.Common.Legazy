using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Tiveria.Common.Bootstrapper.Core;


namespace Tiveria.Common.Bootstrapper
{
    public class BootstrapperAutoMapperPlugin : Profile, Plugins.IBootstrapperPlugin
    {
        public void Initialize(IBootstrapperContext context)
        {
            context.AssembliesConfiguration.ExcludeAssembly("AutoMapper");
        }

        public void Startup(IBootstrapperContext context)
        {
            List<IBootstrapperAutoMapperRegistration> autoMapperRegistrations;
            List<Profile> profiles;

            autoMapperRegistrations = context.GetInstancesOfTypesImplementing<IBootstrapperAutoMapperRegistration>();
            profiles = context.GetInstancesOfTypesImplementing<Profile>();

            Mapper.Initialize(c =>
            {
                profiles.ForEach(c.AddProfile);
                autoMapperRegistrations.ForEach(m => m.RegisterMappings(c, context.Bag));
            });
        }

        public void Shutdown(IBootstrapperContext context)
        {
        }
    }
}
