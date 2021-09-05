using BossSlothsCards.MonoBehaviours;
using ModdingUtils.MonoBehaviours;
using Sonigon;
using UnityEngine;
using UnityEngine.Events;

namespace BossSlothsCards.TempEffects
{
    public class AlphaEffect : CounterReversibleEffect
    {
        public bool AlphaActive;

        public SpecialEffectFrontGun effect;

        public override CounterStatus UpdateCounter()
        {
            return AlphaActive ? CounterStatus.Apply : CounterStatus.Remove;
        }

        public override void UpdateEffects()
        {
            characterStatModifiersModifier.lifeSteal_mult = 1.5f;
            gunStatModifier.damage_mult = 1.5f;
            gunStatModifier.projectileSpeed_mult = 1.25f;
            gunStatModifier.projectileColor = Color.red;
        }

        public override void OnApply()
        {
            
        }

        public override void OnAwake()
        {
            base.OnAwake();
            var reloadTrigger = new GameObject("Alpha_A");
            var trigger = reloadTrigger.AddComponent<ReloadTigger>();
            trigger.outOfAmmoEvent = new UnityEvent();
            trigger.outOfAmmoEvent.AddListener(() =>
            {
            });
            trigger.reloadDoneEvent = new UnityEvent();
            trigger.reloadDoneEvent.AddListener(() =>
            {
                AlphaActive = true;
            });
            
            reloadTrigger.transform.parent = player.transform;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            effect.Active = AlphaActive;
        }

        public override void OnStart()
        {
            base.OnStart();
            var effectObj = new GameObject("gunEffect");
            effectObj.transform.parent = transform;
            effect = effectObj.AddComponent<SpecialEffectFrontGun>();
            effect.particleColor = Color.red;
        }

        public override void Reset()
        {
            
        }
    }
}