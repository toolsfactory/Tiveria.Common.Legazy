using System;

namespace Tiveria.Common.Configuration
{
    public interface IConfigurationSection
    {
        string Name { get; }
        string Path { get; }
        T GetItem<T>(string name);
        T GetItem<T>(string name, T defaultValue);
        IConfigurationSection SetItem<T>(string name, T value);
        void DeleteItem(string name);
        IConfigurationSection GetSection(string name);
        void DeleteSection(string name);

        event EventHandler Changed;
    }
}
