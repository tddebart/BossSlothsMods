using ModdingUtils.MonoBehaviours;

namespace BossSlothsCards.TempEffects
{
    public class DrumEffect : CounterReversibleEffect
    {
        public override CounterStatus UpdateCounter()
        {
            return CounterStatus.Apply;
        }

        public override void UpdateEffects()
        {
            gravityModifier.gravityForce_mult = gunAmmo.maxAmmo/25f + 1f;
        }

        public override void OnApply()
        {
            
        }

        public override void Reset()
        {
            
        }
    }
}