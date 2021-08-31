using HarmonyLib;
using ModdingUtils.MonoBehaviours;

namespace BossSlothsCards.TempEffects
{
    public class OverclockedFlywheelsEffect : CounterReversibleEffect
    {
        public bool OmegaActive;
        public float percentageAmmo;

        public override CounterStatus UpdateCounter()
        {
            var currentAmmo = (int)Traverse.Create(gunAmmo).Field("currentAmmo").GetValue();
            percentageAmmo = (float)currentAmmo / (float)gunAmmo.maxAmmo;
            return CounterStatus.Apply;
        }

        public override void UpdateEffects()
        {
            gunStatModifier.projectileSpeed_mult = 3 - percentageAmmo * 2;
        }

        public override void OnApply()
        {
            //gunAmmo = GetComponent<Holding>().holdable.GetComponentInChildren<GunAmmo>();
        }

        public override void Reset()
        {
            
        }
    }
}