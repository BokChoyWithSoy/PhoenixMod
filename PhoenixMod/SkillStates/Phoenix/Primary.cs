using EntityStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using PhoenixWright.Modules.Survivors;
using System;
using PhoenixWright.Modules;

namespace PhoenixWright.SkillStates
{
    public class Primary : BaseSkillState
    {
        public static float baseDuration = 0.75f;
        public static float throwForce = 80f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private int currentItem;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;
            this.fireTime = 0.4f * this.duration;
            base.characterBody.SetAimTimer(2f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime && !hasFired)
            {
                Fire();
                currentItem = UnityEngine.Random.Range(0, 5);
                switch (currentItem) 
                {
                    case 0:
                        Phoenix.primaryThrow.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPhoneIcon");
                        PhoenixController.SetEvidenceType(true);
                        PlayEvidenceSound();
                        break;
                    case 1:
                        Phoenix.primaryThrow.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texKnifeIcon");
                        PhoenixController.SetEvidenceType(true);
                        PlayEvidenceSound();
                        break;
                    case 2:;
                        Phoenix.primaryThrow.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texBottleIcon");
                        PhoenixController.SetEvidenceType(false);
                        break;
                    case 3:
                        Phoenix.primaryThrow.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texVaseIcon");
                        PhoenixController.SetEvidenceType(false);
                        break;
                    case 4:
                        Phoenix.primaryThrow.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texServbotIcon");
                        PhoenixController.SetEvidenceType(false);
                        break;
                }
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                if (Modules.Config.loweredVolume.Value)
                {
                    Util.PlaySound("ThrowVaseQuiet", base.gameObject);
                }
                else Util.PlaySound("ThrowVase", base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    base.PlayAnimation("RightArm, Override", "Throw", "ThrowBomb.playbackRate", this.duration);

                    switch (currentItem)
                    {
                        case 0:
                            ProjectileManager.instance.FireProjectile(Modules.Projectiles.vasePrefab,
                            aimRay.origin,
                            Util.QuaternionSafeLookRotation(aimRay.direction),
                            base.gameObject,
                            StaticValues.PrimaryRedDamageCoefficent * this.damageStat,
                            0f,
                            base.RollCrit(),
                            DamageColorIndex.Default,
                            null,
                            throwForce);
                            break;
                        case 1:
                            ProjectileManager.instance.FireProjectile(Modules.Projectiles.servbotPrefab,
                            aimRay.origin,
                            Util.QuaternionSafeLookRotation(aimRay.direction),
                            base.gameObject,
                            StaticValues.PrimaryRedDamageCoefficent * this.damageStat,
                            0f,
                            base.RollCrit(),
                            DamageColorIndex.Default,
                            null,
                            throwForce);
                            break;
                        case 2:
                            ProjectileManager.instance.FireProjectile(Modules.Projectiles.bottlePrefab,
                            aimRay.origin,
                            Util.QuaternionSafeLookRotation(aimRay.direction),
                            base.gameObject,
                            StaticValues.PrimaryRedDamageCoefficent * this.damageStat,
                            0f,
                            base.RollCrit(),
                            DamageColorIndex.Default,
                            null,
                            throwForce);
                            break;
                        case 3:
                            ProjectileManager.instance.FireProjectile(Modules.Projectiles.phonePrefab,
                            aimRay.origin,
                            Util.QuaternionSafeLookRotation(aimRay.direction),
                            base.gameObject,
                            StaticValues.PrimaryGreenDamageCoefficent * this.damageStat,
                            0f,
                            base.RollCrit(),
                            DamageColorIndex.Default,
                            null,
                            throwForce);
                            break;
                        case 4:
                            ProjectileManager.instance.FireProjectile(Modules.Projectiles.knifePrefab,
                            aimRay.origin,
                            Util.QuaternionSafeLookRotation(aimRay.direction),
                            base.gameObject,
                            StaticValues.PrimaryGreenDamageCoefficent * this.damageStat,
                            0f,
                            base.RollCrit(),
                            DamageColorIndex.Default,
                            null,
                            throwForce);
                            break;
                    }
                }
            }
        }


        public void PlayEvidenceSound()
        {
            if (Modules.Config.loweredVolume.Value)
            {
                Util.PlaySound("EvidenceSoundQuiet", base.gameObject);
            }
            else Util.PlaySound("EvidenceSound", base.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}