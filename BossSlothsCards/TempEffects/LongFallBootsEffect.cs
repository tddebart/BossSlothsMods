using BossSlothsCards.Extensions;
using ModdingUtils.MonoBehaviours;

namespace BossSlothsCards.TempEffects
{
    public class LongFallBootsEffect : CounterReversibleEffect
    {

        public override CounterStatus UpdateCounter()
        {
            return CounterStatus.Apply;
        }

        public override void UpdateEffects()
        {
            characterStatModifiersModifier.jump_mult = characterStatModifiers.GetAdditionalData().extraJumpHeight;
        }

        public override void OnApply()
        {
            
        }

        public override void Reset()
        {
        }
    }
}