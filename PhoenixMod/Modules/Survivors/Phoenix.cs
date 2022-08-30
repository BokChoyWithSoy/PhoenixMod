using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhoenixWright.Modules.Survivors
{
    internal class Phoenix : SurvivorBase
    {
        internal override string bodyName { get; set; } = "Phoenix";

        internal static SkillDef primaryVase;
        internal static SkillDef primaryKnife;
        internal static SkillDef primaryPhone;
        internal static SkillDef primaryServbot;
        internal static SkillDef primaryBottle;
        internal static SkillDef primaryArm;
        internal static SkillDef primaryPaperRed;
        internal static SkillDef primaryPaperGreen;
        internal static SkillDef primaryPaperStrong;
        internal static SkillDef secondaryPress;
        internal static SkillDef secondaryPressStrong;        
        internal static SkillDef secondaryLock;
        internal static SkillDef secondaryLockStrong;
        internal static SkillDef rollSkillDef2;
        internal static SkillDef gavelStrong;
        internal static SkillDef tumbleStrongSkillDef;


        internal override GameObject bodyPrefab { get; set; }
        internal override GameObject displayPrefab { get; set; }

        internal override float sortPosition { get; set; } = 100f;

        internal override ConfigEntry<bool> characterEnabled { get; set; }

        internal override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            armor = 20f,
            armorGrowth = 1f,
            bodyName = "PhoenixBody",
            bodyNameToken = PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_NAME",
            bodyColor = Color.gray,
            characterPortrait = Modules.Assets.LoadCharacterIcon("Phoenix"),
            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            damage = 20f,
            healthGrowth = 20f,
            healthRegen = 2f,
            jumpCount = 1,
            maxHealth = 140f,
            subtitleNameToken = PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_SUBTITLE",
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
        };

        internal static Material phoenixMat = Modules.Assets.CreateMaterial("matPhoenix");
        internal override int mainRendererIndex { get; set; } = 0;

        internal override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] {
            new CustomRendererInfo
            {
                childName = "Model",
                material = phoenixMat
            }
        };

        internal override Type characterMainState { get; set; } = typeof(EntityStates.GenericCharacterMain);

        // item display stuffs
        internal override ItemDisplayRuleSet itemDisplayRuleSet { get; set; }
        internal override List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get; set; }

        internal override UnlockableDef characterUnlockableDef { get; set; }
        private static UnlockableDef masterySkinUnlockableDef;
        private static UnlockableDef swordSkinUnlockableDef;
        private static UnlockableDef dripSkinUnlockableDef;

        internal override void InitializeCharacter()
        {
            base.InitializeCharacter();
            PhoenixPlugin.phoenixController = bodyPrefab.AddComponent<PhoenixController>();
        }

        internal override void InitializeUnlockables()
        {
            masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MasteryAchievement>(true);
            swordSkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.SwordAchievement>(true);
            dripSkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.DripAchievement>(true);
        }

        internal override void InitializeDoppelganger()
        {
            base.InitializeDoppelganger();
        }

        internal override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("FallHitbox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "fall");
        }

        internal override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            string prefix = PhoenixPlugin.developerPrefix;

            #region Passive
            SkillLocator skillLoc = bodyPrefab.GetComponent<SkillLocator>();
            skillLoc.passiveSkill.enabled = true;
            skillLoc.passiveSkill.skillNameToken = PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PASSIVE_NAME";
            skillLoc.passiveSkill.skillDescriptionToken = PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PASSIVE_DESCRIPTION";
            skillLoc.passiveSkill.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBuffIcon");
            skillLoc.passiveSkill.keywordToken = "KEYWORD_TURNABOUT";
            #endregion

            #region primaryPaper
            Phoenix.primaryPaperRed = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_PRIMARY_PAPER_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_PRIMARY_PAPER_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_PRIMARY_PAPER_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPaperIconRed"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowPaper)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", "KEYWORD_STUNNING" }
            });

            Phoenix.primaryPaperStrong = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_PRIMARY_PAPER_STRONG_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_PRIMARY_PAPER_STRONG_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_PRIMARY_PAPER_STRONG_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPaperIconStrong"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowPaperStrong)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", "KEYWORD_STUNNING" }
            });

            #endregion

            #region PrimaryVase
            Phoenix.primaryVase = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texVaseIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowVase)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", "KEYWORD_STUNNING" }
            });

            Modules.Skills.AddPrimarySkill(bodyPrefab, Phoenix.primaryVase);
            Modules.Skills.AddPrimarySkill(bodyPrefab, Phoenix.primaryPaperRed);
            #endregion

            #region PrimaryKnife
            Phoenix.primaryKnife = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texKnifeIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowKnife)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", "KEYWORD_STUNNING" }
            });
            #endregion

            #region PrimaryPhone
            Phoenix.primaryPhone = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPhoneIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowPhone)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", "KEYWORD_STUNNING" }
            });
            #endregion

            #region PrimaryBottle
            Phoenix.primaryBottle = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBottleIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowBottle)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", "KEYWORD_STUNNING" }
            });
            #endregion

            #region PrimaryPhone
            Phoenix.primaryServbot = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texServbotIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowServbot)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", "KEYWORD_STUNNING" }
            });
            #endregion

            #region PrimaryArm
            Phoenix.primaryArm = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_PRIMARY_THROW2_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW2_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_PRIMARY_THROW2_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryUpgrade"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.SpawnArm)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", "KEYWORD_STUNNING" }
            });

            #endregion

            #region Secondary
            SkillDef secondaryPress = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_SECONDARY_PRESS_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_SECONDARY_PRESS_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_SECONDARY_PRESS_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPressIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Press)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_STUNNING", "KEYWORD SECONDARY" }
            });        

            Phoenix.secondaryPressStrong = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_SECONDARY_PRESS2_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_SECONDARY_PRESS2_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_SECONDARY_PRESS2_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryUpgradeIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PressTurnabout)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                 keywordTokens = new string[] { "KEYWORD_STUNNING" }
             });

            Modules.Skills.AddSecondarySkill(bodyPrefab, secondaryPress);
            #endregion

            #region Utility
            SkillDef rollSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_UTILITY_FALL_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_UTILITY_FALL_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_UTILITY_FALL_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texTripIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Fall)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_STUNNING", "KEYWORD UTILITY" }
            });

            Phoenix.rollSkillDef2 = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_UTILITY_FALL_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_UTILITY_FALL_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_UTILITY_FALL_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texTripIconStrong"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Fall)),
                activationStateMachineName = "Body",
                baseMaxStock = 2,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_STUNNING", "KEYWORD UTILITY" }
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, rollSkillDef);
            #endregion

            #region Special
            SkillDef gavelSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_SPECIAL_ORDER_NAME",
                skillNameToken = prefix + "_PHOENIX_BODY_SPECIAL_ORDER_NAME",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_SPECIAL_ORDER_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texGavelIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Gavel)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_STUNNING", "KEYWORD SPECIAL" }
            });

            Phoenix.gavelStrong = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PHOENIX_BODY_SPECIAL_ORDER_NAME2",
                skillNameToken = prefix + "_PHOENIX_BODY_SPECIAL_ORDER_NAME2",
                skillDescriptionToken = prefix + "_PHOENIX_BODY_SPECIAL_ORDER_DESCRIPTION2",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texGavelIconStrong"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.GavelStrong)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Pain,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_STUNNING", "KEYWORD SPECIAL" }
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, gavelSkillDef);
            #endregion

        }

        internal override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRenderers,
                mainRenderer,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshPhoenix"),
                    renderer = defaultRenderers[instance.mainRendererIndex].renderer
                }
            };

            skins.Add(defaultSkin);
            #endregion

            #region MasterySkin
            Material masteryMat = Modules.Assets.CreateMaterial("matPhoenixMastery");
            CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                masteryMat,
                masteryMat,
                masteryMat,
                masteryMat
            });

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasterySkin"),
                masteryRendererInfos,
                mainRenderer,
                model,
                masterySkinUnlockableDef);

            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshPhoenix"),
                    renderer = defaultRenderers[instance.mainRendererIndex].renderer
                }
            };

            skins.Add(masterySkin);
            #endregion

            #region SwordSkin
            Material swordMat = Modules.Assets.CreateMaterial("matPhoenixNaruhodo");
            CharacterModel.RendererInfo[] swordRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                swordMat,
                swordMat,
                swordMat,
                swordMat
            });

            SkinDef swordSkin = Modules.Skins.CreateSkinDef("Naruhodo",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasterySword"),
                swordRendererInfos,
                mainRenderer,
                model,
                swordSkinUnlockableDef);

            swordSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshPhoenix"),
                    renderer = defaultRenderers[instance.mainRendererIndex].renderer
                }
            };

            skins.Add(swordSkin);
            #endregion

            #region DripSkin
            Material dripMat = Modules.Assets.CreateMaterial("matPhoenixDrip");
            CharacterModel.RendererInfo[] dripRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                dripMat,
                dripMat,
                dripMat,
                dripMat
            });

            SkinDef dripSkin = Modules.Skins.CreateSkinDef("Drip",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texDripSkin"),
                dripRendererInfos,
                mainRenderer,
                model,
                dripSkinUnlockableDef);

            dripSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshPhoenix"),
                    renderer = defaultRenderers[instance.mainRendererIndex].renderer
                }
            };

            skins.Add(dripSkin);
            #endregion

            skinController.skins = skins.ToArray();
        }


        internal override void SetItemDisplays()
        {
            itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();

            // add item displays here
            //  HIGHLY recommend using KingEnderBrine's ItemDisplayPlacementHelper mod for this
            #region Item Displays

            //base game items

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/CritGlasses"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlasses"),
                            childName = "Head",
                            localPos = new Vector3(0.00008F, 0.11955F, 0.15313F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Syringe"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
                            childName = "Left Leg",
                            localPos = new Vector3(0.05131F, 0.0329F, 0.0165F),
                            localAngles = new Vector3(345.0316F, 186.6701F, 90.10183F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
                            childName = "Left Leg",
                            localPos = new Vector3(0.04972F, -0.03311F, 0.00881F),
                            localAngles = new Vector3(345.0316F, 186.6701F, 90.10183F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Behemoth"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBehemoth"),
                            childName = "Chest",
                            localPos = new Vector3(-0.02931F, -0.0009F, -0.21935F),
                            localAngles = new Vector3(6.223F, 180F, 0F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Missile"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
                            childName = "Chest",
                            localPos = new Vector3(-0.45543F, 0.35285F, -0.06724F),
                            localAngles = new Vector3(0F, 0F, 51.9225F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Dagger"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDagger"),
                            childName = "Chest",
                            localPos = new Vector3(-0.02887F, 0.09794F, -0.03726F),
                            localAngles = new Vector3(334.8839F, 31.5284F, 34.67839F),
                            localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Hoof"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHoof"),
                            childName = "Left Leg",
                            localPos = new Vector3(-0.00121F, 0.39142F, 0.06504F),
                            localAngles = new Vector3(82.65324F, 182.0755F, 5.49788F),
                            localScale = new Vector3(0.1F, 0.15F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/ChainLightning"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayUkulele"),
                            childName = "Chest",
                            localPos = new Vector3(0.03924F, -0.39091F, -0.14721F),
                            localAngles = new Vector3(0F, 180F, 89.3997F),
                            localScale = new Vector3(0.4749F, 0.4749F, 0.4749F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/GhostOnKill"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMask"),
                            childName = "Head",
                            localPos = new Vector3(-0.00011F, 0.11735F, 0.10443F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Mushroom"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMushroom"),
                            childName = "Left Shoulder",
                            localPos = new Vector3(0.04594F, 0.1408F, -0.05753F),
                            localAngles = new Vector3(69.84948F, 163.9957F, 144.0787F),
                            localScale = new Vector3(0.0501F, 0.0501F, 0.0501F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/AttackSpeedOnCrit"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWolfPelt"),
                            childName = "Head",
                            localPos = new Vector3(-0.00688F, 0.17887F, 0.11809F),
                            localAngles = new Vector3(358.4554F, 0F, 0F),
                            localScale = new Vector3(0.5666F, 0.5666F, 0.5666F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/BleedOnHit"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTriTip"),
                            childName = "Chest",
                            localPos = new Vector3(-0.12992F, 0.24389F, 0.11903F),
                            localAngles = new Vector3(11.5211F, 128.5354F, 165.922F),
                            localScale = new Vector3(0.2615F, 0.2615F, 0.2615F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/WardOnLevel"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarbanner"),
                            childName = "Chest",
                            localPos = new Vector3(0.03597F, 0.21475F, -0.09037F),
                            localAngles = new Vector3(0F, 0F, 90F),
                            localScale = new Vector3(0.3162F, 0.3162F, 0.3162F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/HealOnCrit"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScythe"),
                            childName = "Chest",
                            localPos = new Vector3(0.01922F, -0.09922F, -0.15849F),
                            localAngles = new Vector3(323.9545F, 90F, 270F),
                            localScale = new Vector3(0.1884F, 0.1884F, 0.1884F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/HealWhileSafe"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySnail"),
                            childName = "Head",
                            localPos = new Vector3(0.04469F, 0.07924F, 0.13367F),
                            localAngles = new Vector3(90F, 0F, 0F),
                            localScale = new Vector3(0.0289F, 0.0289F, 0.0289F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Clover"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayClover"),
                            childName = "Left Shoulder",
                            localPos = new Vector3(-0.00379F, 0.0774F, -0.0857F),
                            localAngles = new Vector3(85.61921F, 0.0001F, 179.4897F),
                            localScale = new Vector3(0.2749F, 0.2749F, 0.2749F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/BarrierOnOverHeal"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAegis"),
                            childName = "Right Arm Lower",
                            localPos = new Vector3(-0.03753F, -0.03772F, -0.06834F),
                            localAngles = new Vector3(89.25664F, 25.93928F, 28.03237F),
                            localScale = new Vector3(0.2849F, 0.2849F, 0.2849F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/GoldOnHit"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBoneCrown"),
                            childName = "Head",
                            localPos = new Vector3(-0.00235F, 0.11986F, 0.00528F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(1.1754F, 1.1754F, 1.1754F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/WarCryOnMultiKill"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPauldron"),
                            childName = "Left Arm Upper",
                            localPos = new Vector3(0.00587F, 0.0456F, -0.05634F),
                            localAngles = new Vector3(81.73647F, 205.3085F, 12.81184F),
                            localScale = new Vector3(0.7094F, 0.7094F, 0.7094F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/SprintArmor"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBuckler"),
                            childName = "Right Leg",
                            localPos = new Vector3(0.02165F, 0.29162F, 0.07043F),
                            localAngles = new Vector3(14.01751F, 357.4532F, 188.8993F),
                            localScale = new Vector3(0.1971F, 0.1971F, 0.1971F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/IceRing"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayIceRing"),
                            childName = "Right Arm Lower",
                            localPos = new Vector3(0.04431F, 0.30457F, -0.0085F),
                            localAngles = new Vector3(274.3965F, 90F, 270F),
                            localScale = new Vector3(0.3627F, 0.3627F, 0.3627F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/FireRing"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFireRing"),
                            childName = "Left Arm Lower",
                            localPos = new Vector3(0.0196F, 0.30909F, -0.00882F),
                            localAngles = new Vector3(274.3965F, 90F, 270F),
                            localScale = new Vector3(0.3627F, 0.3627F, 0.3627F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/UtilitySkillMagazine"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                            childName = "Right Foot",
                            localPos = new Vector3(0F, 0F, 0F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.6F, 0.6F, 0.6F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                            childName = "Left Foot",
                            localPos = new Vector3(0F, 0F, 0F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.6F, 0.6F, 0.6F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/JumpBoost"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWaxBird"),
                            childName = "Head",
                            localPos = new Vector3(-0.00493F, -0.05193F, -0.08535F),
                            localAngles = new Vector3(24.419F, 0F, 0F),
                            localScale = new Vector3(0.5253F, 0.5253F, 0.5253F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/ArmorReductionOnHit"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "Chest",
                            localPos = new Vector3(0.02134F, -0.19953F, -0.16994F),
                            localAngles = new Vector3(64.18901F, 90F, 90F),
                            localScale = new Vector3(0.1722F, 0.1722F, 0.1722F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/NearbyDamageBonus"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDiamond"),
                            childName = "Chest",
                            localPos = new Vector3(0.13503F, -0.2352F, 0.15925F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.1236F, 0.1236F, 0.1236F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/ArmorPlate"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
                            childName = "Left Leg",
                            localPos = new Vector3(0.02783F, 0.11808F, 0.05415F),
                            localAngles = new Vector3(90F, 180F, 0F),
                            localScale = new Vector3(0.1971F, 0.1971F, 0.1971F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/CommandMissile"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileRack"),
                                childName = "Chest",
                                localPos = new Vector3(-0.0101F, 0.06735F, -0.14687F),
                                localAngles = new Vector3(90F, 180F, 0F),
                                localScale = new Vector3(0.3362F, 0.3362F, 0.3362F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Feather"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFeather"),
                            childName = "Left Arm Upper",
                            localPos = new Vector3(0.0065F, 0.05863F, 0.03207F),
                            localAngles = new Vector3(302.2662F, 244.1406F, 154.5072F),
                            localScale = new Vector3(0.0285F, 0.0285F, 0.0285F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFeather"),
                            childName = "Left Arm Upper",
                            localPos = new Vector3(-0.03053F, 0.13951F, 0.03192F),
                            localAngles = new Vector3(290.8631F, 257.5243F, 137.2719F),
                            localScale = new Vector3(0.0285F, 0.0285F, 0.0285F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Crowbar"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCrowbar"),
                            childName = "Chest",
                            localPos = new Vector3(-0.01991F, 0.10769F, -0.14058F),
                            localAngles = new Vector3(90F, 90F, 0F),
                            localScale = new Vector3(0.1936F, 0.1936F, 0.1936F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/FallBoots"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
                            childName = "Left Foot",
                            localPos = new Vector3(0.00697F, 0.12576F, 0.00381F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.1485F, 0.1485F, 0.1485F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
                            childName = "Right Foot",
                            localPos = new Vector3(0.01368F, 0.12732F, 0.00864F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.1485F, 0.1485F, 0.1485F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/ExecuteLowHealthElite"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGuillotine"),
                            childName = "Right Arm Upper",
                            localPos = new Vector3(-0.10293F, 0.11881F, -0.18759F),
                            localAngles = new Vector3(8.57194F, 201.4851F, 88.21832F),
                            localScale = new Vector3(0.1843F, 0.1843F, 0.1843F),    
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/EquipmentMagazine"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBattery"),
                            childName = "Chest",
                            localPos = new Vector3(-0.12984F, 0.09352F, 0.13038F),
                            localAngles = new Vector3(0F, 270F, 292.8411F),
                            localScale = new Vector3(0.0773F, 0.0773F, 0.0773F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/NovaOnHeal"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                            childName = "Head",
                            localPos = new Vector3(0.04966F, 0.18214F, 0.04931F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.5349F, 0.5349F, 0.5349F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                            childName = "Head",
                            localPos = new Vector3(-0.04336F, 0.16517F, 0.00733F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(-0.5349F, 0.5349F, 0.5349F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Infusion"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayInfusion"),
                            childName = "Hips",
                            localPos = new Vector3(0.10411F, -0.05932F, 0.13562F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.5253F, 0.5253F, 0.5253F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Medkit"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMedkit"),
                            childName = "Hips",
                            localPos = new Vector3(0.0247F, -0.02118F, -0.15778F),
                            localAngles = new Vector3(281.5152F, 22.21357F, 158.9374F),
                            localScale = new Vector3(0.4907F, 0.4907F, 0.4907F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Bandolier"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBandolier"),
                            childName = "Left Leg",
                            localPos = new Vector3(0.0035F, 0F, 0F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.1684F, 0.242F, 0.242F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/BounceNearby"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHook"),
                            childName = "Head",
                            localPos = new Vector3(-0.00789F, 0.12003F, -0.10451F),
                            localAngles = new Vector3(301.7833F, 235.8446F, 22.23744F),
                            localScale = new Vector3(0.214F, 0.214F, 0.214F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/IgniteOnKill"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGasoline"),
                            childName = "Left Arm Lower",
                            localPos = new Vector3(-0.00446F, 0.21735F, -0.0619F),
                            localAngles = new Vector3(90F, 90F, 0F),
                            localScale = new Vector3(0.3165F, 0.3165F, 0.3165F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/StunChanceOnHit"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStunGrenade"),
                            childName = "Hips",
                            localPos = new Vector3(0.08823F, -0.01596F, 0.15026F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.5672F, 0.5672F, 0.5672F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Firework"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFirework"),
                            childName = "Left Leg",
                            localPos = new Vector3(0.09247F, -0.06814F, 0.00142F),
                            localAngles = new Vector3(90F, 0F, 0F),
                            localScale = new Vector3(0.1194F, 0.1194F, 0.1194F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/LunarDagger"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarDagger"),
                            childName = "Chest",
                            localPos = new Vector3(0.03663F, 0.09482F, -0.19738F),
                            localAngles = new Vector3(351.0772F, 4.81773F, 78.60245F),
                            localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Knurl"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKnurl"),
                            childName = "Left Shoulder",
                            localPos = new Vector3(0.0231F, 0.09327F, -0.09655F),
                            localAngles = new Vector3(78.87074F, 36.6722F, 105.8275F),
                            localScale = new Vector3(0.0848F, 0.0848F, 0.0848F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/BeetleGland"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBeetleGland"),
                            childName = "Left Foot",
                            localPos = new Vector3(-0.00895F, 0.13059F, 0.07611F),
                            localAngles = new Vector3(359.9584F, 0.1329F, 39.8304F),
                            localScale = new Vector3(0.0553F, 0.0553F, 0.0553F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/SprintBonus"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySoda"),
                            childName = "Head",
                            localPos = new Vector3(-0.00277F, 0.03038F, 0.14772F),
                            localAngles = new Vector3(-0.00001F, 270F, 0.00001F),
                            localScale = new Vector3(0.1655F, 0.1655F, 0.1655F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/SecondarySkillMagazine"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDoubleMag"),
                            childName = "Chest",
                            localPos = new Vector3(-0.0018F, 0.0002F, 0.097F),
                            localAngles = new Vector3(84.2709F, 200.5981F, 25.0139F),
                            localScale = new Vector3(0.0441F, 0.0441F, 0.0441F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/StickyBomb"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStickyBomb"),
                            childName = "Right Arm Lower",
                            localPos = new Vector3(-0.00483F, 0.13034F, -0.08934F),
                            localAngles = new Vector3(303.257F, 139.7009F, 216.6799F),
                            localScale = new Vector3(0.0736F, 0.0736F, 0.0736F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/TreasureCache"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKey"),
                            childName = "Chest",
                            localPos = new Vector3(0.16773F, -0.25853F, 0.11086F),
                            localAngles = new Vector3(1.68028F, 157.0609F, 89.28107F),
                            localScale = new Vector3(0.4092F, 0.4092F, 0.4092F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/BossDamageBonus"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAPRound"),
                            childName = "Chest",
                            localPos = new Vector3(0.14605F, -0.04777F, 0.14952F),
                            localAngles = new Vector3(90F, 41.5689F, 0F),
                            localScale = new Vector3(0.2279F, 0.2279F, 0.2279F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/SlowOnHit"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBauble"),
                            childName = "Right Hand",
                            localPos = new Vector3(0.26274F, 0.41467F, 0.24462F),
                            localAngles = new Vector3(330.8744F, 178.5556F, 178.3457F),
                            localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/ExtraLife"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHippo"),
                            childName = "Chest",
                            localPos = new Vector3(-0.07884F, 0.06188F, 0.18508F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.2645F, 0.2645F, 0.2645F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/KillEliteFrenzy"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrainstalk"),
                            childName = "Head",
                            localPos = new Vector3(0F, 0.1882F, 0F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.2638F, 0.2638F, 0.2638F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/RepeatHeal"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCorpseFlower"),
                            childName = "Head",
                            localPos = new Vector3(-0.08303F, 0.11818F, 0.01636F),
                            localAngles = new Vector3(270F, 90F, 0F),
                            localScale = new Vector3(0.1511F, 0.1511F, 0.1511F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/AutoCastEquipment"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFossil"),
                            childName = "Chest",
                            localPos = new Vector3(-0.13963F, 0.10326F, 0.11421F),
                            localAngles = new Vector3(354.3306F, 98.70874F, 342.6167F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/IncreaseHealing"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
                            childName = "Head",
                            localPos = new Vector3(0.0896F, 0.1987F, 0.00443F),
                            localAngles = new Vector3(0F, 90F, 0F),
                            localScale = new Vector3(0.3395F, 0.3395F, 0.3395F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
                            childName = "Head",
                            localPos = new Vector3(-0.08451F, 0.2095F, 0.00505F),
                            localAngles = new Vector3(0F, 90F, 0F),
                            localScale = new Vector3(0.3395F, 0.3395F, -0.3395F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/TitanGoldDuringTP"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldHeart"),
                            childName = "Chest",
                            localPos = new Vector3(-0.09608F, 0.0815F, 0.15113F),
                            localAngles = new Vector3(335.0033F, 343.2951F, 0F),
                            localScale = new Vector3(0.1191F, 0.1191F, 0.1191F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/SprintWisp"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrokenMask"),
                            childName = "Left Hand",
                            localPos = new Vector3(0.03224F, 0.09353F, -0.03892F),
                            localAngles = new Vector3(0F, 180F, 0F),
                            localScale = new Vector3(0.1385F, 0.1385F, 0.1385F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/BarrierOnKill"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrooch"),
                            childName = "Chest",
                            localPos = new Vector3(-0.00124F, 0.00144F, 0.1546F),
                            localAngles = new Vector3(74.11434F, 345.4745F, 352.0098F),
                            localScale = new Vector3(0.1841F, 0.1841F, 0.1841F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/TPHealingNova"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlowFlower"),
                            childName = "Chest",
                            localPos = new Vector3(0.09064F, 0.14942F, 0.11668F),
                            localAngles = new Vector3(328.9383F, 3.18673F, 0.38337F),
                            localScale = new Vector3(0.2731F, 0.2731F, 0.0273F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/LunarUtilityReplacement"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdFoot"),
                            childName = "Hips",
                            localPos = new Vector3(-0.0414F, -0.03764F, -0.19106F),
                            localAngles = new Vector3(0F, 270F, 0F),
                            localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Thorns"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
                            childName = "Right Arm Upper",
                            localPos = new Vector3(-0.03348F, 0.01582F, 0.02884F),
                            localAngles = new Vector3(277.5331F, 227.8641F, 108.0806F),
                            localScale = new Vector3(0.8F, 0.8F, 0.4F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/LunarPrimaryReplacement"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdEye"),
                            childName = "Head",
                            localPos = new Vector3(0F, 0.1738F, 0.1375F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.2866F, 0.2866F, 0.2866F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/NovaOnLowHealth"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayJellyGuts"),
                            childName = "Head",
                            localPos = new Vector3(-0.04569F, -0.0583F, 0.01711F),
                            localAngles = new Vector3(323.315F, 13.30625F, 347.5849F),
                            localScale = new Vector3(0.15F, 0.1035F, 0.1035F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/LunarTrinket"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBeads"),
                            childName = "Right Leg",
                            localPos = new Vector3(-0.01661F, 0.08033F, 0.05544F),
                            localAngles = new Vector3(14.71353F, 358.1346F, 124.9263F),
                            localScale = new Vector3(2F, 2F, 2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Plant"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
                            childName = "Right Shoulder",
                            localPos = new Vector3(0.01553F, 0.05798F, -0.1214F),
                            localAngles = new Vector3(0F, 180F, 0F),
                            localScale = new Vector3(0.0429F, 0.0429F, 0.0429F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Bear"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBear"),
                            childName = "Chest",
                            localPos = new Vector3(0.09486F, -0.00042F, 0.17129F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.2034F, 0.2034F, 0.2034F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/DeathMark"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathMark"),
                            childName = "Hips",
                            localPos = new Vector3(-0.19085F, -0.16532F, -0.13452F),
                            localAngles = new Vector3(90F, 180F, 0F),
                            localScale = new Vector3(-0.0375F, -0.0341F, -0.0464F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/ExplodeOnDeath"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWilloWisp"),
                            childName = "Hips",
                            localPos = new Vector3(-0.19991F, -0.08686F, 0.11345F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.0283F, 0.0283F, 0.0283F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Seed"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySeed"),
                            childName = "Head",
                            localPos = new Vector3(-0.06678F, 0.08396F, 0.12996F),
                            localAngles = new Vector3(324.4779F, 117.6807F, 324.6905F),
                            localScale = new Vector3(0.01F, 0.01F, 0.01F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/SprintOutOfCombat"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWhip"),
                            childName = "Right Leg",
                            localPos = new Vector3(-0.08634F, -0.09948F, 0.00375F),
                            localAngles = new Vector3(356.2555F, 359.7118F, 2.87476F),
                            localScale = new Vector3(0.2845F, 0.2845F, 0.2845F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Phasing"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStealthkit"),
                            childName = "Right Leg",
                            localPos = new Vector3(0.00218F, -0.17353F, 0.06859F),
                            localAngles = new Vector3(90F, 180F, 0F),
                            localScale = new Vector3(0.19058F, 0.2399F, 0.21498F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/PersonalShield"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
                            childName = "Chest",
                            localPos = new Vector3(-0.01242F, -0.09935F, 0.16888F),
                            localAngles = new Vector3(304.1204F, 90F, 270F),
                            localScale = new Vector3(0.1057F, 0.1057F, 0.1057F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/ShockNearby"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
                            childName = "Chest",
                            localPos = new Vector3(-0.0216F, 0.1257F, -0.13219F),
                            localAngles = new Vector3(297.6866F, 1.3864F, 358.5596F),
                            localScale = new Vector3(0.3229F, 0.3229F, 0.3229F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/ShieldOnly"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
                            childName = "Head",
                            localPos = new Vector3(0.08606F, 0.25265F, 0.0087F),
                            localAngles = new Vector3(348.1819F, 268.0985F, 0.3896F),
                            localScale = new Vector3(0.3521F, 0.3521F, 0.3521F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
                            childName = "Head",
                            localPos = new Vector3(-0.08763F, 0.24565F, 0.00974F),
                            localAngles = new Vector3(11.8181F, 268.0985F, 359.6104F),
                            localScale = new Vector3(0.3521F, 0.3521F, -0.3521F),   
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/AlienHead"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAlienHead"),
                            childName = "Head",
                            localPos = new Vector3(0.01651F, 0.35434F, -0.10006F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(2F, 2F, 2F),
                            limbMask = LimbFlags.None   
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/HeadHunter"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySkullCrown"),
                            childName = "Head",
                            localPos = new Vector3(-0.0028F, 0.18483F, 0.00631F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.38037F, 0.14175F, 0.1546F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/EnergizedOnEquipmentUse"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarHorn"),
                            childName = "Head",
                            localPos = new Vector3(-0.05064F, 0.06858F, 0.16446F),
                            localAngles = new Vector3(16.84178F, 258.5842F, 42.69091F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/FlatHealth"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySteakCurved"),
                            childName = "Head",
                            localPos = new Vector3(-0.00271F, 0.24239F, -0.09331F),
                            localAngles = new Vector3(319.4152F, 183.6783F, 186.0788F),
                            localScale = new Vector3(0.1245F, 0.1155F, 0.1155F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Tooth"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
                            childName = "Head",
                            localPos = new Vector3(-0.00446F, 0.02273F, 0.14747F),
                            localAngles = new Vector3(344.9017F, 0F, 0F),
                            localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Pearl"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPearl"),
                            childName = "Chest",
                            localPos = new Vector3(0F, 0F, 0F),
                            localAngles = new Vector3(90F, 0F, 0F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/ShinyPearl"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShinyPearl"),
                            childName = "Chest",
                            localPos = new Vector3(-0.03951F, -0.3283F, 0.00238F),
                            localAngles = new Vector3(90F, 0F, 0F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/BonusGoldPackOnKill"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTome"),
                            childName = "Chest",
                            localPos = new Vector3(-0.02862F, -0.28473F, 0.15297F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.0475F, 0.0475F, 0.0475F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Squid"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySquidTurret"),
                            childName = "Head",
                            localPos = new Vector3(-0.01262F, 0.2572F, 0.00161F),
                            localAngles = new Vector3(0F, 90F, 0F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Icicle"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFrostRelic"),
                            childName = "Head",
                            localPos = new Vector3(0.02003F, 0.37496F, 0.11125F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/Talisman"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTalisman"),
                            childName = "Chest",
                            localPos = new Vector3(0.8942F, -0.06959F, -0.30334F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/LaserTurbine"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
                            childName = "Chest",
                            localPos = new Vector3(0.02098F, 0.01822F, 0.14412F),
                            localAngles = new Vector3(341.9818F, 0.75081F, 359.2094F),
                            localScale = new Vector3(0.2159F, 0.2159F, 0.2159F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/FocusConvergence"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
                            childName = "Chest",
                            localPos = new Vector3(-0.59788F, 0.61425F, -0.0637F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });


            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/FireballsOnHit"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
                            childName = "Chest",
                            localPos = new Vector3(0.01163F, 0.16095F, -0.14337F),
                            localAngles = new Vector3(325.188F, 171.7754F, 192.8122F),
                            localScale = new Vector3(0.0761F, 0.0761F, 0.0761F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/SiphonOnLowHealth"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
                            childName = "Right Arm Upper",
                            localPos = new Vector3(-0.04825F, 0.13126F, -0.07715F),
                            localAngles = new Vector3(0F, 303.4368F, 0F),
                            localScale = new Vector3(0.0385F, 0.0385F, 0.0385F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/BleedOnHitAndExplode"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
                            childName = "Hips",
                            localPos = new Vector3(0.16036F, -0.16303F, -0.10576F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.0486F, 0.0486F, 0.0486F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/MonstersOnShrineUse"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
                            childName = "Chest",
                            localPos = new Vector3(0.11751F, 0.078F, 0.1363F),
                            localAngles = new Vector3(0.66624F, 291.2867F, 29.90962F),
                            localScale = new Vector3(0.0246F, 0.0246F, 0.0246F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/RandomDamageZone"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
                            childName = "Right Arm Upper",
                            localPos = new Vector3(-0.02388F, 0.01735F, -0.1126F),
                            localAngles = new Vector3(328.3619F, 356.4809F, 8.21707F),
                            localScale = new Vector3(0.0465F, 0.0465F, 0.0465F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            //SOTV Item displays

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.AttackSpeedAndMoveSpeed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCoffee"),
                            childName = "Right Leg",
                            localPos = new Vector3(-0.10985F, 0.12737F, 0.01514F),
                            localAngles = new Vector3(-0.00001F, 180F, 180F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.BearVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBearVoid"),
                            childName = "Chest",
                            localPos = new Vector3(0.11009F, 0.02129F, 0.16743F),
                            localAngles = new Vector3(352.4048F, 344.3428F, 8.18645F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
        }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.BleedOnHitVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTriTipVoid"),
                            childName = "Chest",
                            localPos = new Vector3(-0.2639F, 0.27048F, -0.25197F),
                            localAngles = new Vector3(35.9665F, 27.56639F, 14.05124F),
                            localScale = new Vector3(0.4F, 0.4F, 0.4F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ChainLightningVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayUkuleleVoid"),
                            childName = "Chest",
                            localPos = new Vector3(-0.01149F, 0.01341F, -0.15973F),
                            localAngles = new Vector3(349.274F, 178.7936F, 14.3271F),
                            localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.CloverVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCloverVoid"),
                            childName = "Head",
                            localPos = new Vector3(-0.09434F, 0.20207F, 0.05097F),
                            localAngles = new Vector3(319.3075F, 15.57478F, 86.9045F),
                            localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.CritDamage,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLaserSight"),
                            childName = "Right Arm Lower",
                            localPos = new Vector3(-0.01398F, 0.28569F, -0.03613F),
                            localAngles = new Vector3(5.85999F, 112.4123F, 278.7357F),
                            localScale = new Vector3(0.0465F, 0.0465F, 0.0465F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ElementalRingVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayVoidRing"),
                            childName = "Right Arm Lower",
                            localPos = new Vector3(0.00341F, 0.32236F, 0.01401F),
                            localAngles = new Vector3(287.7512F, 265.4616F, 105.7352F),
                            localScale = new Vector3(0.3F, 0.3F, 0.3F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.CritGlassesVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlassesVoid"),
                            childName = "Head",
                            localPos = new Vector3(0.00135F, 0.12765F, 0.14661F),
                            localAngles = new Vector3(347.2212F, 356.7163F, 7.996F),
                            localScale = new Vector3(0.14788F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.LunarSun,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySunHead"),
                            childName = "Head",
                            localPos = new Vector3(0.00698F, 0.19833F, 0.02922F),
                            localAngles = new Vector3(328.3619F, 356.4809F, 8.21707F),
                            localScale = new Vector3(1.2F, 1.2F, 1.2F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySunHeadNeck"),
                            childName = "Head",
                            localPos = new Vector3(-0.00695F, -0.02869F, 0.06769F),
                            localAngles = new Vector3(357.1531F, 20.53568F, 0.89384F),
                            localScale = new Vector3(1.3F, 1.3F, 1.3F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.EquipmentMagazineVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFuelCellVoid"),
                            childName = "Chest",
                            localPos = new Vector3(-0.0242F, 0.01376F, -0.17823F),
                            localAngles = new Vector3(17.6067F, 333.4188F, 13.4999F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ExplodeOnDeathVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWillowWispVoid"),
                            childName = "Hips",
                            localPos = new Vector3(-0.17576F, 0.0017F, 0.11115F),
                            localAngles = new Vector3(328.3619F, 356.4809F, 8.21707F),
                            localScale = new Vector3(0.0465F, 0.0465F, 0.0465F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ExtraLifeVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHippoVoid"),
                            childName = "Chest",
                            localPos = new Vector3(-0.00847F, 0.0759F, -0.13269F),
                            localAngles = new Vector3(347.3998F, 167.992F, 11.25176F),
                            localScale = new Vector3(0.4F, 0.4F, 0.4F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.FragileDamageBonus,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDelicateWatch"),
                            childName = "Left Arm Lower",
                            localPos = new Vector3(-0.01135F, 0.30127F, -0.00683F),
                            localAngles = new Vector3(274.331F, 27.58414F, 334.2618F),
                            localScale = new Vector3(0.7F, 0.8F, 0.7F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.FreeChest,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShippingRequestForm"),
                            childName = "Left Leg",
                            localPos = new Vector3(0.09325F, 0.1477F, 0.03634F),
                            localAngles = new Vector3(273.9354F, 76.54321F, 186.1321F),
                            localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.GoldOnHurt,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRollOfPennies"),
                            childName = "Left Arm Lower",
                            localPos = new Vector3(0.07616F, 0.14417F, -0.02815F),
                            localAngles = new Vector3(14.41057F, 105.9124F, 157.6283F),
                            localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.HalfAttackSpeedHalfCooldowns,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarShoulderNature"),
                            childName = "Left Arm Upper",
                            localPos = new Vector3(-0.01345F, -0.03115F, -0.04162F),
                            localAngles = new Vector3(359.7319F, 106.0152F, 214.1257F),
                            localScale = new Vector3(0.7F, 0.7F, 0.7F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.HalfSpeedDoubleHealth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarShoulderStone"),
                            childName = "Right Arm Upper",
                            localPos = new Vector3(-0.0187F, -0.01035F, -0.06246F),
                            localAngles = new Vector3(337.6837F, 100.9652F, 195.5736F),
                            localScale = new Vector3(0.7F, 0.7F, 0.7F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.HealingPotion,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHealingPotion"),
                            childName = "Hips",
                            localPos = new Vector3(0.10886F, 0.12749F, 0.16503F),
                            localAngles = new Vector3(17.34065F, 275.3904F, 14.8417F),
                            localScale = new Vector3(0.05F, 0.05F, 0.05F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ImmuneToDebuff,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRainCoatBelt"),
                            childName = "Hips",
                            localPos = new Vector3(0.01552F, 0.03508F, 0.06727F),
                            localAngles = new Vector3(3.65836F, 13.23351F, 352.7898F),
                            localScale = new Vector3(1.1F, 1.1F, 1.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MissileVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileLauncherVoid"),
                            childName = "Chest",
                            localPos = new Vector3(-0.35974F, 0.4021F, -0.22142F),
                            localAngles = new Vector3(346.6386F, 345.6217F, 32.2748F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MoreMissile,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayICBM"),
                            childName = "Right Arm Upper",
                            localPos = new Vector3(-0.00884F, 0.15095F, -0.1085F),
                            localAngles = new Vector3(341.5276F, 103.6745F, 171.1494F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MoveSpeedOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGrappleHook"),
                            childName = "Right Arm Lower",
                            localPos = new Vector3(0.23818F, 0.13324F, 0.01027F),
                            localAngles = new Vector3(273.9353F, 76.5432F, 186.1321F),
                            localScale = new Vector3(0.3F, 0.3F, 0.3F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MushroomVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMushroomVoid"),
                            childName = "Right Arm Upper",
                            localPos = new Vector3(-0.00442F, -0.01537F, 0.02413F),
                            localAngles = new Vector3(0.44384F, 70.59093F, 224.4747F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.OutOfCombatArmor,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayOddlyShapedOpal"),
                            childName = "Chest",
                            localPos = new Vector3(0.00114F, 0.07736F, 0.12514F),
                            localAngles = new Vector3(334.5104F, 357.6981F, 189.5376F),
                            localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.PermanentDebuffOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScorpion"),
                            childName = "Hips",
                            localPos = new Vector3(-0.02696F, 0.02094F, -0.12116F),
                            localAngles = new Vector3(12.98954F, 359.3605F, 358.4683F),
                            localScale = new Vector3(1.5F, 1.5F, 1.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.PrimarySkillShuriken,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShuriken"),
                            childName = "Right Arm Lower",
                            localPos = new Vector3(-0.01129F, 0.37265F, -0.01793F),
                            localAngles = new Vector3(273.9353F, 76.5432F, 186.1321F),
                            localScale = new Vector3(0.6F, 0.6F, 0.6F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.RandomEquipmentTrigger,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBottledChaos"),
                            childName = "Left Arm Upper",
                            localPos = new Vector3(0.0413F, 0.12869F, -0.09363F),
                            localAngles = new Vector3(355.2653F, 193.9402F, 197.8134F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.RandomlyLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDomino"),
                            childName = "Chest",
                            localPos = new Vector3(0.60765F, 0.75336F, 0.07805F),
                            localAngles = new Vector3(274.9098F, 59.8631F, 145.5208F),
                            localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.RegeneratingScrap,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRegeneratingScrap"),
                            childName = "Head",
                            localPos = new Vector3(0.07823F, 0.17631F, -0.06869F),
                            localAngles = new Vector3(18.99674F, 354.5888F, 19.72143F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.SlowOnHitVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBaubleVoid"),
                            childName = "Right Leg",
                            localPos = new Vector3(-0.39872F, 0.1144F, 0.23546F),
                            localAngles = new Vector3(273.9353F, 76.5432F, 186.1321F),
                            localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.StrengthenBurn,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGasTank"),
                            childName = "Chest",
                            localPos = new Vector3(0.04611F, -0.35589F, 0.16849F),
                            localAngles = new Vector3(273.9216F, 74.46684F, 203.5066F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.TreasureCacheVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKeyVoid"),
                            childName = "Hips",
                            localPos = new Vector3(-0.22026F, 0.04781F, 0.02632F),
                            localAngles = new Vector3(5.27006F, 173.8339F, 90.50729F),
                            localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.VoidMegaCrabItem,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMegaCrabItem"),
                            childName = "Hips",
                            localPos = new Vector3(0.0061F, -0.14232F, 0.15262F),
                            localAngles = new Vector3(7.66534F, 7.6849F, 183.3206F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            //Base Game Equipment Displays

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/Fruit"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFruit"),
                            childName = "Right Arm Upper",
                            localPos = new Vector3(0.20576F, 0.1442F, 0.07384F),
                            localAngles = new Vector3(1.36894F, 319.8027F, 170.8728F),
                            localScale = new Vector3(0.2118F, 0.2118F, 0.2118F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/AffixRed"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3(0.07658F, 0.19903F, 0.05725F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.1036F, 0.1036F, 0.1036F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3(-0.07844F, 0.20858F, 0.04059F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(-0.1036F, 0.1036F, 0.1036F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/AffixBlue"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(-0.00275F, 0.18601F, 0.16221F),
                            localAngles = new Vector3(315F, 0F, 0F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(-0.00014F, 0.23012F, 0.14643F),
                            localAngles = new Vector3(300F, 0F, 0F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/AffixWhite"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
                            childName = "Head",
                            localPos = new Vector3(-0.00582F, 0.25185F, 0.03086F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.02F, 0.02F, 0.02F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/AffixPoison"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
                            childName = "Head",
                            localPos = new Vector3(0.00032F, 0.20361F, 0.00592F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.0496F, 0.0496F, 0.0496F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/AffixHaunted"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
                            childName = "Head",
                            localPos = new Vector3(-0.00108F, 0.19782F, 0.04735F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.03468F, 0.03F, 0.0392F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/CritOnUse"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
                            childName = "Head",
                            localPos = new Vector3(-0.00191F, 0.13451F, 0.23858F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.2326F, 0.2326F, 0.2326F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/DroneBackup"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRadio"),
                            childName = "Chest",
                            localPos = new Vector3(0.1334F, 0.11309F, 0.11981F),
                            localAngles = new Vector3(340.2856F, 1.22042F, 359.3574F),
                            localScale = new Vector3(0.2641F, 0.2641F, 0.2641F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/Lightning"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
                            childName = "Left Shoulder",
                            localPos = new Vector3(0.05778F, 0.18172F, -0.07366F),
                            localAngles = new Vector3(277.6853F, 78.85683F, 291.4521F),
                            localScale = new Vector3(0.3413F, 0.3413F, 0.3413F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/BurnNearby"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPotion"),
                            childName = "Chest",
                            localPos = new Vector3(-0.16863F, -0.29585F, 0.14104F),
                            localAngles = new Vector3(359.1402F, 0.1068F, 331.8908F),
                            localScale = new Vector3(0.0307F, 0.0307F, 0.0307F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/CrippleWard"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEffigy"),
                            childName = "Chest",
                            localPos = new Vector3(-0.13518F, -0.15361F, 0.17434F),
                            localAngles = new Vector3(0F, 180F, 0F),
                            localScale = new Vector3(0.2812F, 0.2812F, 0.2812F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/QuestVolatileBattery"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBatteryArray"),
                            childName = "Chest",
                            localPos = new Vector3(-0.00496F, -0.03855F, -0.19404F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.2188F, 0.2188F, 0.2188F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/GainArmor"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayElephantFigure"),
                            childName = "Right Shoulder",
                            localPos = new Vector3(0.01201F, 0.02871F, -0.16037F),
                            localAngles = new Vector3(270F, 0.00001F, 0F),
                            localScale = new Vector3(0.6279F, 0.6279F, 0.6279F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/Recycle"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRecycler"),
                            childName = "Chest",
                            localPos = new Vector3(-0.02751F, -0.0253F, -0.19446F),
                            localAngles = new Vector3(0F, 90F, 0F),
                            localScale = new Vector3(0.0802F, 0.0802F, 0.0802F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/FireBallDash"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEgg"),
                            childName = "Chest",
                            localPos = new Vector3(0.07441F, -0.03098F, 0.16384F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.1891F, 0.1891F, 0.1891F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/Cleanse"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWaterPack"),
                            childName = "Chest",
                            localPos = new Vector3(-0.0014F, -0.09706F, -0.13153F),
                            localAngles = new Vector3(0F, 180F, 0F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/Tonic"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTonic"),
                            childName = "Left Hand",
                            localPos = new Vector3(0.00981F, 0.09236F, -0.03525F),
                            localAngles = new Vector3(13.83449F, 356.5916F, 354.6006F),
                            localScale = new Vector3(0.1252F, 0.1252F, 0.1252F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/Gateway"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayVase"),
                            childName = "Chest",
                            localPos = new Vector3(-0.12238F, 0.11426F, -0.14207F),
                            localAngles = new Vector3(0F, 90F, 0F),
                            localScale = new Vector3(0.0982F, 0.0982F, 0.0982F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/Meteor"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMeteor"),
                            childName = "Chest",
                            localPos = new Vector3(0.65854F, 0.79489F, -0.05648F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/Saw"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySawmerang"),
                            childName = "Chest",
                            localPos = new Vector3(-0.67559F, 0.55616F, -0.00934F),
                            localAngles = new Vector3(90F, 0F, 0F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/Blackhole"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravCube"),
                            childName = "Chest",
                            localPos = new Vector3(-0.55903F, 0.03074F, -0.01245F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/Scanner"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScanner"),
                            childName = "Right Shoulder",
                            localPos = new Vector3(0.03333F, 0.00003F, -0.001F),
                            localAngles = new Vector3(355.6282F, 199.0192F, 72.95554F),
                            localScale = new Vector3(0.1884F, 0.1884F, 0.1884F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/Jetpack"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
         {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBugWings"),
                            childName = "Chest",
                            localPos = new Vector3(-0.00511F, 0.05807F, -0.09967F),
                            localAngles = new Vector3(21.4993F, 358.6616F, 358.3334F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
         }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/GoldGat"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldGat"),
                            childName = "Right Shoulder",
                            localPos = new Vector3(0.13416F, 0.23563F, -0.32966F),
                            localAngles = new Vector3(288.111F, 339.51F, 356.2275F),
                            localScale = new Vector3(0.15F, 0.15F, 0.15F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/BFG"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBFG"),
                            childName = "Chest",
                            localPos = new Vector3(0.04495F, 0.04455F, 0.00431F),
                            localAngles = new Vector3(0F, 0F, 313.6211F),
                            localScale = new Vector3(0.3F, 0.3F, 0.3F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/DeathProjectile"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
                            childName = "Chest",
                            localPos = new Vector3(-0.00395F, 0.02198F, 0.19145F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.0596F, 0.0596F, 0.0596F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/LifestealOnHit"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
                            childName = "Head",
                            localPos = new Vector3(-0.16103F, 0.3673F, -0.00358F),
                            localAngles = new Vector3(44.0939F, 33.5151F, 43.5058F),
                            localScale = new Vector3(0.1246F, 0.1246F, 0.1246F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2.LegacyResourcesAPI.Load<EquipmentDef>("EquipmentDefs/TeamWarCry"),
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
                            childName = "Chest",
                            localPos = new Vector3(-0.01345F, -0.34087F, 0.16481F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.05F, 0.05F, 0.05F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            //SOTV Equipment 

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.BossHunter,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTriconGhost"),
                            childName = "Head",
                            localPos = new Vector3(0F, 0.393F, -0.053F),
                            localAngles = new Vector3(11.45647F, 0.00001F, 0F),
                            localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        },                   
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBlunderbuss"),
                            childName = "Head",
                            localPos = new Vector3(0F, 0.393F, -0.053F),
                            localAngles = new Vector3(11.45647F, 0.00001F, 0F),
                            localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.BossHunterConsumed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTricornUsed"),
                            childName = "Head",
                            localPos = new Vector3(0.01951F, 0.24506F, -0.01688F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.EliteVoidEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAffixVoid"),
                            childName = "Head",
                            localPos = new Vector3(-0.00222F, 0.08705F, 0.13122F),
                            localAngles = new Vector3(73.98045F, 13.9495F, 18.87792F),
                            localScale = new Vector3(0.12F, 0.12F, 0.12F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.Molotov,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMolotov"),
                            childName = "Hips",
                            localPos = new Vector3(0.16606F, -0.25524F, 0.14767F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.MultiShopCard,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayExecutiveCard"),
                            childName = "Chest",
                            localPos = new Vector3(-0.02193F, -0.33623F, 0.16979F),
                            localAngles = new Vector3(87.43418F, 1.21722F, 6.00731F),
                            localScale = new Vector3(0.7F, 0.7F, 0.7F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.VendingMachine,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayVendingMachine"),
                            childName = "Hips",
                            localPos = new Vector3(0.14738F, -0.11606F, 0.0605F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            #endregion

            itemDisplayRuleSet.keyAssetRuleGroups = itemDisplayRules.ToArray();
            itemDisplayRuleSet.GenerateRuntimeValues();
        }

        private static CharacterModel.RendererInfo[] SkinRendererInfos(CharacterModel.RendererInfo[] defaultRenderers, Material[] materials)
        {
            CharacterModel.RendererInfo[] newRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(newRendererInfos, 0);

            newRendererInfos[0].defaultMaterial = materials[0];

          return newRendererInfos;
        }   
    }
}