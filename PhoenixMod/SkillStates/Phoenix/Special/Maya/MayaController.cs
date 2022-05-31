//using R2API.Networking;
//using R2API.Networking.Interfaces;
//using RoR2;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;
//using UnityEngine.Networking;
//using WispMod.Modules.Networking;

//namespace PhoenixWright.SkillStates
//{
//    class MayaController : MonoBehaviour
//    {
//        private float stopwatch;
//        public Vector3 moveDir;
//        private CharacterMaster master;
//        private RigidbodyMotor motor;
//        public NetworkInstanceId objID;
//        private GameObject wispObj;
//        public float speedScaling;

//        public void Start()
//        {
//            stopwatch = 0f;
//            master = gameObject.GetComponent<CharacterMaster>();
//            wispObj = GameObject.Instantiate(Modules.Assets.willOWispPrefab);
//        }

//        public void FixedUpdate()
//        {
//            stopwatch += Time.fixedDeltaTime;
//            if (!motor)
//            {
//                if (master.GetBodyObject())
//                {
//                    motor = master.GetBodyObject().GetComponent<RigidbodyMotor>();
//                }
//            }
//            if (motor)
//            {
//                if (master)
//                {
//                    wispObj.transform.position = master.GetBodyObject().transform.position;
//                }

//                wispObj.transform.rotation = Quaternion.LookRotation(moveDir);
//                float speed = (speedScaling / 15f);
//                if (speed < 1.0f)
//                {
//                    speed = 1.0f;
//                }
//                motor.rootMotion += moveDir.normalized * Modules.StaticValues.willowispSpeed * Time.fixedDeltaTime * speed;
//            }

//            if (stopwatch >= Modules.StaticValues.willowispDuration && NetworkServer.active)
//            {
//                CleanUp();
//            }
//        }

//        public void CleanUp()
//        {
//            Destroy(wispObj);
//            new ClientWispDestroyNetworkRequest(objID).Send(NetworkDestination.Clients);
//        }

//        void OnDestroy()
//        {
//            Destroy(wispObj);
//        }
//    }
//}