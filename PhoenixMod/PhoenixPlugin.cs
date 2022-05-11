﻿using BepInEx;
using PhoenixWright.Modules.Survivors;
using PhoenixWright.SkillStates.BaseStates;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace PhoenixWright
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
    })]

    public class PhoenixPlugin : BaseUnityPlugin
    {
        public static int currentStacks;
        public static bool turnaboutActive;
        public bool turnaboutEnabled;
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.BokChoyWithSoy.PhoenixWright";
        public const string MODNAME = "PhoenixWright";
        public const string MODVERSION = "1.7.4";

        public static PhoenixController phoenixController;
        public static Vector3 characterPos;

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "BOK";

        internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

        public static PhoenixPlugin instance;

        private void Awake()
        {
            phoenixController = null;

            instance = this;

            // load assets and read config
            Modules.Assets.Initialize();
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new Phoenix().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += LateSetup;

            Hook();

            turnaboutActive = true;
        }

        private void LateSetup(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
        {
            // have to set item displays later now because they require direct object references..
            Modules.Survivors.Phoenix.instance.SetItemDisplays();
        }


        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterModel.Awake += CharacterModel_Awake;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.FixedUpdate += CharacterBody_FixedUpdate;
            MusicTrackDef music = new MusicTrackDef();
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // a simple stat hook, adds armor after stats are recalculated
            if (self)
            {
                if (self.HasBuff(Modules.Buffs.armorBuff))
                {
                    self.armor += 300f;
                }
            }
        }

        private void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        {
            orig(self);

            characterPos = self.transform.position;

            if (self.baseNameToken == PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_NAME")
            {
                self.SetBuffCount(Modules.Buffs.turnaboutBuff.buffIndex, currentStacks);
                if (PhoenixPlugin.currentStacks >= PhoenixController.maxStacks)
                {
                    #region primary
                    if (self.skillLocator.primary.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME"))
                    {
                        self.skillLocator.primary.UnsetSkillOverride(self.skillLocator.primary, Phoenix.primaryBottle, GenericSkill.SkillOverridePriority.Contextual);
                        self.skillLocator.primary.UnsetSkillOverride(self.skillLocator.primary, Phoenix.primaryKnife, GenericSkill.SkillOverridePriority.Contextual);
                        self.skillLocator.primary.UnsetSkillOverride(self.skillLocator.primary, Phoenix.primaryPhone, GenericSkill.SkillOverridePriority.Contextual);
                        self.skillLocator.primary.UnsetSkillOverride(self.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
                        self.skillLocator.primary.SetSkillOverride(self.skillLocator.primary, Phoenix.primaryArm, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    if (self.skillLocator.primary.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PRIMARY_PAPER_NAME"))
                    {
                        self.skillLocator.primary.SetSkillOverride(self.skillLocator.primary, Phoenix.primaryPaperStrong, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    #endregion

                    PhoenixController.setLock(false);

                    #region secondary
                    if (self.skillLocator.secondary.skillNameToken.Equals(PhoenixPlugin.developerPrefix +"_PHOENIX_BODY_SECONDARY_PRESS_NAME"))
                    {
                        self.skillLocator.secondary.SetSkillOverride(self.skillLocator.secondary, Phoenix.secondaryPressStrong, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    if (self.skillLocator.secondary.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_SECONDARY_LOCK_NAME"))
                    {
                        self.skillLocator.secondary.SetSkillOverride(self.skillLocator.secondary, Phoenix.secondaryLockStrong, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    #endregion

                    #region utility
                    if (self.skillLocator.utility.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_UTILITY_FALL_NAME"))
                    {
                        self.skillLocator.utility.SetSkillOverride(self.skillLocator.utility, Phoenix.rollSkillDef2, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    if (self.skillLocator.utility.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_UTILITY_FALL2_NAME"))
                    {
                        self.skillLocator.utility.SetSkillOverride(self.skillLocator.utility, Phoenix.tumbleStrongSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    #endregion

                    self.skillLocator.special.SetSkillOverride(self.skillLocator.special, Phoenix.gavelStrong, GenericSkill.SkillOverridePriority.Contextual);

                }
            }

        }

        private void CharacterBody_OnDeathStart(On.RoR2.CharacterBody.orig_OnDeathStart orig, CharacterBody self)
        {
            orig(self);
            if (self.baseNameToken == PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_NAME")
            {
                if (Modules.Config.loweredVolume.Value)
                {
                    Util.PlaySound("PhoenixDyingQuiet", self.gameObject);
                }
                else Util.PlaySound("PhoenixDying", self.gameObject);
            }
        }

        private void CharacterModel_Awake(On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self)
        {
            orig(self);
            if (self.gameObject.name.Contains("PhoenixDisplay"))
            {
                if (Modules.Config.loweredVolume.Value)
                {
                    Util.PlaySound("PhoenixMenuSoundQuiet", self.gameObject);
                }
                else Util.PlaySound("PhoenixMenuSound", self.gameObject);
                currentStacks = 0;
                turnaboutActive = true;
            }
        }
    }
}