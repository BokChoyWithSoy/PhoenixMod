using EntityStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using PhoenixWright.Modules.Survivors;

namespace PhoenixWright.SkillStates
{
    public class ThrowServbot : BaseSkillState
    {
        public static float damageCoefficient = 2f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        public static float throwForce = 70f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private bool justSwitched = true;
        private int nextItem;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ThrowServbot.baseDuration / this.attackSpeedStat;
            if (justSwitched)
            {
                this.fireTime = 0.5f * this.duration;
            }
            else this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            base.PlayAnimation("Gesture, Override", "ThrowBomb", "ThrowBomb.playbackRate", this.duration);
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
                Util.PlaySound("HenryBombThrow", base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.servbotPrefab, 
                        aimRay.origin, 
                        Util.QuaternionSafeLookRotation(aimRay.direction), 
                        base.gameObject,
                        ThrowServbot.damageCoefficient * this.damageStat, 
                        4000f, 
                        base.RollCrit(), 
                        DamageColorIndex.Default, 
                        null,
                        ThrowServbot.throwForce);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime && !hasFired)
            {
                this.Fire();
                nextItem = Random.Range(0, 2);
                switch (nextItem)
                {
                    case 0:
                        base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
                        PhoenixController.SetEvidenceType(false);
                        break;
                    case 1:
                        base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
                        base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Phoenix.primaryPhone, GenericSkill.SkillOverridePriority.Contextual);
                        PhoenixController.SetEvidenceType(true);
                        break;
                    case 2:
                        base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
                        base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Phoenix.primaryKnife, GenericSkill.SkillOverridePriority.Contextual);
                        PhoenixController.SetEvidenceType(true);
                        break;
                    case 3:
                        base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
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

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}