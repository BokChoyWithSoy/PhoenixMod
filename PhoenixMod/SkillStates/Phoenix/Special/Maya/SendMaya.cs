//using EntityStates;
//using RoR2;
//using RoR2.Projectile;
//using UnityEngine;
//using RoR2.Skills;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.Networking;
//using PhoenixWright.Modules.Survivors;
//using PhoenixWright.Modules.Networking;
//using R2API.Networking.Interfaces;
//using R2API.Networking;

//namespace PhoenixWright.SkillStates
//{
//    public class SendMaya : BaseSkillState
//    {
//        public static float damageCoefficient = 16f;
//        public static float procCoefficient = 1f;
//        public static float baseDuration = 0.1f;
//        public static float throwForce = 15f;
//        public static GameObject projectilePrefab;
//        public static Vector3 endPoint;

//        private MayaController mayaCon;
//        private float duration;
//        private float fireTimer;
//        private Vector3 startingDirection;
//        private bool hasFired;
//        private Animator animator;

//        public override void OnEnter()
//        {
//            base.OnEnter();

//            this.duration = SendMaya.baseDuration / this.attackSpeedStat;
//            base.characterBody.SetAimTimer(2f);
//            this.animator = base.GetModelAnimator();

//            base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.Cloak.buffIndex, 5f);
//            //base.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.CloakSpeed.buffIndex, 5f);

//            hasFired = false;

//            startingDirection = GetAimRay().direction;

//            mayaCon = this.gameObject.GetComponent<MayaController>();

//            //Call network request here to request a spawn.

//            //Invoke wisp spawn request to spawn an object.
//            if (base.isAuthority)
//            {
//                if (mayaCon.hasFiredClone)
//                {
//                    //Nothing to pass since the user is the key.
//                    new ClientWispDestroyNetworkRequest(characterBody.masterObjectId).Send(NetworkDestination.Clients);

//                    //Set everything back to false
//                    mayaCon.hasFiredClone = false;
//                    mayaCon.wispStopwatch = 0f;
//                }
//                new ClientWispSpawnNetworkRequest(characterBody.masterObjectId, startingDirection, (double)moveSpeedStat).Send(NetworkDestination.Clients);
//            }
//        }

//        public override void OnExit()
//        {
//            base.OnExit();
//            wispCon.SetNewTransitionTarget(WispController.GunState.RUN);
//        }

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();

//            //Change the skill ONCE and start the stopwatch on the controller.
//            //THE SKILL UNSET IS ON THE WISP CONTROLLER, DON'T LOOK HERE TO UNSET, UNSET IN THE WISP CONTROLLER.
//            if (!hasFired && base.isAuthority)
//            {
//                wispCon.aimDir = startingDirection;
//                wispCon.originalLoc = base.transform.position;
//                wispCon.hasFiredClone = true;
//                wispCon.wispStopwatch = 0f;
//                base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Modules.Survivors.Wisp.willOWispReactivate, GenericSkill.SkillOverridePriority.Contextual);
//            }

//            if (base.fixedAge >= this.duration && base.isAuthority)
//            {
//                //State ends naturally with nothing to perform.
//                this.outer.SetNextStateToMain();
//                return;
//            }
//        }

//        public override InterruptPriority GetMinimumInterruptPriority()
//        {
//            return InterruptPriority.PrioritySkill;
//        }
//    }
//}