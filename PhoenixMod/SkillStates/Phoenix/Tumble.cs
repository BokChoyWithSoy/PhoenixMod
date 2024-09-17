﻿using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System;
using PhoenixWright.Modules.Survivors;

namespace PhoenixWright.SkillStates
{
    public class Tumble : BaseSkillState
    {
        

        public static float damageCoefficient = 4f;
        public static float procCoefficient = 1f;
        public static float duration = 0.5f;
        public static float initialSpeedCoefficient = 10f;
        public static float finalSpeedCoefficient = 8.5f;

        private bool hasFired;
        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;
        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;
        public Vector3 rayPosition;

        protected string hitboxName = "fall";
        protected OverlapAttack attack;
        protected float attackStartTime = 0.001f * duration;
        protected float attackEndTime = 8f *duration;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.hasFired = false;
            this.animator = base.GetModelAnimator();

            if (base.isAuthority && base.inputBank && base.characterDirection)
            {
                this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            }

            Vector3 rhs = base.characterDirection ? base.characterDirection.forward : this.forwardDirection;
            Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);

            float num = Vector3.Dot(this.forwardDirection, rhs);
            float num2 = Vector3.Dot(this.forwardDirection, rhs2);

            this.RecalculateRollSpeed();

            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity.y = 0f;
                base.characterMotor.velocity = this.forwardDirection * this.rollSpeed;
            }

            Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
            this.previousPosition = base.transform.position - b;


            base.StartAimMode(duration, true);
            base.PlayAnimation("FullBody, Override", "Tumble", "Roll.playbackRate", duration/2);
            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();
            if(Modules.Config.loweredVolume.Value)
            {
                Util.PlaySound("FallQuiet", base.gameObject);
            }
            else Util.PlaySound("Fall", base.gameObject);


            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
            }

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.BypassArmor;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = damageCoefficient * this.damageStat;
            this.attack.procCoefficient = procCoefficient;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
        }

        private void RecalculateRollSpeed()
        {
            this.rollSpeed = (this.moveSpeedStat) * Mathf.Lerp(Fall.initialSpeedCoefficient, Fall.finalSpeedCoefficient, base.fixedAge / Tumble.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.RecalculateRollSpeed();

            if (base.characterDirection) base.characterDirection.forward = this.forwardDirection;
            if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = Mathf.Lerp(Fall.dodgeFOV, 60f, base.fixedAge / Tumble.duration);

            Vector3 normalized = (base.transform.position - this.previousPosition).normalized;
            if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
            {
                Vector3 vector = normalized * this.rollSpeed;
                float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
                vector = this.forwardDirection * d;
                vector.y = 0f;

                base.characterMotor.velocity = vector;
            }
            this.previousPosition = base.transform.position;

            if (base.fixedAge >= attackStartTime && base.fixedAge < attackEndTime)
            {
                FireAttack();
                if (PhoenixController.GetEvidenceType())
                {
                    base.skillLocator.utility.Reset();
                }
            }

            if (base.isAuthority && base.fixedAge >= Tumble.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }

        }

        public override void OnExit()
        {
            if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
            base.OnExit();

            base.characterMotor.disableAirControlUntilCollision = false;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.forwardDirection = reader.ReadVector3();
        }

        private void FireAttack()
        {
            if(!hasFired)
            {
                hasFired = true;
                if (Modules.Config.loweredVolume.Value)
                {
                    Util.PlaySound("FallVoiceQuiet", base.gameObject);
                }
                else Util.PlaySound("FallVoice", base.gameObject);
            }
            this.attack.Fire();
            if (skillLocator.primary.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME") && PhoenixController.GetEvidenceType())
            {
                base.skillLocator.utility.Reset();
                ShufflePrimary();
                PhoenixController.SetEvidenceType(false);
            }
            if (skillLocator.primary.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PRIMARY_PAPER_NAME") && PhoenixController.GetEvidenceType())
            {
                base.skillLocator.utility.Reset();
                base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryPaperGreen, GenericSkill.SkillOverridePriority.Contextual);
                PhoenixController.resetPaperAttackCount();
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

        private void UnsetAll()
        {
            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryBottle, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryKnife, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryPhone, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.primary.UnsetSkillOverride(base.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
        }
    }
}