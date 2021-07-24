using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using HarmonyLib;
using Sonigon;
using UnityEngine;

namespace BossSlothsCards.Patches
{
    [HarmonyPatch(typeof(DamageOverTime))]
    public class DamageOverTimePatch
    {
        [HarmonyPatch("TakeDamageOverTime")]
        [HarmonyPrefix]
        public static bool TakeDamageOverTime(Vector2 damage, Vector2 position, float time, float interval, Color color, SoundEvent soundDamageOverTime,CharacterData ___data, GameObject damagingWeapon = null, Player damagingPlayer = null)
        {
            if (___data.GetComponent<ArmorHandler>() && ___data.GetAdditionalData().armor > 0 && !___data.GetComponent<ArmorHandler>().armorIsZero)
            {
                ___data.GetComponent<ArmorDamageOverTime>().DoDamageOverTimeVoid(damage, position, time, interval, color, soundDamageOverTime, damagingWeapon, damagingPlayer);
                return false;
            }

            return true;
        }
    }
}