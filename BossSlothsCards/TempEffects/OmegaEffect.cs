using HarmonyLib;
using ModdingUtils.MonoBehaviours;

namespace BossSlothsCards.TempEffects
{
    public class OmegaEffect : CounterReversibleEffect
    {
        public bool OmegaActive;

        public override CounterStatus UpdateCounter()
        {
            var currentAmmo = (int)Traverse.Create(gunAmmo).Field("currentAmmo").GetValue();
            return currentAmmo == 1 ? CounterStatus.Apply : CounterStatus.Remove;
        }

        public override void UpdateEffects()
        {
            characterStatModifiersModifier.lifeSteal_mult = 1.75f;
            gunStatModifier.damage_mult = 1.75f;
            gunStatModifier.projectileSpeed_mult = 1.25f;
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