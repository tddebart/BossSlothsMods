using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.TempEffects;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

namespace BossSlothsCards.Patches
{
    public class GunAmmoPatch
    {
        [HarmonyPatch(typeof(GunAmmo),"Shoot")]
        class Patch_Shoot
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(GunAmmo __instance, GameObject projectile)
            {
                // Destroy pong
                if(__instance.GetAdditionalData().destroyAllPongOnNextShot)
                {
                    foreach (var obj in __instance.GetAdditionalData().projectiles)
                    {
                        GameObject.Destroy(obj);
                    }

                    __instance.GetAdditionalData().destroyAllPongOnNextShot = false;
                }
                
                // DoRecoil
                if (__instance.transform.parent.GetComponentInParent<Holdable>() && __instance.transform.parent.GetComponentInParent<Holdable>().holder.GetComponent<PhotonView>().IsMine && __instance.transform.parent.GetComponentInParent<Holdable>().holder.GetComponent<CharacterStatModifiers>().GetAdditionalData().recoil != 0)
                {
                    var holdable = __instance.transform.parent.GetComponentInParent<Holdable>();
                    var healthHandler = holdable.holder.GetComponent<HealthHandler>();
                    var player = holdable.holder.GetComponent<Player>();
                    var direction = Utils.Aim.GetAimDirectionAsVector(player);
                    var recoil = holdable.holder.GetComponent<CharacterStatModifiers>().GetAdditionalData().recoil;
                    
                    healthHandler.CallTakeForce(-new Vector2(1000 * direction.x, 1000 * direction.y) * (recoil*3f));
                }

                // Alpha effect
                if (__instance.transform.parent.GetComponentInParent<Holdable>() && __instance.transform.parent.GetComponentInParent<Holdable>().holder.GetComponent<AlphaEffect>())
                {
                    __instance.transform.parent.GetComponentInParent<Holdable>().holder.GetComponent<AlphaEffect>()
                        .AlphaActive = false;
                }

                // Keep track of projectiles if have pong
                if (__instance.transform.parent.GetComponentInParent<Holdable>() && __instance.transform.parent
                    .GetComponentInParent<Holdable>().holder.GetComponent<Pong_Mono>())
                {
                    __instance.GetAdditionalData().projectiles.Add(projectile);
                    GameObject.Destroy(projectile.GetComponent<RemoveAfterSeconds>());
                }
            }
        }
    }
}