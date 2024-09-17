using EntityStates;
using PhoenixWright.Modules.Networking;
using R2API.Networking.Interfaces;

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
            AkSoundEngine.StopAll();
            new PlaySoundNetworkRequest(base.characterBody.netId, 4242188613).Send(R2API.Networking.NetworkDestination.Clients);
            base.PlayAnimation("FullBody, Override", "Dying", "Roll.playbackRate", 1000f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}