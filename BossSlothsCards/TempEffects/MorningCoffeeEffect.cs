using System;
using System.Collections;
using BossSlothsCards.Extensions;
using ModdingUtils.MonoBehaviours;
using UnboundLib.Utils;
using UnityEngine;

namespace BossSlothsCards.TempEffects
{
    public class MorningCoffeeEffect : CounterReversibleEffect
    {
        private float multiplier;
        public float timeSinceStandedStill;
        private const float timeToMax = 10;
        private bool isStandingStill;

        private Vector3 positionLastFrame;


        public override CounterStatus UpdateCounter()
        {
            multiplier = Mathf.Clamp(150 - (15 * timeSinceStandedStill), 0, 150);
            return CounterStatus.Apply;
        }

        public override void UpdateEffects()
        {
            characterStatModifiersModifier.movementSpeed_mult = multiplier / 100f + 1f;
        }

        public override void OnApply()
        {
        }

        public override void Reset()
        {
        }

        public override void OnStart()
        {
            StartCoroutine(multiplierCoroutine());
            base.OnStart();
        }

        public override void OnFixedUpdate()
        {
            if (positionLastFrame.Rounded() != transform.position.Rounded())
            {
                isStandingStill = false;
            }
            else
            {
                isStandingStill = true;
            }

            timeSinceStandedStill = Mathf.Clamp(timeSinceStandedStill, 0, 10);

            positionLastFrame = transform.position;
            base.OnFixedUpdate();
        }

        public override void OnOnDestroy()
        {
            StopCoroutine(multiplierCoroutine());
            base.OnOnDestroy();
        }

        public IEnumerator multiplierCoroutine()
        {
            yield return new WaitForSeconds(0.01f);
            if (isStandingStill)
            {
                timeSinceStandedStill -= 0.03f;
            }
            else
            {
                timeSinceStandedStill += 0.01f;
            }

            StartCoroutine(multiplierCoroutine());
        }
    }
}