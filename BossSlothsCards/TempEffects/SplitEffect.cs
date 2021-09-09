using System;
using System.Collections.Generic;
using System.Linq;
using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.Utils;
using ModdingUtils.Extensions;
using ModdingUtils.RoundsEffects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BossSlothsCards.TempEffects
{
    // Some code "borrowed from PCE
    public class SplitEffect : HitSurfaceEffect
    {
        static readonly System.Random rng = new System.Random() { };
        
        private Player player;
        private Gun gun;
        private GunAmmo gunAmmo;
        
        public void Awake()
        {
            player = gameObject.GetComponent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();
            gunAmmo = player.GetComponent<Holding>().holdable.GetComponentInChildren<GunAmmo>();
        }
        
        public override void Hit(Vector2 position, Vector2 normal, Vector2 velocity)
        {
            if (gun.reflects >= 2147482647 && Random.Range(0, 10) != 4) return;
            
            if (gun.numberOfProjectiles > 5 || gunAmmo.maxAmmo > 12)
            {
                var rnd = Random.Range(0, Math.Max(gun.numberOfProjectiles, gunAmmo.maxAmmo));
                if (rnd > 6)
                {
                    return;
                }
            }

            var newGun = player.gameObject.AddComponent<SplittingGun>();
            
            SpawnBulletsEffect effect = player.gameObject.AddComponent<SpawnBulletsEffect>();
            // set the position and direction to fire
            Vector2 parallel = ((Vector2)Vector3.Cross(Vector3.forward, normal)).normalized;
            List<Vector3> positions = GetPositions(position, normal, parallel);
            List<Vector3> directions = GetDirections(position, positions);
            effect.SetPositions(positions);
            effect.SetDirections(directions);
            effect.SetNumBullets(1);
            effect.SetTimeBetweenShots(0f);
            effect.SetInitialDelay(0f);

            // copy private gun stats over and reset a few public stats
            SpawnBulletsEffect.CopyGunStats(this.gun, newGun);
            
            newGun.reflects = 0;
            newGun.spread = 0.5f;
            newGun.numberOfProjectiles = 1;
            newGun.projectileColor = Color.yellow;
            newGun.projectiles = (from e in Enumerable.Range(0, newGun.numberOfProjectiles) from x in newGun.projectiles select x).ToList().Take(newGun.numberOfProjectiles).ToArray();
            newGun.damage = Mathf.Clamp(newGun.damage/2f, 0.5f, float.MaxValue);
            newGun.projectileSpeed = velocity.magnitude/65;
            newGun.damageAfterDistanceMultiplier = 1f;
            newGun.GetAdditionalData().inactiveDelay = 0.1f;
            newGun.objectsToSpawn = new [] { PreventRecursion.stopRecursionObjectToSpawn };

            // set the gun of the spawnbulletseffect
            effect.SetGun(newGun);
        }
        
        private List<Vector3> GetPositions(Vector2 position, Vector2 normal, Vector2 parallel)
        {
            List<Vector3> res = new List<Vector3>() { };

            for (int i = 0; i < 5; i++)
            {
                res.Add(position + 0.2f * normal + 0.1f * (float)rng.NextGaussianDouble() * parallel);
            }

            return res;
        }

        private List<Vector3> GetDirections(Vector2 position, List<Vector3> shootPos)
        {
            List<Vector3> res = new List<Vector3>() { };

            foreach (Vector3 shootposition in shootPos)
            {
                res.Add(((Vector2)shootposition - position).normalized);
            }

            return res;
        }
    }

    public class SplittingGun : Gun
    {

    }
}