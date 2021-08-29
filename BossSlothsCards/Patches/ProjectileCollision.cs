using System.Linq;
using BossSlothsCards.MonoBehaviours;
using HarmonyLib;
using UnityEngine;

namespace BossSlothsCards.Patches
{
    [HarmonyPatch(typeof(ProjectileCollision))]
    public class ProjectileCollisionPatch
    {
        [HarmonyPatch("HitSurface")]
        [HarmonyPrefix]
        public static bool hitSurface(ProjectileCollision __instance, ref ProjectileHitSurface.HasToStop __result, GameObject projectile, HitInfo hit)
        {
            if (__instance.GetComponentInParent<ProjectileHit>().ownPlayer.data.currentCards.Any(cr => cr.cardName == "BulletProof bullets"))
            {
                UnityEngine.Debug.LogWarning("hit surface");
                var position1 = projectile.transform.position;
                position1 -= position1.normalized;
                projectile.transform.position = position1;
                projectile.GetComponent<MoveTransform>().velocity *= -1f;
                if (projectile.GetComponentInParent<ProjectileHit>().ownPlayer.data.currentCards
                    .Any(cr => cr.cardName == "BulletProof bullets"))
                {
                    var bullet = __instance.transform.parent;
                    var transform = bullet.transform;
                    var position = transform.position;
                    position -= position.normalized;
                    transform.position = position;
                    bullet.GetComponent<MoveTransform>().velocity *= -1f;
                }
                return false;
            }

            return true;
        }
    }
}