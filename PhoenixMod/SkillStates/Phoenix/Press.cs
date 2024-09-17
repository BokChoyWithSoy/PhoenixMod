﻿using EntityStates;
using RoR2;
using UnityEngine;
using PhoenixWright.Modules.Survivors;
using PhoenixWright.Modules.Networking;
using R2API.Networking.Interfaces;

namespace PhoenixWright.SkillStates
{
    public class Press : BaseSkillState
    {
        public static float damageCoefficient = 4f;
        public static float procCoefficient = 1f;
        public static float duration = 0.025f;
        public Vector3 rayPosition;
        public static bool hasDamaged;


        private bool hasFired;

        protected BlastAttack blastAttack;
        protected float attackStartTime = 0.01f * duration;
        protected float attackEndTime = 1f *duration;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            Ray ray = new Ray(aimRay.origin + 1f * aimRay.direction, aimRay.direction);
            this.hasFired = false;

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 20f))
            {
                rayPosition = hit.point;
            }
            else
            {
                rayPosition = aimRay.origin + 20 * aimRay.direction;
            }

            base.StartAimMode(duration, true);
            base.PlayAnimation("FullBody, Override", "Point", "ShootGun.playbackRate", (Press.duration  / Press.duration));

            if (base.isAuthority)
            {
                EffectManager.SpawnEffect(Modules.Assets.pressEffect, new EffectData
                {
                    origin = rayPosition,
                    scale = 1f,
                    rotation = Quaternion.LookRotation(aimRay.direction)

                }, true);
            }

            blastAttack = new BlastAttack();
            blastAttack.radius = 10f;
            blastAttack.procCoefficient = procCoefficient;
            blastAttack.position = rayPosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = this.damageStat * damageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.Default;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= attackStartTime && base.fixedAge < attackEndTime && !hasFired)
            {
                    hasFired = true;
                    if (Modules.Config.loweredVolume.Value)
                    {
                    new PlaySoundNetworkRequest(base.characterBody.netId, 3304771474).Send(R2API.Networking.NetworkDestination.Clients);
                }
                    else new PlaySoundNetworkRequest(base.characterBody.netId, 3304771474).Send(R2API.Networking.NetworkDestination.Clients);
                if (base.isAuthority)
                    {
                        if (blastAttack.Fire().hitCount > 0)
                        {
                            OnHitEnemyAuthority();
                        }
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

        protected virtual void OnHitEnemyAuthority()
        {
            if (PhoenixController.GetEvidenceType())
            {
                base.characterBody.AddBuff(Modules.Buffs.turnaboutBuff);
                if (Modules.Config.loweredVolume.Value)
                {
                    Util.PlaySound("GainStackQuiet", base.gameObject);
                }
                else Util.PlaySound("GainStack", base.gameObject);
                if (!(Modules.Config.gainTurnaboutOnHit.Value))
                {
                    PhoenixPlugin.currentStacks += blastAttack.Fire().hitCount;
                }
                else PhoenixPlugin.currentStacks++;
                if (skillLocator.primary.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME") && PhoenixController.GetEvidenceType())
                {
                    ShufflePrimary();
                }
                if (skillLocator.primary.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PRIMARY_PAPER_NAME") && PhoenixController.GetEvidenceType())
                {
                    base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryPaperGreen, GenericSkill.SkillOverridePriority.Contextual);
                    PhoenixController.resetPaperAttackCount();
                }
                if (PhoenixPlugin.currentStacks >= PhoenixController.maxStacks)
                { 
                    if (PhoenixPlugin.turnaboutActive)
                    {
                        if (Modules.Config.loweredVolume.Value)
                        {
                            new PlaySoundNetworkRequest(base.characterBody.netId, 3561233033).Send(R2API.Networking.NetworkDestination.Clients);
                            new PlaySoundNetworkRequest(base.characterBody.netId, 3345964952).Send(R2API.Networking.NetworkDestination.Clients);
                        }
                        else
                        {
                            new PlaySoundNetworkRequest(base.characterBody.netId, 3561233033).Send(R2API.Networking.NetworkDestination.Clients);
                            new PlaySoundNetworkRequest(base.characterBody.netId, 3345964952).Send(R2API.Networking.NetworkDestination.Clients);
                        }
                        PhoenixPlugin.turnaboutActive = false;
                    }
                }
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
            return InterruptPriority.Pain;
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