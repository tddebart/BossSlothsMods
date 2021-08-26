using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using HarmonyLib;
using UnityEngine;

namespace BossSlothsCards.Patches
{
    public class ExplosionPatch
    {
        [HarmonyPatch(typeof(Explosion),"DoExplosionEffects")]
        class Patch_DoAttack
        {
            // ReSharper disable once UnusedMember.Local
            private static void Prefix(Explosion __instance, Collider2D hitCol)
            {
                if (hitCol.gameObject.GetComponent<CharacterStatModifiers>() && 
                    __instance.damage > 0 && hitCol.gameObject.GetComponent<CharacterStatModifiers>().GetAdditionalData().explosiveResistant)
                {
                    __instance.damage = 0;
                }
                
            }
        }
    }
}