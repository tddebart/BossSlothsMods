using ModdingUtils.MonoBehaviours;

namespace BossSlothsCards.TempEffects
{
    public class AlphaEffect : CounterReversibleEffect
    {
        public bool AlphaActive;
        
        public override CounterStatus UpdateCounter()
        {
            return AlphaActive ? CounterStatus.Apply : CounterStatus.Remove;
        }

        public override void UpdateEffects()
        {
            characterStatModifiersModifier.lifeSteal_mult = 1.75f;
            gunStatModifier.damage_mult = 1.75f;
            gunStatModifier.projectileSpeed_mult = 1.25f;
        }

        public override void OnApply()
        {
            
        }

        public override void Reset()
        {
            
        }
    }
}