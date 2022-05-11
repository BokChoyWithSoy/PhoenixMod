using RoR2;
using System;
using UnityEngine;

namespace PhoenixWright.Modules.Achievements
{
    [RegisterAchievement(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_DRIPUNLOCKABLE_ACHIEVEMENT",
    PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_DRIPUNLOCKABLE_REWARD_ID", null, null)]
    internal class DripAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_DRIPUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_DRIPUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_DRIPUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_UNLOCKABLE_REWARD_ID";
        public override string UnlockableNameToken { get; } = PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_DRIPUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_DRIPUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texDripSkin");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_DRIPUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_DRIPUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("Dripped Out"),
                                Language.GetString(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_DRIPUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Modules.Survivors.Phoenix.instance.fullBodyName);
        }

        private void CheckItem(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            orig(self);

                if (self && self.teamIndex == TeamIndex.Player && self.inventory)
                {
                    if (InventoryCheck(self.inventory))
                    {
                        if(base.meetsBodyRequirement)
                        {
                            base.Grant();
                        }
                    }
                }
        }

        private bool InventoryCheck(Inventory inventory)
        {

            bool hasFire = (inventory.GetItemCount(RoR2Content.Items.FireRing) > 0 ? true : false);
            bool hasIce = (inventory.GetItemCount(RoR2Content.Items.IceRing) > 0 ? true : false);
            bool hasHoof = (inventory.GetItemCount(RoR2Content.Items.Hoof) > 0 ? true : false);
            bool hasTooth = (inventory.GetItemCount(RoR2Content.Items.Tooth) > 0 ? true : false);

            return hasFire && hasIce && hasHoof && hasTooth;
        }

        public override void OnInstall()
        {
            base.OnInstall();

            On.RoR2.CharacterMaster.OnInventoryChanged += this.CheckItem;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            On.RoR2.CharacterMaster.OnInventoryChanged -= this.CheckItem;
        }
    }
}