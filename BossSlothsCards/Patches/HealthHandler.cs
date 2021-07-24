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
        public static bool CallTakeDamage(CharacterData ___data, Vector2 damage, Vector2 position, GameObject damagingWeapon = null, Player damagingPlayer = null)
        {
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