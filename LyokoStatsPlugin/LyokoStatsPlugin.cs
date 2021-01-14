using System.Collections.Generic;
using System.Threading.Tasks;
using LyokoAPI.API;
using LyokoAPI.Events;
using LyokoAPI.API.Compatibility;
using LyokoAPI.Plugin;
using System.IO;

namespace LyokoStatsPlugin
{
    public class LyokoStatsPlugin : LyokoAPIPlugin
    {
        public override string Name => PluginName;

        public static string PluginName => "StatsPlugin";

        public override string Author => "App24";
        public override LVersion Version => "1.0.0";
        public override List<LVersion> CompatibleLAPIVersions => new List<LVersion>() { "2.0.0" };

        StatsLogger statsLogger = new StatsLogger();

        LVersion ConfigVersion = "1.0.0";

        SimpleHTTPServer myServer;

        protected override bool OnEnable()
        {
            if (ConfigManager.GetMainConfig().HasSetting("version"))
            {
                if(LVersion.TryParse(ConfigManager.GetMainConfig().GetSetting("version"), out var newVersion)){
                    if (!newVersion.UpToDate(ConfigVersion))
                    {
                        ConfigManager.GetMainConfig().CleanStats();
                        ConfigManager.GetMainConfig().SetSetting("version", ConfigVersion.ToString());
                    }
                }
                else
                {
                    ConfigManager.GetMainConfig().CleanStats();
                    ConfigManager.GetMainConfig().SetSetting("version", ConfigVersion.ToString());
                }
            }
            else
            {
                ConfigManager.GetMainConfig().CleanStats();
                ConfigManager.GetMainConfig().SetSetting("version", ConfigVersion.ToString());
            }

            statsLogger.PluginConfig = ConfigManager.GetMainConfig();
            statsLogger.StartListening();
            int port = -1;
            if (ConfigManager.GetMainConfig().HasSetting("serverPort"))
            {
                if(uint.TryParse(ConfigManager.GetMainConfig().GetSetting("serverPort"), out var result)){
                    port = (int)result;
                }
                else
                {
                    LyokoLogger.Log(Name, $"Port Number {ConfigManager.GetMainConfig().GetSetting("serverPort")} is not a valid port (must be a whole number, and above 0)");
                }
            }
            if (port == -1)
            {
                myServer = new SimpleHTTPServer();
            }
            else
            {
                myServer = new SimpleHTTPServer(port);
            }
            myServer.PluginConfig = ConfigManager.GetMainConfig();
            LyokoLogger.Log(Name, $"StatsPlugin Web server started in port {myServer.Port}, goto http://localhost:{myServer.Port} to access it");
            if(port==-1)
            LyokoLogger.Log(Name, "You can set a custom port for the server by going to the config file and putting \"serverPort: *any value*\" as a new line");
            return true;
        }

        protected override bool OnDisable()
        {
            statsLogger.StopListening();
            ConfigManager.SaveAllConfigs();
            myServer.Stop();
            return true;
        }

        public override void OnGameStart(bool storyMode)
        {
            ConfigManager.GetMainConfig().IncrementStat("gameStart");
        }

        public override void OnGameEnd(bool failed)
        {
            ConfigManager.GetMainConfig().IncrementStat("gameEnd");
        }

        public override void OnInterfaceExit()
        {
            ConfigManager.GetMainConfig().IncrementStat("interfaceExit");
        }

        public override void OnInterfaceEnter()
        {
            ConfigManager.GetMainConfig().IncrementStat("interfaceEnter");
        }
    }
}
