using System.Linq;
using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.Utils;
using HarmonyLib;
using ModdingUtils.MonoBehaviours;
using UnityEngine;

namespace BossSlothsCards.TempEffects
{
    public class SleightOfHandEffect : CounterReversibleEffect
    {

        public SpecialEffectFrontGun effect;
        
        public override CounterStatus UpdateCounter()
        {
            var currentAmmo = (int)Traverse.Create(gunAmmo).Field("currentAmmo").GetValue();
            return currentAmmo == 1 ? CounterStatus.Apply : CounterStatus.Remove;
        }

        public override void UpdateEffects()
        {
            gunStatModifier.numberOfProjectiles_mult = 2;
            gunStatModifier.spread_add = 0.12f;
            gunStatModifier.damage_mult = 1.2f;
            if (data.currentCards.Any(cr => cr.cardName == "Omega"))
            {
                gunStatModifier.projectileColor = Colors.HueColourValue(Colors.HueColorNames.Orange);
            }
            else
            {
                gunStatModifier.projectileColor = Colors.HueColourValue(Colors.HueColorNames.Pink);
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
            if (data.currentCards.Any(cr => cr.cardName == "Omega"))
            {
                effect.particleColor = Colors.HueColourValue(Colors.HueColorNames.Orange);
            }
            else
            {
                effect.particleColor = Colors.HueColourValue(Colors.HueColorNames.Pink);
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