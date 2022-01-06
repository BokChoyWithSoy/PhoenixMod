using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System;
using PhoenixWright.Modules.Survivors;

namespace PhoenixWright.SkillStates
{
    public class Fall : BaseSkillState
    {
        

        public static float damageCoefficient = 3f;
        public static float procCoefficient = 1f;
        public static float duration = 0.5f;
        public static float initialSpeedCoefficient = 5f;
        public static float finalSpeedCoefficient = 2.5f;

        private bool hasFired;
        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;
        private float stopwatch;
        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;

        protected string hitboxName = "fall";
        protected OverlapAttack attack;
        protected float attackStartTime = 0.58f * duration;
        protected float attackEndTime = 1f *duration;

        public override void OnEnter()
        {
            base.OnEnter();
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
            base.PlayAnimation("FullBody, Override", "FallFlat", "Roll.playbackRate", Fall.duration * 2);
            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();
            base.PlayAnimation("FullBody, Override", "GetUp", "Roll.playbackRate", Fall.duration * 2);
            Util.PlaySound("Fall", base.gameObject);
       

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
            }

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Stun1s;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = damageCoefficient * this.damageStat;
            this.attack.procCoefficient = procCoefficient;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();

            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuff(Modules.Buffs.armorBuff, 3f * Fall.duration);
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.5f * Fall.duration);
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

            stopwatch += Time.fixedDeltaTime;
            if (this.stopwatch >= attackStartTime && this.stopwatch <= attackEndTime * 4 )
            {
                FireAttack();
            }

            if (base.isAuthority && base.fixedAge >= Fall.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }

            if(PhoenixController.dying)
            {
                PhoenixController.dying = false;
                base.PlayAnimation("FullBody, Override", "Dying", "Roll.playbackRate", 3f);
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
            if (!this.hasFired)
            {
                this.hasFired = true;
                this.attack.Fire();
                Util.PlaySound("FallVoice", base.gameObject);
            }
        }
    }
}