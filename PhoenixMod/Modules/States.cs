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
            //Register Primary Skills
            entityStates.Add(typeof(Primary));
            entityStates.Add(typeof(SpawnArm));

            //Register Secondary Skills
            entityStates.Add(typeof(Press));
            entityStates.Add(typeof(PressTurnabout));

            //Register Utility Skills
            entityStates.Add(typeof(Fall));

            //Register Special Skills
            entityStates.Add(typeof(Gavel));
            entityStates.Add(typeof(GavelStrong));

            //Register Death State
            entityStates.Add(typeof(Death));

        }
    }
}