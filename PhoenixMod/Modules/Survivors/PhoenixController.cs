using PhoenixWright.Modules.Survivors;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace PhoenixWright.Modules.Survivors
{
    public class PhoenixController : MonoBehaviour
    {
        public static int currentStacks;
        public static int maxStacks;
        private static bool scepterActive;

        public int GetCurrentStacks()
        {
            return currentStacks;
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