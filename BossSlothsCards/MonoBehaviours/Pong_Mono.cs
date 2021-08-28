using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class Pong_Mono : BossSlothMonoBehaviour
    {
        private Gun gun;
        private GunAmmo gunAmmo;
        public int maxAmmo;

        private void Start()
        {
            gun = GetComponent<Holding>().holdable.GetComponent<Gun>();
            gunAmmo = GetComponent<Holding>().holdable.GetComponentInChildren<GunAmmo>();
        }

        private void Update()
        {
            gunAmmo.maxAmmo = 1;
            gun.numberOfProjectiles = maxAmmo;
        }
    }
}