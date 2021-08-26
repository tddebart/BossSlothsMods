using System;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace BossSlothsCards.Extensions
{
    public partial class CharacterStatModifiersAdditionalData
    {
        public float damageReduction;
        public float recoil;

        public CharacterStatModifiersAdditionalData()
        {
            damageReduction = 1;
            recoil = 0;
        }
    }
    public static class CharacterStatModifiersExtension
    {
        public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData> data = new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData>();

        public static CharacterStatModifiersAdditionalData GetAdditionalData(this CharacterStatModifiers characterstats)
        {
            return data.GetOrCreateValue(characterstats);
        }

        public static void AddData(this CharacterStatModifiers characterstats, CharacterStatModifiersAdditionalData value)
        {
            try
            {
                data.Add(characterstats, value);
            }
            catch (Exception) { }
        }
        
        // reset additional CharacterStatModifiers when ResetStats is called
        [HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
        class CharacterStatModifiersPatchResetStats
        {
            private static void Prefix(CharacterStatModifiers __instance)
            {
                __instance.GetAdditionalData().damageReduction = 1;
                __instance.GetAdditionalData().recoil = 0;
            }
        }

    }
}
