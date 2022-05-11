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
        public static int paperAttackCount;
        private static bool decisiveEvidence;
        private static bool scepterActive;
        private static bool LockActive;
        public static float stopwatch;
        public static float lockAttackTimer;
        public static float paperAttackTimer;
        public static float paperEvidenceCount;

        public static ParticleSystem lock1Particle;
        public static ParticleSystem lock2Particle;
        public static ParticleSystem lock3Particle;
        public static ParticleSystem lock4Particle;        
        public static ParticleSystem blackLock1Particle;
        public static ParticleSystem blackLock2Particle;
        public static ParticleSystem blackLock3Particle;
        public static ParticleSystem blackLock4Particle;

        public void Awake()
        {
            characterBody = gameObject.GetComponent<CharacterBody>();
            childLocator = GetComponentInChildren<ChildLocator>();
            maxStacks = Modules.Config.necessaryStacksTurnabout.Value;
            paperAttackCount = 0;
            paperEvidenceCount = 0;
            decisiveEvidence = false;
            scepterActive = false;
            dying = false;
            LockActive = false;

            if (childLocator)
            {
                lock1Particle = childLocator.FindChild("Lock1").GetComponent<ParticleSystem>();
                lock2Particle = childLocator.FindChild("Lock2").GetComponent<ParticleSystem>();
                lock3Particle = childLocator.FindChild("Lock3").GetComponent<ParticleSystem>();
                lock4Particle = childLocator.FindChild("Lock4").GetComponent<ParticleSystem>();
                blackLock1Particle = childLocator.FindChild("BlackLock1").GetComponent<ParticleSystem>();
                blackLock2Particle = childLocator.FindChild("BlackLock2").GetComponent<ParticleSystem>();
                blackLock3Particle = childLocator.FindChild("BlackLock3").GetComponent<ParticleSystem>();
                blackLock4Particle = childLocator.FindChild("BlackLock4").GetComponent<ParticleSystem>();
                lock1Particle.Stop();
                lock2Particle.Stop();
                lock3Particle.Stop();
                lock4Particle.Stop();
                blackLock1Particle.Stop();
                blackLock2Particle.Stop();
                blackLock3Particle.Stop();
                blackLock4Particle.Stop();
            }
        }

        public void FixedUpdate()
        {
            if(LockActive)
            {
                if(characterBody.skillLocator.secondary.skillNameToken == PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_SECONDARY_LOCK_NAME")
                {
                    lock1Particle.Play();
                    lock2Particle.Play();
                    lock3Particle.Play();
                    lock4Particle.Play();
                }
                else if(characterBody.skillLocator.secondary.skillNameToken == PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_SECONDARY_LOCK_STRONG_NAME")
                {
                    lock1Particle.Stop();
                    lock2Particle.Stop();
                    lock3Particle.Stop();
                    lock4Particle.Stop();
                    blackLock1Particle.Play();
                    blackLock2Particle.Play();
                    blackLock3Particle.Play();
                    blackLock4Particle.Play();
                }
                stopwatch += Time.deltaTime;
                lockAttackTimer += Time.deltaTime;
            }
            else
            {
                lock1Particle.Stop();
                lock2Particle.Stop();
                lock3Particle.Stop();
                lock4Particle.Stop();
                blackLock1Particle.Stop();
                blackLock2Particle.Stop();
                blackLock3Particle.Stop();
                blackLock4Particle.Stop();
            }

            if(lockAttackTimer > 0.25f)
            {
                if(characterBody.hasEffectiveAuthority)
                {
                    if (characterBody.skillLocator.secondary.skillNameToken == PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_SECONDARY_LOCK_NAME")
                    {
                        PhoenixWright.SkillStates.LockSecondary.FireAttack();
                        blackLock1Particle.Stop();
                        blackLock2Particle.Stop();
                        blackLock3Particle.Stop();
                        blackLock4Particle.Stop();
                    }
                    if (characterBody.skillLocator.secondary.skillNameToken == PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_SECONDARY_LOCK_STRONG_NAME")
                    {
                        PhoenixWright.SkillStates.LockSecondaryStrong.FireAttack();
                        lock1Particle.Stop();
                        lock2Particle.Stop();
                        lock3Particle.Stop();
                        lock4Particle.Stop();
                    }
                }
                lockAttackTimer = 0;
            }

            if (paperEvidenceCount >= 10)
            {
                PhoenixPlugin.currentStacks++;
                if (Modules.Config.loweredVolume.Value)
                {
                    Util.PlaySound("GainStackQuiet", base.gameObject);
                }
                else Util.PlaySound("GainStack", base.gameObject);
                paperEvidenceCount = 0;
                if (characterBody.skillLocator.primary.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PRIMARY_THROW_NAME"))
                {
                    ShufflePrimary();
                }
                if (characterBody.skillLocator.primary.skillNameToken.Equals(PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_PRIMARY_PAPER_NAME"))
                {
                    characterBody.skillLocator.primary.UnsetSkillOverride(characterBody.skillLocator.primary, Phoenix.primaryPaperGreen, GenericSkill.SkillOverridePriority.Contextual);
                    PhoenixController.resetPaperAttackCount();
                }
            }


            if (stopwatch > 10)
            {
                setLock(false);
            }
        }


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

        public static int getPaperAttackCount()
        {
            return paperAttackCount;
        }

        public static void resetPaperAttackCount()
        {
            SetEvidenceType(false);
            paperAttackCount = 0;
        }

        public static void incrementPaperAttackCount()
        {
            paperAttackCount++;
        }

        public static void setLock(bool boolean)
        {
            LockActive = boolean;
        }

        private void ShufflePrimary()
        {
            int random = UnityEngine.Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    UnsetAll();
                    PhoenixController.SetEvidenceType(false);
                    break;
                case 1:
                    UnsetAll();
                    characterBody.skillLocator.primary.SetSkillOverride(characterBody.skillLocator.primary, Phoenix.primaryBottle, GenericSkill.SkillOverridePriority.Contextual);
                    PhoenixController.SetEvidenceType(false);
                    break;
                case 2:
                    UnsetAll();
                    characterBody.skillLocator.primary.SetSkillOverride(characterBody.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
                    PhoenixController.SetEvidenceType(false);
                    break;
            }
        }

        private void UnsetAll()
        {
            characterBody.skillLocator.primary.UnsetSkillOverride(characterBody.skillLocator.primary, Phoenix.primaryBottle, GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.primary.UnsetSkillOverride(characterBody.skillLocator.primary, Phoenix.primaryKnife, GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.primary.UnsetSkillOverride(characterBody.skillLocator.primary, Phoenix.primaryPhone, GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.primary.UnsetSkillOverride(characterBody.skillLocator.primary, Phoenix.primaryServbot, GenericSkill.SkillOverridePriority.Contextual);
        }
    }
}
