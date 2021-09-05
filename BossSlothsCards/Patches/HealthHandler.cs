using System;
using System.Linq;
using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using HarmonyLib;
using UnityEngine;

namespace BossSlothsCards.Patches
{
    [HarmonyPatch(typeof(HealthHandler))]
    public class HealthHandlerPatch
    {
        [HarmonyPatch("CallTakeDamage")]
        [HarmonyPrefix]
        public static bool CallTakeDamage(CharacterData ___data, ref Vector2 damage, Vector2 position, GameObject damagingWeapon = null, Player damagingPlayer = null)
        {
            // Long fall boots reduced damage from wall
            if (damagingPlayer == null && damagingWeapon == null && position == (Vector2)___data.transform.position)
            {
                damage *= ___data.stats.GetAdditionalData().reducedDamageFromWall;
            }
            
            // Thorns damage
            foreach (var card in ___data.currentCards.Where(card => card.cardName.Contains("Thorns")).Where(card => damagingPlayer != null))
            {
                // ReSharper disable once PossibleNullReferenceException
                damagingPlayer.GetComponent<HealthHandler>().CallTakeDamage(damage * 0.2f, Vector2.zero);
            }
            
            // Damage reduction
            damage /= Mathf.Sqrt(___data.GetComponent<CharacterStatModifiers>().GetAdditionalData().damageReduction)*1.2f;
            
            // Underdog
            if (damagingPlayer != null && damagingPlayer.GetComponent<Underdog_Mono>() && damagingPlayer.data.health < ___data.health)
            {
                damage *= 1.5f;
            }
            
            // Deal damage to armor not health
            if (___data.GetComponent<ArmorHandler>() && ___data.GetAdditionalData().armor > 0 && !___data.GetComponent<ArmorHandler>().armorIsZero)
            {
                ___data.GetComponent<ArmorHandler>().CallTakeDamage(damage.magnitude, position, damagingWeapon, damagingPlayer);
                return false;
            }

            return true;
        }
        [HarmonyPatch("Revive")]
        [HarmonyPostfix]
        public static void Revive(CharacterData ___data)
        {
            if (___data.GetComponent<ArmorHandler>())
            {
                ___data.GetAdditionalData().armor = ___data.GetAdditionalData().maxArmor;
            }
        }
    }
}