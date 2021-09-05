using System.Linq;
using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.Utils;
using HarmonyLib;
using ModdingUtils.MonoBehaviours;
using UnityEngine;

namespace BossSlothsCards.TempEffects
{
    public class OmegaEffect : CounterReversibleEffect
    {
        public bool OmegaActive;
        
        public SpecialEffectFrontGun effect;

        public override CounterStatus UpdateCounter()
        {
            var currentAmmo = (int)Traverse.Create(gunAmmo).Field("currentAmmo").GetValue();
            return currentAmmo == 1 ? CounterStatus.Apply : CounterStatus.Remove;
        }

        public override void UpdateEffects()
        {
            characterStatModifiersModifier.lifeSteal_mult = 1.5f;
            gunStatModifier.damage_mult = 1.5f;
            gunStatModifier.projectileSpeed_mult = 1.25f;
            if (data.currentCards.Any(cr => cr.cardName == "Sleight of hand"))
            {
                gunStatModifier.projectileColor = Colors.HueColourValue(Colors.HueColorNames.Orange);
            }
            else
            {
                gunStatModifier.projectileColor = Color.green;
            }
        }
        
        public override void OnStart()
        {
            base.OnStart();
            var effectObj = new GameObject("gunEffect");
            effectObj.transform.parent = transform;
            effect = effectObj.AddComponent<SpecialEffectFrontGun>();
            
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (data.currentCards.Any(cr => cr.cardName == "Sleight of hand"))
            {
                effect.particleColor = Colors.HueColourValue(Colors.HueColorNames.Orange);
            }
            else
            {
                effect.particleColor = Color.green;
            }
            effect.Active = status == CounterStatus.Apply;
        }

        public override void OnApply()
        {
            //gunAmmo = GetComponent<Holding>().holdable.GetComponentInChildren<GunAmmo>();
        }

        public override void Reset()
        {
            
        }
    }
}