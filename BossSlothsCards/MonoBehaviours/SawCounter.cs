using System;
using BossSlothsCards.TempEffects;
using On.Photon.Compression;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class SawCounter : ChargeCounter
    {
        private SawBladeEffect effect;
        public override void Start()
        {
            base.Start();
            effect = GetComponent<SawBladeEffect>();
            //Instantiate(BossSlothCards.EffectAsset.LoadAsset<GameObject>("SawSprite"),chargeCounterObj.transform);
            backgroundColor = new Color(0.5377358f, 0.2257476f, 0.2509089f);
        }
        
        public override void Update()
        {
            text = Mathf.Round(Mathf.Clamp(-(effect.timeSinceLastSaw - 15), 0, 15)).ToString();
            base.Update();
        }        
    }
}