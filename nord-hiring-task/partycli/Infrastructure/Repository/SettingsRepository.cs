using System;

namespace partycli.Infrastructure.Repository
{
    public class SettingsRepository : ISettingsRepository
    {
        public void InsertValue(string name, string value)
        {
            try
            {
                var settings = Properties.Settings.Default;
                settings[name] = value;
                settings.Save();
            }
            catch
            {
                Console.WriteLine("Error: Couldn't save " + name + ". Check if command was input correctly.");
            }
        }
    }
}
