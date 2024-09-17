using BepInEx.Configuration;
using UnityEngine;
using System;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RiskOfOptions;

namespace PhoenixWright.Modules
{
    public static class Config
    {
        public static ConfigEntry<bool> loweredVolume;
        public static ConfigEntry<bool> gainTurnaboutOnHit;
        public static ConfigEntry<int> necessaryStacksTurnabout;

        public static ConfigEntry<float> phoenixSFXVolume;
        public static ConfigEntry<float> phoenixVoiceVolume;
        public static ConfigEntry<float> phoenixMusicVolume;

        public static void ReadConfig()
        {
            loweredVolume = PhoenixPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Sound settings", "SFX"), false, new ConfigDescription("Set to true to lower SFX"));
            gainTurnaboutOnHit = PhoenixPlugin.instance.Config.Bind<bool>(new ConfigDefinition("Turnabout stacks", "Limit turnabout stacks"), false, new ConfigDescription("Set to true to gain max of one turnabout per hit"));
            necessaryStacksTurnabout = PhoenixPlugin.instance.Config.Bind<int>(new ConfigDefinition("Turnabout stacks", "Necessary Stacks for Turnabout Mode"), 50, new ConfigDescription("Set to 0 to start game with turnabout mode active"));

            phoenixMusicVolume = PhoenixPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("Phoenix Turnabout Music Volume", "Phoenix Music Volume"),
                100f,
                new ConfigDescription("Determines of the volume of Phoenix's Turnabout Theme")
            );

            phoenixVoiceVolume = PhoenixPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("Phoenix Voice Volume", "Phoenix Voice Volume"),
                100f,
                new ConfigDescription("Determines of the volume of Phoenix's Voicelines")
            );

            phoenixSFXVolume = PhoenixPlugin.instance.Config.Bind<float>
            (
                new ConfigDefinition("Phoenix SFX Volume", "Phoenix SFX Volume"),
                100f,
                new ConfigDescription("Determines of the volume of Phoenix's Sound Effects")
            );
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

        public static void SetupRiskOfOptions()
        {
            ModSettingsManager.AddOption(
                new StepSliderOption(
                    phoenixSFXVolume,
                    new StepSliderConfig
                    {
                        min = 0f,
                        max = 100f,
                        increment = 0.5f,
                    }
                )
            );

            ModSettingsManager.AddOption(
            new StepSliderOption(
                    phoenixVoiceVolume,
                    new StepSliderConfig
                    {
                        min = 0f,
                        max = 100f,
                        increment = 0.5f,
                    }
                )
            );

            ModSettingsManager.AddOption(
            new StepSliderOption(
                phoenixMusicVolume,
                new StepSliderConfig
                    {
                        min = 0f,
                        max = 100f,
                        increment = 0.5f,
                    }
                )
            );
        }

        public static void OnChangeHooks()
        {
            phoenixSFXVolume.SettingChanged += PhoenixSFXVolume_Changed;
            phoenixVoiceVolume.SettingChanged += PhoenixVoiceVolume_Changed;
            phoenixMusicVolume.SettingChanged += PhoenixMusicVolume_Changed;
        }

        private static void PhoenixSFXVolume_Changed(object sender, EventArgs e)
        {
            if (AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.SetRTPCValue("VolumePhoenixSFX", phoenixSFXVolume.Value);
            }
        }

        private static void PhoenixVoiceVolume_Changed(object sender, EventArgs e)
        {
            if (AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.SetRTPCValue("VolumePhoenixVoice", phoenixVoiceVolume.Value);
            }
        }

        private static void PhoenixMusicVolume_Changed(object sender, EventArgs e)
        {
            if (AkSoundEngine.IsInitialized())
            {
                AkSoundEngine.SetRTPCValue("VolumePhoenixMusic", phoenixMusicVolume.Value);
            }
        }
    }
}
