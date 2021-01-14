using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyokoAPI.Commands;
using LyokoAPI.Exceptions;
using LyokoAPI.Plugin;

namespace LyokoStatsPlugin.Commands
{
    public class SetPort : Command
    {
        public override string Name => "setport";
        public override int MinArgs => 1;

        private PluginConfig pluginConfig;

        public SetPort SetPluginConfig(PluginConfig config)
        {
            pluginConfig = config;
            return this;
        }

        protected override void DoCommand(string[] args)
        {
            if (args[0].ToLower() == "clear")
            {
                pluginConfig.RemoveSetting("serverPort");
                Output("Cleared Port Number");
            }
            else
            {
                if (!uint.TryParse(args[0], out var result))
                {
                    throw new CommandException(this, $"Port Number {args[0]} is not a valid port (must be a whole number, and above 0)");
                }
                pluginConfig.SetSetting("serverPort", result.ToString());
                Output($"Port Number is now {result}");
            }
            Output("Restarting plugin");
            LyokoStatsPlugin.DisablePlugin();
            LyokoStatsPlugin.EnablePlugin();
        }
    }
}
