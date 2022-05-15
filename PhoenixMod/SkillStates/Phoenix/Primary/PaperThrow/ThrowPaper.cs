using EntityStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;
using PhoenixWright.Modules.Survivors;

namespace PhoenixWright.SkillStates
{
    public class ThrowPaper
        : BaseSkillState
    {
        public static float damageCoefficient = 1f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        public static float throwForce = 80f;
        

        private float duration;
        private BlastAttack blastAttack;
        private bool hasFired;
        private Vector3 rayPosition;
        private Animator animator;

        public override void OnEnter()
        {
            Ray aimRay = base.GetAimRay();
            rayPosition = aimRay.origin + 4 * aimRay.direction;
            base.OnEnter();
            this.duration = ThrowPaper.baseDuration / this.attackSpeedStat;

            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            blastAttack = new BlastAttack();
            blastAttack.radius = 6f;
            blastAttack.procCoefficient = procCoefficient;
            blastAttack.position = rayPosition;
            blastAttack.attacker = base.gameObject;
            blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
            blastAttack.baseDamage = this.damageStat * damageCoefficient;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
            blastAttack.damageType = DamageType.SlowOnHit;
            blastAttack.attackerFiltering = AttackerFiltering.Default;

            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuff(Modules.Buffs.armorBuff, duration);
            }

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

                    base.PlayAnimation("RightArm, Override", "PaperThrow", "ThrowBomb.playbackRate", this.duration / 4);

                    blastAttack.Fire();

                    EffectManager.SpawnEffect(Modules.Assets.paperEffect, new EffectData
                    {
                        origin = rayPosition,
                        scale = 1f,
                        rotation = Quaternion.LookRotation(aimRay.direction)

                    }, true);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (PhoenixController.getPaperAttackCount() >= 3)
            {
                PhoenixController.SetEvidenceType(true);
                base.skillLocator.primary.SetSkillOverride(base.skillLocator.primary, Phoenix.primaryPaperGreen, GenericSkill.SkillOverridePriority.Contextual);
            }

            if (!hasFired)
            {
                this.Fire();
                PhoenixController.incrementPaperAttackCount();
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