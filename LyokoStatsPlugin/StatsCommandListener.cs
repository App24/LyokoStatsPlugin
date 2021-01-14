using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyokoAPI.Commands;

namespace LyokoStatsPlugin
{
    public class StatsCommandListener : CommandListener
    {
        protected override string Prefix => "stats";
    }
}
