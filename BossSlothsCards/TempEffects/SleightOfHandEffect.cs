using HarmonyLib;
using ModdingUtils.MonoBehaviours;

namespace BossSlothsCards.TempEffects
{
    public class SleightOfHandEffect : CounterReversibleEffect
    {

        public override CounterStatus UpdateCounter()
        {
            var currentAmmo = (int)Traverse.Create(gunAmmo).Field("currentAmmo").GetValue();
            return currentAmmo == 1 ? CounterStatus.Apply : CounterStatus.Remove;
        }

        public override void UpdateEffects()
        {
            gunStatModifier.numberOfProjectiles_mult = 2;
            gunStatModifier.spread_add = 0.12f;
            gunStatModifier.damage_mult = 1.2f;
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