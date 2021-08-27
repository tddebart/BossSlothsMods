using BossSlothsCards.Extensions;
using HarmonyLib;
using UnityEngine;

namespace BossSlothsCards.Patches
{
    public class GunAmmoPatch
    {
        [HarmonyPatch(typeof(GunAmmo),"Shoot")]
        class Patch_Shoot
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(GunAmmo __instance)
            {
                if (__instance.transform.parent.GetComponentInParent<Holdable>())
                {
                    var holdable = __instance.transform.parent.GetComponentInParent<Holdable>();
                    var healthHandler = holdable.holder.GetComponent<HealthHandler>();
                    var player = holdable.holder.GetComponent<Player>();
                    var direction = Utils.Aim.GetAimDirectionAsVector(player);
                    var recoil = holdable.holder.GetComponent<CharacterStatModifiers>().GetAdditionalData().recoil;
                    
                    healthHandler.CallTakeForce(-new Vector2(1000 * direction.x, 1000 * direction.y) * (recoil*2f));
                }
            }
        }
    }
}