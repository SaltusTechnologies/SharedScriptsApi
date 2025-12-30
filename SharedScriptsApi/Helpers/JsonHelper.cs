#region usings

using Newtonsoft.Json;
using System.Runtime.CompilerServices;

#endregion

namespace SharedScriptsApi.Helpers
{
    public static class JsonHelper
    {
        #region static fields/properties

        public static JsonSerializerSettings Settings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    //NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                };
            }
        }

        public static JsonSerializerSettings SettingsNoNull
        {
            get
            {
                var settings = Settings;
                settings.NullValueHandling = NullValueHandling.Ignore;
                return settings;
            }
        }

        public static JsonSerializerSettings SettingsPreserveObjectReferences
        {
            get
            {
                return new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    //NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                };
            }
        }

        public static JsonSerializerSettings SettingsNoFormatting
        {
            get
            {
                var settings = Settings;
                settings.Formatting = Formatting.None;
                return settings;
            }
        }

        public static JsonSerializerSettings SettingsNoFormattingNull
        {
            get
            {
                var settings = SettingsNoFormatting;
                settings.NullValueHandling = NullValueHandling.Ignore;
                return settings;
            }
            
        }

        #endregion
    }
}
