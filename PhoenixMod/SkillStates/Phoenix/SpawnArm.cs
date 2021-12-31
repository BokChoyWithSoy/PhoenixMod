using EntityStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using PhoenixWright.Modules.Survivors;

namespace PhoenixWright.SkillStates
{
    public class SpawnArm : BaseSkillState
    {
        public static float damageCoefficient = 10f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
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
            this.duration = SpawnArm.baseDuration / this.attackSpeedStat;
            if (justSwitched)
            {
                this.fireTime = 0.5f * this.duration;
            }
            else this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            base.PlayAnimation("FullBody, Override", "Point", "ThrowBomb.playbackRate", this.duration);
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

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.armPrefab, 
                        aimRay.origin, 
                        Util.QuaternionSafeLookRotation(aimRay.direction), 
                        base.gameObject, 
                        SpawnArm.damageCoefficient * this.damageStat, 
                        4000f, 
                        base.RollCrit(), 
                        DamageColorIndex.Default, 
                        null, 
                        SpawnArm.throwForce);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime && !hasFired)
            {
                Fire();
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