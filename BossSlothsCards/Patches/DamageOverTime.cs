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
        public static bool TakeDamageOverTime(ref Vector2 damage, Vector2 position, float time, float interval, Color color, SoundEvent soundDamageOverTime,CharacterData ___data, GameObject damagingWeapon = null, Player damagingPlayer = null)
        {
            // Less damage when hazmat
            if (___data.GetComponent<Hazmat_Mono>())
            {
                damage *= 0.9f;
            }
            
            // Damage reduction
            damage /= ___data.GetComponent<CharacterStatModifiers>().GetAdditionalData().damageReduction;
            
            // Underdog
            if (damagingPlayer != null && damagingPlayer.GetComponent<Underdog_Mono>() && damagingPlayer.data.health < ___data.health)
            {
                damage *= 1.5f;
            }
            
            // Do damage to armor
            if (___data.GetComponent<ArmorHandler>() && ___data.GetAdditionalData().armor > 0 && !___data.GetComponent<ArmorHandler>().armorIsZero)
            {
                ___data.GetComponent<ArmorDamageOverTime>().DoDamageOverTimeVoid(damage, position, time, interval, color, soundDamageOverTime, damagingWeapon, damagingPlayer);
                return false;
            }

            return true;
        }
    }
}