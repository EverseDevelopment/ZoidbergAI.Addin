using System.Configuration;

namespace Common.Utils
{
    public class AppSettingsUtilities
    {
        public static string GetValue(string configPath, string key)
        {
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = configPath}, ConfigurationUserLevel.None);
            return configuration.AppSettings.Settings[key].Value;
        }

        public static void SetValue(string configPath, string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = configPath }, ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
