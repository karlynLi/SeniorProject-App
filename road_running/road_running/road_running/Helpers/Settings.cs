using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace road_running.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        public static bool IsLogin
        {
            get => AppSettings.GetValueOrDefault(nameof(IsLogin), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsLogin), value);
        }
        public static string Token
        {
            get => AppSettings.GetValueOrDefault(nameof(Token), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Token), value);
        }

        //#region Setting Constants

        //private const string SettingsKey = "settings_key";
        //private const string SettingPass = "settings_pass";
        //private static readonly string SettingsDefault = string.Empty;

        //#endregion


        //public static string GeneralSettings
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue(SettingsKey, value);
        //    }
        //}

    }
}