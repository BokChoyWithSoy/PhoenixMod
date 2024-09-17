using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using PhoenixWright.Modules.Networking;
using R2API.Networking.Interfaces;



namespace PhoenixWright.SkillStates
{
    public class SpawnArm : BaseSkillState
    {
        public static float damageCoefficient = 4f;
        public static float procCoefficient = 2f;
        public static float baseDuration = 1f;
        public static float throwForce = 130f;

        private float duration;
        private float fireTime;
        private bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = SpawnArm.baseDuration / this.attackSpeedStat;
            fireTime = 0.4f * this.duration;
            base.characterBody.SetAimTimer(2f);

            base.PlayAnimation("FullBody, Override", "Point", "ThrowBomb.playbackRate", duration);
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
                        0f, 
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
                if (Modules.Config.loweredVolume.Value)
                {
                    new PlaySoundNetworkRequest(base.characterBody.netId, 4107067948).Send(R2API.Networking.NetworkDestination.Clients);
                }
                else new PlaySoundNetworkRequest(base.characterBody.netId, 4107067948).Send(R2API.Networking.NetworkDestination.Clients);
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