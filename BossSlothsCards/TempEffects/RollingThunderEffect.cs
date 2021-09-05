using System.Collections.Generic;
using System.Linq;
using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.Utils;
using ModdingUtils.Extensions;
using ModdingUtils.RoundsEffects;
using UnityEngine;
using CharacterStatModifiersExtension = BossSlothsCards.Extensions.CharacterStatModifiersExtension;

namespace BossSlothsCards.TempEffects
{
    public class RollingThunderEffect : HitSurfaceEffect
    {
        static readonly System.Random rng = new System.Random() { };
        
        private Player player;
        private Gun gun;
        
        public override void Hit(Vector2 position, Vector2 normal, Vector2 velocity)
        {
            player = gameObject.GetComponent<Player>();
            gun = player.GetComponent<Holding>().holdable.GetComponent<Gun>();

            if (gun.reflects >= 2147482647 && Random.Range(0, 10) != 4) return; 
            
            var newGun = player.gameObject.AddComponent<SplittingGun>();
            
            SpawnBulletsEffect effect = player.gameObject.AddComponent<SpawnBulletsEffect>();
            // set the position and direction to fire
            Vector2 parallel = ((Vector2)Vector3.Cross(Vector3.forward, normal)).normalized;
            List<Vector3> positions = GetPositions(position, normal, parallel);
            List<Vector3> directions = GetDirections(position, positions);
            effect.SetPositions(positions);
            effect.SetDirections(directions);
            effect.SetNumBullets(5);
            effect.SetTimeBetweenShots(0f);
            effect.SetInitialDelay(0f);

            // copy private gun stats over and reset a few public stats
            SpawnBulletsEffect.CopyGunStats(this.gun, newGun);
            
            newGun.numberOfProjectiles = 1;
            newGun.spread = 0;
            newGun.reflects = 0;
            newGun.projectileColor = Color.red;
            newGun.projectiles = (from e in Enumerable.Range(0, newGun.numberOfProjectiles) from x in newGun.projectiles select x).ToList().Take(newGun.numberOfProjectiles).ToArray();
            newGun.damage /= CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).damageReducedAistrike;
            newGun.projectileSpeed = 0.6f;
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
                res.Add(new Vector2(position.x + (-5+2*i),30));
            }

            return res;
        }

        private List<Vector3> GetDirections(Vector2 position, List<Vector3> shootPos)
        {
            List<Vector3> res = new List<Vector3>() { };

            foreach (Vector3 shootposition in shootPos)
            {
                res.Add(Vector3.down);
            }

            return res;
        }
    }
}