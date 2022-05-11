using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using PhoenixWright.Modules.Survivors;

namespace PhoenixWright.SkillStates
{
    public class LockSecondary : BaseSkillState
    {
        public static float damageCoefficient = 0.5f;
        public static float procCoefficient = 1f;
        public static float duration = 0.25f;
        public Vector3 rayPosition;

        public static BlastAttack blastAttack;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();

            rayPosition = aimRay.origin;

            PhoenixController.stopwatch = 0;


            blastAttack = new BlastAttack();
            blastAttack.radius = 4f;
            blastAttack.procCoefficient = procCoefficient;
            blastAttack.position = rayPosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = this.damageStat * damageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.Stun1s;
            blastAttack.attackerFiltering = AttackerFiltering.Default;

            PhoenixController.setLock(true);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            rayPosition = base.GetAimRay().origin;
            blastAttack.position = rayPosition;

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

        public static void FireAttack()
        {
            blastAttack.position = PhoenixPlugin.characterPos;

            if(blastAttack.Fire().hitCount > 0)
            {
                PhoenixWright.Modules.Survivors.PhoenixController.paperEvidenceCount++;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}