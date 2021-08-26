using ModdingUtils.Extensions;
using ModdingUtils.MonoBehaviours;
using UnityEngine;
using CharacterStatModifiersExtension = BossSlothsCards.Extensions.CharacterStatModifiersExtension;

namespace BossSlothsCards.TempEffects
{
    public class EagleEffect : CounterReversibleEffect
    {
        public float sinceGrounded;
        public bool grounded;
        private float origReduction;
        private float multiplier;
        private float maxMultiplier = 5;
        private const float timeToMax = 17;
        
        public override CounterStatus UpdateCounter()
        {
            if (!PlayerStatus.PlayerSimulated(player)) sinceGrounded = 0;
            SinceGroundUpdate();
            grounded = player.data.isGrounded || player.data.isWallGrab;
            multiplier = Mathf.Clamp(((maxMultiplier - 1f) / (timeToMax)) * sinceGrounded + 1f, 1f, maxMultiplier);
            UnityEngine.Debug.LogWarning(CharacterStatModifiersExtension.GetAdditionalData(characterStatModifiers).damageReduction);
            return !grounded ? CounterStatus.Apply : CounterStatus.Remove;
        }

        public void SinceGroundUpdate()
        {
            if (!player.data.isPlaying) return;
            if (!grounded)
            {
                sinceGrounded += TimeHandler.fixedDeltaTime;
                if (sinceGrounded < 0)
                {
                    sinceGrounded = Mathf.Lerp(sinceGrounded, 0f, TimeHandler.fixedDeltaTime * 15f);
                }
            }
            else
            {
                sinceGrounded = 0;
            }
        }
        
        public override void UpdateEffects()
        {
            gunStatModifier.damage_mult = multiplier;
            CharacterStatModifiersExtension.GetAdditionalData(characterStatModifiers).damageReduction = 1/(multiplier <= 2 ? 1 : multiplier/2);
        }
        
        public override void OnApply()
        {
        }

        public override void OnRemove()
        {
            CharacterStatModifiersExtension.GetAdditionalData(characterStatModifiers).damageReduction = origReduction;
        }

        public override void Reset()
        {
            origReduction = CharacterStatModifiersExtension.GetAdditionalData(characterStatModifiers).damageReduction;
        }
    }
}