using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyokoAPI.Commands;
using LyokoAPI.Exceptions;
using LyokoAPI.Events;

namespace LyokoStatsPlugin
{
    public class StatsCommandListener : CommandListener
    {
        protected override string Prefix => "stats";

        public override void onCommandInput(string arg)
        {
            List<ICommand> Commands = GetCommands();
            if (!Commands.Any())
            {
                return;
            }
            if (!arg.StartsWith(Prefix))
            {
                return;
            }

            string[] commandargs = arg.Split('.');
            if (commandargs.Length > 2)
            {
                commandargs = commandargs.Skip(2).ToArray();
            }
            else if (commandargs.Length == 1)
            {
                List<string> CommandNames = new List<string>();
                if (Commands != null)
                {
                    Commands.ForEach(c => { CommandNames.Add(c.Name); });
                    CommandOutputEvent.Call(Prefix, $"Commands: {String.Join(",", CommandNames)}");
                }
                return;
            }
            else
            {
                commandargs = new string[] { };
            }
            var commandname = commandargs[1]; //The 0th element is the prefix, so the first is the command
            var command = Commands?.Find(commandd => commandd.Name.Equals(commandname));
            if (command != null)
            {
                command.Run(commandargs);
            }
            else
            {
                CommandOutputEvent.Call(Prefix, $"Unknown command: \"{commandname}\"");
            }
        }
    }
}
