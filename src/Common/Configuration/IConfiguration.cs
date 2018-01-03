using System;

namespace Tiveria.Common.Configuration
{
    public interface IConfiguration
    {
        bool PendingChanges { get; }
        IConfigurationSection RootSection { get; }

        void Save();
        void SaveAs(string configurationName);

        event EventHandler Saved;
        event EventHandler Changed;
    }
}
