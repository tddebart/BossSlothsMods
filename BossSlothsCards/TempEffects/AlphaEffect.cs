using ModdingUtils.MonoBehaviours;
using UnityEngine;
using UnityEngine.Events;

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

        public override void Reset()
        {
            
        }
    }
}