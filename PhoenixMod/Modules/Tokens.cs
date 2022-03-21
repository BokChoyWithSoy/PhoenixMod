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

            string outro = "..and so he left, trying to find a way home.";
            string outroFailure = "..and so he vanished, defended the innocent but not himself.";

            LanguageAPI.Add(prefix + "NAME", "Attorney");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Ace Attorney");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Wright");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Edgeworth");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Turnabout");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION","Use " + Helpers.decisivePrefix + "to passively gain " + Helpers.turnaboutPrefix +" If you have <style=cIsDamage>" + Modules.Config.necessaryStacksTurnabout.Value + " stacks</style>, enter <style=cIsUtility>turnabout mode</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_THROW_NAME", "Throw Evidence");
            LanguageAPI.Add(prefix + "PRIMARY_THROW_DESCRIPTION", Helpers.stunningPrefix + Helpers.agilePrefix + $"Throw a random piece of evidence, Junk will deal <style=cIsDamage>200% damage</style> and decisive evidence will deal <style=cIsDamage>400% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_THROW2_NAME", "Take That!");
            LanguageAPI.Add(prefix + "PRIMARY_THROW2_DESCRIPTION", Helpers.stunningPrefix + Helpers.agilePrefix + $"State your case, summoning a giant hand which travels in a line, dealing <style=cIsDamage>600% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_PRESS_NAME", "Press The Witness");
            LanguageAPI.Add(prefix + "SECONDARY_PRESS_DESCRIPTION", Helpers.stunningPrefix + $"Press the witness dealing <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_PRESS2_NAME", "Objection!");
            LanguageAPI.Add(prefix + "SECONDARY_PRESS2_DESCRIPTION", Helpers.freezingPrefix + $"Raise an objection dealing <style=cIsDamage>{100f * (StaticValues.gunDamageCoefficient + 2)}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_FALL_NAME", "Fall");
            LanguageAPI.Add(prefix + "UTILITY_FALL_DESCRIPTION", Helpers.stunningPrefix +  $"<style=cIsUtility>Fall</style> on your face gaining <style=cIsUtility>300 armor</style> and dealing <style=cIsDamage>300% damage</style>. <style=cIsUtility>You cannot be hit during the fall.</style>");

            LanguageAPI.Add(prefix + "UTILITY_FALL2_NAME", "Tumble");
            LanguageAPI.Add(prefix + "UTILITY_FALL2_DESCRIPTION", Helpers.stunningPrefix + $"<style=cIsUtility>Fall over and tumble</style>, dealing <style=cIsDamage>600% damage</style>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_ORDER_NAME", "Order In The Court!");
            LanguageAPI.Add(prefix + "SPECIAL_ORDER_DESCRIPTION", Helpers.stunningPrefix + $"After a short delay, a giant gavel falls from the sky dealing <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>. <style=cIsUtility>You cannot be hit for 2 seconds.</style>");
            LanguageAPI.Add(prefix + "SPECIAL_ORDER_NAME2", "Objection Overruled!");
            LanguageAPI.Add(prefix + "SPECIAL_ORDER_DESCRIPTION2", Helpers.stunningPrefix + $"After a short delay, a giant gavel falls from the sky dealing <style=cIsDamage>{200f * StaticValues.bombDamageCoefficient}% damage and weakening enemies</style>. <style=cIsUtility>You cannot be hit for 2 seconds.</style>");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Phoenix: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Phoenix, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Phoenix: Mastery");

            LanguageAPI.Add(prefix + "SWORDUNLOCKABLE_ACHIEVEMENT_NAME", "Phoenix: A Dear Friend");
            LanguageAPI.Add(prefix + "SWORDUNLOCKABLE_ACHIEVEMENT_DESC", "As Phoenix, collect a ceremonial dagger.");
            LanguageAPI.Add(prefix + "SWORDUNLOCKABLE_UNLOCKABLE_NAME", "Phoenix: A Dear Friend");
            #endregion
            #endregion

            #region Keywords
            LanguageAPI.Add("KEYWORD_TURNABOUT", "[ Decisive Evidence ]\nRed evidence is junk and green evidence is decisive evidence. Using other skills while having decisive evidence in your primary slot will grant bonus effects.\n\n[ Turnabout ]\nDealing damage with Attorney's secondary ability while having " + Helpers.decisivePrefix + "awards <style=cIsUtility>turnabout</style>.\n\n[ Turnabout Mode ]\nEntering <style=cIsUtility>turnabout mode</style> will <style=cIsUtility>permanently empower</style> all of Attorney's skills ");
                LanguageAPI.Add("KEYWORD SECONDARY", "[ Decisive Evidence ]\nGain Turnabout stacks equal to the amount of enemies hit.");
                LanguageAPI.Add("KEYWORD UTILITY", "[ Decisive Evidence ]\nReset Cooldown on use.");
                LanguageAPI.Add("KEYWORD SPECIAL", "[ Decisive Evidence ]\nDouble damage and range and weaken all enemies in range.");

            #endregion
        }

    }
}
