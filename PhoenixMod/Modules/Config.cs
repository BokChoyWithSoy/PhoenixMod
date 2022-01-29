using BepInEx.Configuration;
using UnityEngine;
using System;

namespace PhoenixWright.Modules
{
    public static class Config
    {
        public static ConfigEntry<bool> loweredVolume;
        public static ConfigEntry<bool> gainTurnaboutOnHit;
        public static void ReadConfig()
        {
            loweredVolume = PhoenixPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Sound settings", "SFX"), false, new ConfigDescription("Set to true to lower SFX"));
            gainTurnaboutOnHit = PhoenixPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Turnabout stacks", "Turnabout stacks"), false, new ConfigDescription("Set to true to enable gain turnabout on hit"));
        }

        // this helper automatically makes config entries for disabling survivors
        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return PhoenixPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this character"));
        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return PhoenixPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }
    }
}