using ModdingUtils.MonoBehaviours;

namespace BossSlothsCards.TempEffects
{
    public class FireHydrantEffect : CounterReversibleEffect
    {

        public override CounterStatus UpdateCounter()
        {
            return CounterStatus.Apply;
        }

        public override void UpdateEffects()
        {
            gunAmmoStatModifier.maxAmmo_add = gun.numberOfProjectiles-1;
        }

        public override void OnApply()
        {
            
        }

        public override void Reset()
        {
            
        }
    }
}