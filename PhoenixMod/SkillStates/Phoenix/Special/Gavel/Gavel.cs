﻿using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using PhoenixWright.Modules.Survivors;

namespace PhoenixWright.SkillStates
{
    public class Gavel : BaseSkillState
    {
        public static float damageCoefficient = 7f;
        public static float procCoefficient = 1f;
        public static float duration = 0.7f;
        public Vector3 rayPosition;


        private bool hasFired;
        private float stopwatch;
        private Animator animator;

        protected BlastAttack blastAttack;
        protected BlastAttack blastAttackStrong;
        protected float attackStartTime = 0.01f * 1f;
        protected float attackEndTime = 1f *1f;

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
                Util.PlaySound("SpecialSoundQuiet", base.gameObject);
            }
            else Util.PlaySound("SpecialSound", base.gameObject);

            blastAttack = new BlastAttack();
            blastAttack.radius = 25f;
            blastAttack.procCoefficient = procCoefficient;
            blastAttack.position = rayPosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = this.damageStat * damageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.Default;

            blastAttackStrong = new BlastAttack();
            blastAttackStrong.radius = 50f;
            blastAttackStrong.procCoefficient = procCoefficient;
            blastAttackStrong.position = rayPosition;
            blastAttackStrong.attacker = base.gameObject;
            blastAttackStrong.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttackStrong.baseDamage = this.damageStat * (damageCoefficient * 2);
            blastAttackStrong.falloffModel = BlastAttack.FalloffModel.None;
            blastAttackStrong.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttackStrong.damageType = DamageType.Stun1s | DamageType.WeakOnHit;
            blastAttackStrong.attackerFiltering = AttackerFiltering.Default;

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Ray aimRay = base.GetAimRay();
            rayPosition = aimRay.origin;

            stopwatch += Time.fixedDeltaTime;
            if(stopwatch >= duration * 0.4f)
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

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                if (PhoenixController.GetEvidenceType())
                {
                    if (base.isAuthority)
                    {
                        blastAttackStrong.Fire();
                        if (skillLocator.primary.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME") && PhoenixController.GetEvidenceType())
                        {
                            PhoenixController.SetEvidenceType(false);
                            ShufflePrimary();
                        }
                        if (skillLocator.primary.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PRIMARY_PAPER_NAME") && PhoenixController.GetEvidenceType())
                        {
                            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryPaperGreen, GenericSkill.SkillOverridePriority.Contextual);
                            PhoenixController.resetPaperAttackCount();
                        }
                    }
                }
                else if (base.isAuthority)
                {
                    blastAttack.Fire();
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
                RoR2.ShakeEmitter.CreateSimpleShakeEmitter(rayPosition, wave, 0.5f , 20f , true);

            }
        }

        private void ShufflePrimary()
        {
            int random = UnityEngine.Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    UnsetAll();
                    PhoenixController.SetEvidenceType(false);
                    break;
                case 1:
                    UnsetAll();
                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Phoenix.primaryBottle, GenericSkill.SkillOverridePriority.Contextual);
                    PhoenixController.SetEvidenceType(false);
                    break;
                case 2:
                    UnsetAll();
                    base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
                    PhoenixController.SetEvidenceType(false);
                    break;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        private void UnsetAll()
        {
            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryBottle, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryKnife, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryPhone, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
        }
    }
}