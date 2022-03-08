using PhoenixWright.Modules.Survivors;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using RoR2.Skills;


namespace PhoenixWright.Modules.Survivors
{
    public class PhoenixController : MonoBehaviour
    {
        public static int maxStacks;
        public static bool dying;
        private static bool decisiveEvidence;
        private static bool scepterActive;

        public void Awake()
        {
            maxStacks = Modules.Config.necessaryStacksTurnabout.Value;;
            decisiveEvidence = false;
            scepterActive = false;
            dying = false;
        }

        //Increment currentStacks on call


        //return current stacks.
        //return if evidence is decisive or not.
        public static bool GetEvidenceType()
        {
            return decisiveEvidence;
        }
        //set decisive evidence.
        public static void SetEvidenceType(bool input)
        {
            decisiveEvidence = input;
        }

        public void SetScepterActive(bool newState)
        {
            scepterActive = newState;
        }

        public bool GetScepterState()
        {
            return scepterActive;
        }
    }
}
