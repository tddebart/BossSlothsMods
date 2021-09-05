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
            private static void Postfix(GunAmmo __instance, GameObject projectile, Gun ___gun)
            {
                // Prevent effects when shot from temp effect
                if (projectile.GetComponent<ProjectileHit>().ownWeapon.name == "WeaponBase(Clone)(Clone)") return;
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
                if (___gun.holdable && ___gun.holdable.holder.view.IsMine && ___gun.holdable.holder.stats.GetAdditionalData().recoil != 0)
                {
                    var holdable = ___gun.holdable;
                    var healthHandler = holdable.holder.healthHandler;
                    var player = holdable.holder.player;
                    var direction = Utils.Aim.GetAimDirectionAsVector(player);
                    var recoil = holdable.holder.stats.GetAdditionalData().recoil;
                    var damage = ___gun.damage;
                    
                    healthHandler.CallTakeForce(-new Vector2(1000 * direction.x, 1000 * direction.y) * (recoil*2.5f*damage));
                }

                // Alpha effect
                if (___gun.holdable && ___gun.holdable.holder.GetAdditionalData().alphaEffect)
                {
                    ___gun.holdable.holder.GetAdditionalData().alphaEffect
                        .AlphaActive = false;
                }

                // Keep track of projectiles if have pong
                if (___gun.holdable && ___gun.holdable.holder.GetComponent<Pong_Mono>())
                {
                    __instance.GetAdditionalData().projectiles.Add(projectile);
                    GameObject.Destroy(projectile.GetComponent<RemoveAfterSeconds>());
                }
            }
        }
    }
}