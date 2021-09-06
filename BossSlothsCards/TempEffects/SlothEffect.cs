using System;
using BossSlothsCards.Utils;
using ModdingUtils.MonoBehaviours;
using UnityEngine;

namespace BossSlothsCards.TempEffects
{
    public class SlothEffect : CounterReversibleEffect
    {
        public float multiplier;
        private const float timeToMax = 25;
        public float maxMultiplier = 3;

        public GameObject circle;
        public SpriteRenderer renderer;

        private float timeSinceAttack;
        
        public override CounterStatus UpdateCounter()
        {
            timeSinceAttack = gun.sinceAttack;
            multiplier = Mathf.Clamp(((maxMultiplier - 1f) / (timeToMax)) * timeSinceAttack + 1f, 1f, maxMultiplier);
            
            return CounterStatus.Apply;
        }

        public override void UpdateEffects()
        {
            gunStatModifier.damage_mult = multiplier;
        }

        public override void OnApply()
        {
            
        }

        public override void OnStart()
        {
            base.OnStart();
            circle = Instantiate(BossSlothCards.EffectAsset.LoadAsset<GameObject>("Orange circle"), transform.Find("Particles"));
            circle.name = "SlothCircle";
            Destroy(circle.GetComponent<Animator>());
            renderer = circle.GetComponent<SpriteRenderer>();
            circle.transform.localScale = new Vector3(0.38f, 0.38f, 0.38f);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (multiplier > 1)
            {
                circle.SetActive(true);
            }
            else
            {
                circle.SetActive(false);
            }

            var rendererColor = renderer.color;
            rendererColor.a = Mathf.InverseLerp(0, 25, timeSinceAttack);
            renderer.color = rendererColor;

            renderer.color = Color.Lerp(new Color32(98, 37, 14, 255), new Color32(255, 255, 10, 255),
                Mathf.InverseLerp(0, 25, timeSinceAttack));
        }

        public override void Reset()
        {
            
        }
        
    }
}