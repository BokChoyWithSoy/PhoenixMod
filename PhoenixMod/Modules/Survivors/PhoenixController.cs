using PhoenixWright.Modules.Survivors;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using RoR2.Skills;



namespace PhoenixWright.Modules.Survivors
{
    public class PhoenixController : MonoBehaviour
    {
        public CharacterBody characterBody;
        public ChildLocator childLocator;
        public static int maxStacks;
        public static bool dying;
        private static bool decisiveEvidence;
        public static float stopwatch;

        public void Awake()
        {
            characterBody = gameObject.GetComponent<CharacterBody>();
            childLocator = GetComponentInChildren<ChildLocator>();
            maxStacks = Modules.Config.necessaryStacksTurnabout.Value;
            decisiveEvidence = false;
            dying = false;
        }

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

        private void ShufflePrimary()
        {
            int random = UnityEngine.Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    PhoenixController.SetEvidenceType(false);
                    break;
                case 1:
                    characterBody.skillLocator.primary.SetSkillOverride(characterBody.skillLocator.primary, Phoenix.primaryBottle, GenericSkill.SkillOverridePriority.Contextual);
                    PhoenixController.SetEvidenceType(false);
                    break;
                case 2:

                    characterBody.skillLocator.primary.SetSkillOverride(characterBody.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
                    PhoenixController.SetEvidenceType(false);
                    break;
            }
        }
    }
}
