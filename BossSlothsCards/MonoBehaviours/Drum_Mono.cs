using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class Drum_Mono : MonoBehaviour
    {
        private GunAmmo gunAmmo;
        private CharacterStatModifiers stats;

        private float increasedGravity;
        private float actualGravity;

        private bool firstTime = true;
        
        public void Start()
        {
            gunAmmo = GetComponent<Holding>().holdable.GetComponentInChildren<GunAmmo>();
            stats = GetComponent<CharacterStatModifiers>();
        }

        public void Update()
        {
            increasedGravity = gunAmmo.maxAmmo/50f;
            if (firstTime)
            {
                actualGravity = stats.gravity;
                firstTime = false;
            }

            stats.gravity = actualGravity + increasedGravity;
            actualGravity = stats.gravity - increasedGravity;
        }
    }
}