using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System;
using PhoenixWright.Modules.Survivors;

namespace PhoenixWright.SkillStates
{
    public class Tumble : BaseSkillState
    {
        

        public static float damageCoefficient = 2f;
        public static float procCoefficient = 1f;
        public static float duration = 0.5f;
        public static float initialSpeedCoefficient = 7f;
        public static float finalSpeedCoefficient = 2.5f;

        private bool hasFired;
        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;
        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;
        public Vector3 rayPosition;

        protected string hitboxName = "fall";
        protected BlastAttack blastAttack;
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

            blastAttack = new BlastAttack();
            blastAttack.radius = 10f;
            blastAttack.procCoefficient = procCoefficient;
            blastAttack.position = aimRay.origin;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = this.damageStat * damageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.Default;

            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuff(Modules.Buffs.armorBuff, 5f * Fall.duration);
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 1f * Fall.duration);
            }
        }

        private void RecalculateRollSpeed()
        {
            this.rollSpeed = this.moveSpeedStat * Mathf.Lerp(Fall.initialSpeedCoefficient, Fall.finalSpeedCoefficient, base.fixedAge / Fall.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.RecalculateRollSpeed();

            if (base.characterDirection) base.characterDirection.forward = this.forwardDirection;
            if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = Mathf.Lerp(Fall.dodgeFOV, 60f, base.fixedAge / Fall.duration);

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
                FireAttack();
                FireAttack();
                if (PhoenixController.GetEvidenceType())
                {
                    base.skillLocator.utility.Reset();
                    ShufflePrimary();
                }
            }

            if (base.isAuthority && base.fixedAge >= Fall.duration)
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
            this.blastAttack.Fire();
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