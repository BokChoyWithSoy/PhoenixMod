using EntityStates;
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
        public static float damageCoefficient = 10f;
        public static float procCoefficient = 1f;
        public static float duration = 1f;
        public Vector3 rayPosition;


        private bool hasFired;
        private float stopwatch;
        private Animator animator;

        protected BlastAttack blastAttack;
        protected float attackStartTime = 0.01f * duration;
        protected float attackEndTime = 1f *duration;

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

            }, true) ;

            Util.PlaySound("SpecialSound", base.gameObject);

            blastAttack = new BlastAttack();
            blastAttack.radius = 15f;
            blastAttack.procCoefficient = 0.2f;
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
            Ray aimRay = base.GetAimRay();
            rayPosition = aimRay.origin;

            stopwatch += Time.fixedDeltaTime;
            if(stopwatch >= duration * 0.4)
            {
                FireAttack();
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
                blastAttack.Fire();
                base.PlayAnimation("FullBody, Override", "Getup", "ShootGun.playbackRate", (Press.duration / Press.duration));  
                Wave wave = new Wave
                {
                    amplitude = 2f,
                    frequency = 180f,
                    cycleOffset = 0f
                };
                RoR2.ShakeEmitter.CreateSimpleShakeEmitter(rayPosition, wave, 0.5f , 20f , true);

            }
        }
    }
}