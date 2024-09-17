using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using PhoenixWright.Modules.Survivors;
using PhoenixWright.Modules.Networking;
using R2API.Networking.Interfaces;

namespace PhoenixWright.SkillStates
{
    public class GavelStrong : BaseSkillState
    {
        public static float damageCoefficient = 7f;
        public static float procCoefficient = 2f;
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

            if (base.isAuthority)
            {
                EffectManager.SpawnEffect(Modules.Assets.gavelEffect, new EffectData
                {
                    origin = new Vector3(base.transform.position.x, base.transform.position.y + 10, base.transform.position.z),
                    scale = 1f,

                }, true);
            }

            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 1f);
            }

            if (Modules.Config.loweredVolume.Value)
            {
                new PlaySoundNetworkRequest(base.characterBody.netId, 1688684367).Send(R2API.Networking.NetworkDestination.Clients);
            }
            else new PlaySoundNetworkRequest(base.characterBody.netId, 1688684367).Send(R2API.Networking.NetworkDestination.Clients);

            blastAttackStrong = new BlastAttack();
            blastAttackStrong.radius = 40f;
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
                if(base.isAuthority)
                {
                    FireAttack();
                }
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

                if (base.isAuthority)
                {
                    blastAttackStrong.Fire();
                }

                if (base.isAuthority)
                {
                    EffectManager.SpawnEffect(Modules.Assets.dustEffect, new EffectData
                    {
                        origin = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z),
                        scale = 1f,

                    }, true);
                }

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