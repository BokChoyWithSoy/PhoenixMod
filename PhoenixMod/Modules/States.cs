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
            entityStates.Add(typeof(BaseMeleeAttack));
            entityStates.Add(typeof(SlashCombo));

            entityStates.Add(typeof(Shoot));

            entityStates.Add(typeof(Roll));

            entityStates.Add(typeof(ThrowBomb));
        }
    }
}