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
                if (__instance.transform.parent.GetComponentInParent<Holdable>() && __instance.transform.parent.GetComponentInParent<Holdable>().holder.GetComponent<CharacterStatModifiers>().GetAdditionalData().recoil > 0)
                {
                    var holdable = __instance.transform.parent.GetComponentInParent<Holdable>();
                    var pv = holdable.holder.GetComponent<PlayerVelocity>();
                    var player = holdable.holder.GetComponent<Player>();
                    var direction = Utils.Aim.GetAimDirectionAsVector(player);
                    var recoil = holdable.holder.GetComponent<CharacterStatModifiers>().GetAdditionalData().recoil;
                
                    pv.AddForce(-new Vector2(100000 * direction.x, 100000 * direction.y)*(recoil*2.25f));
                }
            }
        }
    }
}