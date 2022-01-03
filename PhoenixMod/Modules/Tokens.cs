using R2API;
using System;

namespace PhoenixWright.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region PheonixWright
            string prefix = PhoenixPlugin.developerPrefix + "_PHOENIX_BODY_";

            string desc = "Phoenix is a survivor who starts off weak in investigation mode and becomes very powerful once he collects enough evidence and enters turnabout mode.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Red evidence is junk and green evidence is decisive evidence. Ocne enough is collected enter turnabout mode." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > If you hit an enemy with press with decisive evidence, you will gain a stack of turnabout." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Fall deals damage on top of providing brief invulnerability." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Order in the court! can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

            LanguageAPI.Add(prefix + "NAME", "Phoenix Wright");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Ace Attorney");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Turnabout");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Whenever you hit an enemy with Press The Witness while holding a decisive piece of evidence, gain a stack of turnabout, if you have 50 stacks of turnabout, Phoenix will enter turnabout mode, replacing Phoenix's primary and secondary abilities with empowered versions.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_THROW_NAME", "Throw Evidence");
            LanguageAPI.Add(prefix + "PRIMARY_THROW_DESCRIPTION", Helpers.agilePrefix + $"Throw a random piece of evidence, If the evidence is junk it will deal <style=cIsDamage>200% damage</style> and if it is decisive evidence it will deal <style=cIsDamage>400% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_THROW2_NAME", "Take That!");
            LanguageAPI.Add(prefix + "PRIMARY_THROW2_DESCRIPTION", Helpers.agilePrefix + $"State your case, summoning a giant hand which travels in a line, dealing <style=cIsDamage>600% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_PRESS_NAME", "Press The Witness");
            LanguageAPI.Add(prefix + "SECONDARY_PRESS_DESCRIPTION", $"Press the witness dealing <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style> and stunning all enemies hit.");

            LanguageAPI.Add(prefix + "SECONDARY_PRESS2_NAME", "Objection!");
            LanguageAPI.Add(prefix + "SECONDARY_PRESS2_DESCRIPTION", $"Raise an objection dealing <style=cIsDamage>{100f * (StaticValues.gunDamageCoefficient + 2)}% damage</style> and stunning all enemies hit.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_FALL_NAME", "Fall");
            LanguageAPI.Add(prefix + "UTILITY_FALL_DESCRIPTION", "Fall forward on your face gaining <style=cIsUtility>300 armor</style> and dealing  <style=cIsDamage>300% damage</style> and stunning all enemies hit. <style=cIsUtility>You cannot be hit during the fall.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_ORDER_NAME", "Order In The Court!");
            LanguageAPI.Add(prefix + "SPECIAL_ORDER_DESCRIPTION", $"After a short delay, a giant gavel falls from the sky dealing <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style> and stunning all enemies hit.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Phoenix: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Phoenix, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Phoenix: Mastery");
            #endregion
            #endregion
        }
    }
}