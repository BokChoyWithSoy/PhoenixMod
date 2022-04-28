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

        public ParticleSystem lock1Particle;
        public ParticleSystem lock2Particle;
        public ParticleSystem lock3Particle;
        public ParticleSystem lock4Particle;

        public void Awake()
        {
            characterBody = gameObject.GetComponent<CharacterBody>();
            childLocator = GetComponentInChildren<ChildLocator>();
            maxStacks = Modules.Config.necessaryStacksTurnabout.Value;
            paperAttackCount = 0;
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
                lock1Particle.Stop();
                lock2Particle.Stop();
                lock3Particle.Stop();
                lock4Particle.Stop();
            }
        }

        public void FixedUpdate()
        {
            if(LockActive)
            {
                lock1Particle.Play();
                lock2Particle.Play();
                lock3Particle.Play();
                lock4Particle.Play();
                stopwatch += Time.deltaTime;
                lockAttackTimer += Time.deltaTime;
            }
            else
            {
                lock1Particle.Stop();
                lock2Particle.Stop();
                lock3Particle.Stop();
                lock4Particle.Stop();
            }

            if(lockAttackTimer > 0.25f)
            {
                if(characterBody.hasEffectiveAuthority)
                {
                    PhoenixWright.SkillStates.LockSecondary.FireAttack();
                }
                lockAttackTimer = 0;
            }            
            
            if(paperAttackTimer > 0.25f)
            {
                if(characterBody.hasEffectiveAuthority)
                {
                    PhoenixWright.SkillStates.LockSecondary.FireAttack();
                }
                paperAttackTimer = 0;
            }

            if(stopwatch > 10)
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
    }
}
