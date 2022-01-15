using EntityStates;
using RoR2;
using UnityEngine;

namespace PhoenixWright.SkillStates.BaseStates
{
    internal class Death : GenericCharacterDeath
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Util.PlaySound("PhoenixDying", base.gameObject);
            base.PlayAnimation("FullBody, Override", "Dying", "Roll.playbackRate", 1000f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}