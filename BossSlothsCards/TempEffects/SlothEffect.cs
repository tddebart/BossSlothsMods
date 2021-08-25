using ModdingUtils.MonoBehaviours;
using UnityEngine;

namespace BossSlothsCards.TempEffects
{
    public class SlothEffect : CounterReversibleEffect
    {
        public float multiplier;
        private const float timeToMax = 25;
        public float maxMultiplier = 3;
        
        public override CounterStatus UpdateCounter()
        {
            float timeSince = gun.sinceAttack;
            multiplier = Mathf.Clamp(((maxMultiplier - 1f) / (timeToMax)) * timeSince + 1f, 1f, maxMultiplier);
            
            return CounterStatus.Apply;
        }

        public override void UpdateEffects()
        {
            gunStatModifier.damage_mult = multiplier;
        }

        public override void OnApply()
        {
            
        }

        public override void Reset()
        {
            
        }
        
    }
}