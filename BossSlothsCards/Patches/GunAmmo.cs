using BossSlothsCards.Extensions;
using BossSlothsCards.TempEffects;
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
            // DoRecoil
            private static void Postfix(GunAmmo __instance)
            {
                if (__instance.transform.parent.GetComponentInParent<Holdable>())
                {
                    var holdable = __instance.transform.parent.GetComponentInParent<Holdable>();
                    var healthHandler = holdable.holder.GetComponent<HealthHandler>();
                    var player = holdable.holder.GetComponent<Player>();
                    var direction = Utils.Aim.GetAimDirectionAsVector(player);
                    var recoil = holdable.holder.GetComponent<CharacterStatModifiers>().GetAdditionalData().recoil;
                    
                    healthHandler.CallTakeForce(-new Vector2(1000 * direction.x, 1000 * direction.y) * (recoil*3f));
                }

                if (__instance.transform.parent.GetComponentInParent<Holdable>() && __instance.transform.parent.GetComponentInParent<Holdable>().holder.GetComponent<AlphaEffect>())
                {
                    __instance.transform.parent.GetComponentInParent<Holdable>().holder.GetComponent<AlphaEffect>()
                        .AlphaActive = false;
                }
            }
        }
    }
}