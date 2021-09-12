using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class FireHy_Mono : BossSlothMonoBehaviour
    {
        private Gun gun;
        private GunAmmo gunAmmo;

        void Start()
        {
            gunAmmo = GetComponent<Holding>().holdable.GetComponentInChildren<GunAmmo>();
            gun = GetComponent<Holding>().holdable.GetComponent<Gun>();
        }

        void Update()
        {
            gun.bursts = gunAmmo.maxAmmo+1;
            gun.attackSpeed = gun.timeBetweenBullets * gunAmmo.maxAmmo + 0.1f;
            gun.numberOfProjectiles = 1;
        }
    }
}