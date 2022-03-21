using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using PhoenixWright.Modules.Survivors;

namespace PhoenixWright.SkillStates
{
    public class GavelStrong : BaseSkillState
    {
        public static float damageCoefficient = 10f;
        public static float procCoefficient = 5f;
        public static float duration = 0.7f;
        public Vector3 rayPosition;


        private bool hasFired;
        private float stopwatch;
        private Animator animator;

        protected BlastAttack blastAttackStrong;
        protected float attackStartTime = 0.01f * 1f;
        protected float attackEndTime = 1f * 1f;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.hasFired = false;

            rayPosition = aimRay.origin;

            EffectManager.SpawnEffect(Modules.Assets.gavelEffect, new EffectData
            {
                origin = new Vector3(base.transform.position.x, base.transform.position.y + 10, base.transform.position.z),
                scale = 1f,

            }, true);

            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 2f);
            }

            if (Modules.Config.loweredVolume.Value)
            {
                Util.PlaySound("SpecialSoundQuiet", base.gameObject);
            }
            else Util.PlaySound("SpecialSound", base.gameObject);

            blastAttackStrong = new BlastAttack();
            blastAttackStrong.radius = 50f;
            blastAttackStrong.procCoefficient = procCoefficient;
            blastAttackStrong.position = rayPosition;
            blastAttackStrong.attacker = base.gameObject;
            blastAttackStrong.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttackStrong.baseDamage = this.damageStat * (damageCoefficient * 2);
            blastAttackStrong.falloffModel = BlastAttack.FalloffModel.None;
            blastAttackStrong.teamIndex = TeamComponent.GetObjectTeam(blastAttackStrong.attacker);
            blastAttackStrong.damageType = DamageType.Stun1s | DamageType.WeakOnHit;
            blastAttackStrong.attackerFiltering = AttackerFiltering.Default;

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Ray aimRay = base.GetAimRay();
            rayPosition = aimRay.origin;

            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= duration * 0.4f)
            {
                FireAttack();
            }


            if (base.fixedAge >= duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            base.characterMotor.disableAirControlUntilCollision = false;
        }

        private void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                blastAttackStrong.Fire();

                base.PlayAnimation("FullBody, Override", "Getup", "ShootGun.playbackRate", duration);
                Wave wave = new Wave
                {
                    amplitude = 2f,
                    frequency = 180f,
                    cycleOffset = 0f
                };
                RoR2.ShakeEmitter.CreateSimpleShakeEmitter(rayPosition, wave, 0.5f, 20f, true);

            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}