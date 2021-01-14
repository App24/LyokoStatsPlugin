using LyokoAPI.API;
using LyokoAPI.API.Compatibility;
using LyokoAPI.Plugin;
using System.Collections.Generic;

namespace LyokoStatsPlugin
{
    public static class Extensions
    {
        public static string GetSetting(this PluginConfig config, string key)
        {
            return config.GetSetting(key, null);
        }
        public static string GetSetting(this PluginConfig config, string key, string defaultValue)
        {
            if (config.Values.TryGetValue(key, out var value))
            {
                return value;
            }
            return defaultValue;
        }

        public static void RemoveSetting(this PluginConfig config, string key)
        {
            if (config.HasSetting(key))
            {
                config.Values.Remove(key);
            }
        }

        public static bool HasSetting(this PluginConfig config, string key)
        {
            return config.GetSetting(key) != null;
        }

        public static void SetSetting(this PluginConfig config, string key, string value)
        {
            if (config.HasSetting(key))
            {
                config.Values.Remove(key);
                config.Values.Add(key, value);
            }
            else
            {
                config.Values.Add(key, value);
            }
        }

        public static void IncrementStat(this PluginConfig config, string statName, int defaultValue=0)
        {
            string settingName = $"stat_{statName}";
            string setting = config.GetSetting(settingName, "0");
            if (!int.TryParse(setting, out var value))
            {
                value = defaultValue;
            }
            value++;
            config.SetSetting(settingName, value.ToString());
        }

        public static void CleanStats(this PluginConfig config)
        {
            Dictionary<string, string> newValues = new Dictionary<string, string>(config.Values);
            config.Values.Clear();
            foreach (var item in newValues)
            {
                if (!item.Key.StartsWith("stat_")) config.Values.Add(item.Key, item.Value);
            }
        }

        public static bool UpToDate(this LVersion version, LVersion other)
        {
            bool result = false;
            if (version.MajorVersion >= other.MajorVersion)
            {
                result = true;
            }
            else if (version.MajorVersion == other.MajorVersion && version.MinorVersion >= other.MinorVersion)
            {
                result = true;
            }
            else if (version.MajorVersion == other.MajorVersion && version.MinorVersion == other.MinorVersion && version.SubVersion >= other.SubVersion)
            {
                result = true;
            }
            else if (version.MajorVersion == other.MajorVersion && version.MinorVersion == other.MinorVersion && version.SubVersion == other.SubVersion && version.BuildVersion >= other.BuildVersion)
            {
                result = true;
            }
            return result;
        }
    }
}
