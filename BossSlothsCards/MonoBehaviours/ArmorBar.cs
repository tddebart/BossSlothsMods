using System;
using BossSlothsCards.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace BossSlothsCards.MonoBehaviours
{
    public class ArmorBar : MonoBehaviour
    {
        // Token: 0x04000848 RID: 2120
        public Image shield;

        // Token: 0x04000849 RID: 2121
        public Image white;

        // Token: 0x0400084A RID: 2122
        private float drag = 25f;

        // Token: 0x0400084B RID: 2123
        private float spring = 25f;

        // Token: 0x0400084C RID: 2124
        private float shCur;

        // Token: 0x0400084D RID: 2125
        private float shVel;

        // Token: 0x0400084E RID: 2126
        private float shTarg;

        // Token: 0x0400084F RID: 2127
        private float whiteCur;

        // Token: 0x04000850 RID: 2128
        private float whiteVel;

        // Token: 0x04000851 RID: 2129
        private float whiteTarg;

        // Token: 0x04000852 RID: 2130
        private float sinceDamage;

        private ArmorHandler shieldHandler;

        // Token: 0x04000853 RID: 2131
        private CharacterData data;
        // Token: 0x060006E1 RID: 1761 RVA: 0x00026204 File Offset: 0x00024404
        private void Start()
        {
            data = GetComponentInParent<CharacterData>();
            CharacterStatModifiers componentInParent = GetComponentInParent<CharacterStatModifiers>();
            componentInParent.WasDealtDamageAction = (Action<Vector2, bool>)Delegate.Combine(componentInParent.WasDealtDamageAction, new Action<Vector2, bool>(TakeDamage));
            shield = transform.Find("Canvas/Image/Health").GetComponent<Image>();
            white = transform.Find("Canvas/Image/White").GetComponent<Image>();
            shieldHandler = data.GetComponent<ArmorHandler>();
        }

        // Token: 0x060006E2 RID: 1762 RVA: 0x0002623C File Offset: 0x0002443C
        private void Update()
        {
            shTarg = data.GetAdditionalData().armor / data.GetAdditionalData().maxArmor;
            sinceDamage += TimeHandler.deltaTime;
            shVel = FRILerp.Lerp(shVel, (shTarg - shCur) * spring, drag);
            whiteVel = FRILerp.Lerp(whiteVel, (whiteTarg - whiteCur) * spring, drag);
            shCur += shVel * TimeHandler.deltaTime;
            whiteCur += whiteVel * TimeHandler.deltaTime;
            shield.fillAmount = shCur;
            white.fillAmount = whiteCur;
            if (sinceDamage > 0.5f)
            {
                whiteTarg = shTarg;
            }
            
            if (shieldHandler.regeneration > 0f && sinceDamage > 2f)
            {
                shieldHandler.Heal(shieldHandler.regeneration * TimeHandler.deltaTime);
            }
        }

        // Token: 0x060006E3 RID: 1763 RVA: 0x0002633B File Offset: 0x0002453B
        public void TakeDamage(Vector2 dmg, bool selfDmg)
        {
            sinceDamage = 0f;
        }

    }
}