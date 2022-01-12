using PhoenixWright.SkillStates;
using PhoenixWright.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace PhoenixWright.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void RegisterStates()
        {
            //Register Primary Attacks
            entityStates.Add(typeof(BaseMeleeAttack));
            entityStates.Add(typeof(ThrowVase));
            entityStates.Add(typeof(ThrowKnife));
            entityStates.Add(typeof(ThrowServbot));
            entityStates.Add(typeof(ThrowPhone));
            entityStates.Add(typeof(ThrowBottle));

            //Register Utility Attacks
            entityStates.Add(typeof(Press));
            entityStates.Add(typeof(PressTurnabout));

            //Register Utility Attacks
            entityStates.Add(typeof(Fall));

            //Register Special Attacks
            entityStates.Add(typeof(Gavel));
        }
    }
}