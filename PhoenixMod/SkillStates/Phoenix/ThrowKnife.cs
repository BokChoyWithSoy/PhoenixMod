using EntityStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using PhoenixWright.Modules.Survivors;
using PhoenixWright.Modules.Networking;
using R2API.Networking.Interfaces;

namespace PhoenixWright.SkillStates
{
    public class ThrowKnife : BaseSkillState
    {
        public static float damageCoefficient = 4f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.75f;
        public static float throwForce = 80f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private bool justSwitched = true;
        private int nextItem;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ThrowKnife.baseDuration / this.attackSpeedStat;
            if (justSwitched)
            {
                this.fireTime = 0.5f * this.duration;
            }
            else this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                if (Modules.Config.loweredVolume.Value)
                {
                    new PlaySoundNetworkRequest(base.characterBody.netId, 2883528150).Send(R2API.Networking.NetworkDestination.Clients);
                }
                else new PlaySoundNetworkRequest(base.characterBody.netId, 2883528150).Send(R2API.Networking.NetworkDestination.Clients);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    base.PlayAnimation("RightArm, Override", "Throw", "ThrowBomb.playbackRate", this.duration);
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.knifePrefab, 
                        aimRay.origin, 
                        Util.QuaternionSafeLookRotation(aimRay.direction), 
                        base.gameObject, 
                        ThrowKnife.damageCoefficient * this.damageStat, 
                        0f, 
                        base.RollCrit(), 
                        DamageColorIndex.Default, 
                        null, 
                        ThrowKnife.throwForce);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime && !hasFired)
            {
                this.Fire();
                nextItem = Random.Range(0, 4);
                switch (nextItem)
                {
                    case 0:
                        base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryKnife, GenericSkill.SkillOverridePriority.Contextual);
                        PhoenixController.SetEvidenceType(false);
                        break;
                    case 1:
                        base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryKnife, GenericSkill.SkillOverridePriority.Contextual);
                        base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Phoenix.primaryPhone, GenericSkill.SkillOverridePriority.Contextual);
                        PhoenixController.SetEvidenceType(true);
                        PlayEvidenceSound();
                        break;
                    case 2:
                        base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryKnife, GenericSkill.SkillOverridePriority.Contextual);
                        base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
                        PhoenixController.SetEvidenceType(false);
                        break;
                    case 3:
                        base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryKnife, GenericSkill.SkillOverridePriority.Contextual);
                        base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Phoenix.primaryBottle, GenericSkill.SkillOverridePriority.Contextual);
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

        public void PlayEvidenceSound()
        {
            if (Modules.Config.loweredVolume.Value)
            {
                Util.PlaySound("EvidenceSoundQuiet", base.gameObject);
            }
            else Util.PlaySound("EvidenceSound", base.gameObject);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}