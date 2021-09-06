using ModdingUtils.Extensions;
using ModdingUtils.MonoBehaviours;
using Photon.Pun;
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
        private const float timeToMax = 15;

        public GameObject wings;
        
        public override CounterStatus UpdateCounter()
        {
            if (!PlayerStatus.PlayerSimulated(player)) sinceGrounded = 0;
            SinceGroundUpdate();
            grounded = player.data.isGrounded || player.data.isWallGrab;
            multiplier = Mathf.Clamp(((maxMultiplier - 1f) / (timeToMax)) * sinceGrounded + 1f, 1f, maxMultiplier);
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

        public override void OnStart()
        {
            base.OnStart();
            wings = Instantiate(BossSlothCards.EffectAsset.LoadAsset<GameObject>("wings"), transform.Find("Particles"));
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //new Color32(255, 165, 67, 255);
            var scaleValue = Mathf.Lerp(0f, 0.8f, Mathf.Clamp(Mathf.InverseLerp(0.1f, 15, sinceGrounded), 0, 17));
            wings.transform.localScale = new Vector3(scaleValue, scaleValue);
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