﻿using PhoenixWright.SkillStates;
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
            entityStates.Add(typeof(ThrowVase));
            entityStates.Add(typeof(ThrowKnife));
            entityStates.Add(typeof(ThrowServbot));
            entityStates.Add(typeof(ThrowPhone));
            entityStates.Add(typeof(ThrowBottle));
            entityStates.Add(typeof(SpawnArm));
            entityStates.Add(typeof(ThrowPaper));

            entityStates.Add(typeof(Press));
            entityStates.Add(typeof(PressTurnabout));
            entityStates.Add(typeof(LockSecondary));

            entityStates.Add(typeof(Fall));
            entityStates.Add(typeof(Tumble));

            entityStates.Add(typeof(Gavel));
            entityStates.Add(typeof(GavelStrong));

            entityStates.Add(typeof(Death));

        }
    }
}