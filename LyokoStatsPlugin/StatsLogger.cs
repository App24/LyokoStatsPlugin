using System;
using LyokoAPI.API;
using LyokoAPI.Plugin;
using LyokoAPI.Events;
using LyokoAPI.Events.LWEvents;
using LyokoAPI.VirtualEntities.LyokoWarrior;
using LyokoAPI.VirtualEntities.Overvehicle;
using LyokoAPI.VirtualStructures;
using LyokoAPI.VirtualStructures.Interfaces;

namespace LyokoStatsPlugin
{
    public class StatsLogger : LAPIListener
    {
        public PluginConfig PluginConfig;

        //Temporary, until LAPI gets updated
        public override void StartListening()
        {
            base.StartListening();
            LW_DeTranslationEvent.LwE += onLW_Detrans;
            LW_CodeEarthResolvedEvent.LwE += onLW_CodeEarthResolved;
            LW_DNACorruptionEvent.LwE += onLW_DNACorruption;
            LW_FixedDNAEvent.LwE += onLW_FixedDNA;
        }

        //Temporary, until LAPI gets updated
        public override void StopListening()
        {
            base.StopListening();
            LW_DeTranslationEvent.LwE -= onLW_Detrans;
            LW_CodeEarthResolvedEvent.LwE -= onLW_CodeEarthResolved;
            LW_DNACorruptionEvent.LwE -= onLW_DNACorruption;
            LW_FixedDNAEvent.LwE -= onLW_FixedDNA;
        }

        #region LW Events
        public override void onLW_Virt(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwVirted{warrior.WarriorName}");
        }

        public override void onLW_Devirt(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwDevirted{warrior.WarriorName}");
        }

        public override void onLW_Death(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwKilled{warrior.WarriorName}");
        }

        public override void onLW_Dexanafication(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwDexan{warrior.WarriorName}");
        }

        public override void onLW_Frontier(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwFrontier{warrior.WarriorName}");
        }

        public override void onLW_PermXanafy(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwPermXan{warrior.WarriorName}");
        }

        public override void onLW_Trans(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwTrans{warrior.WarriorName}");
        }

        public void onLW_Detrans(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwDetrans{warrior.WarriorName}");
        }

        public override void onLW_Xanafication(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwXan{warrior.WarriorName}");
        }

        public void onLW_CodeEarthResolved(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwCodeearth{warrior.WarriorName}");
        }

        public void onLW_DNACorruption(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwCorrupt{warrior.WarriorName}");
        }

        public void onLW_FixedDNA(LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"lwFix{warrior.WarriorName}");
        }
        #endregion

        #region Misc
        public override void onRTTP()
        {
            PluginConfig.IncrementStat("rttp");
        }

        public override void onWorldDestruction(IVirtualWorld world)
        {
            PluginConfig.IncrementStat("worldDestroy");
        }
        #endregion

        #region OV Events
        public override void onOV_Virt(Overvehicle overvehicle)
        {
            PluginConfig.IncrementStat($"ovVirted{overvehicle.OvervehicleName}");
        }

        public override void onOV_Devirt(Overvehicle overvehicle)
        {
            PluginConfig.IncrementStat($"ovDevirted{overvehicle.OvervehicleName}");
        }

        public override void onOV_GetOff(Overvehicle overvehicle, LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"ovGetoff{overvehicle.OvervehicleName}Lw{warrior.WarriorName}");
        }

        public override void onOV_Ride(Overvehicle overvehicle, LyokoWarrior warrior)
        {
            PluginConfig.IncrementStat($"ovRide{overvehicle.OvervehicleName}Lw{warrior.WarriorName}");
        }
        #endregion

        #region Sector Events
        public override void onSectorCreation(ISector sector)
        {
            PluginConfig.IncrementStat($"sectorCreate");
        }

        public override void onSectorDestruction(ISector sector)
        {
            PluginConfig.IncrementStat($"sectorDestroy");
        }
        #endregion

        #region Xana Events
        public override void onXanaAwaken()
        {
            PluginConfig.IncrementStat($"xanaAwaken");
        }

        public override void onXanaDefeat()
        {
            PluginConfig.IncrementStat($"xanaDefeat");
        }
        #endregion

        #region Tower Events
        public override void onTowerActivation(ITower tower)
        {
            if (tower.Activator == APIActivator.NONE) return;
            PluginConfig.IncrementStat($"towerActivate{tower.Activator}");
        }

        public override void onTowerDeactivation(ITower tower)
        {
            PluginConfig.IncrementStat($"towerDeactivate");
        }

        public override void onTowerHijack(ITower tower, APIActivator old, APIActivator newac)
        {
            PluginConfig.IncrementStat($"towerHijack{old}By{newac}");
        }
        #endregion
    }
}
