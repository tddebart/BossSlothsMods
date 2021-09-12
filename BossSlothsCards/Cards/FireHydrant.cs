using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.TempEffects;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class FireHydrant : CustomCard
    {

        protected override string GetTitle()
        {
            return "Fire hydrant";
        }

        protected override string GetDescription()
        {
            return "You will always fire your entire clip in one burst and you will always shoot 1 projectile per bullet";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.AddComponent<FireHy_Mono>();
            //player.gameObject.AddComponent<FireHydrantEffect>();
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;

            gun.ammo = 3;
            gun.reloadTimeAdd = 0.2f;
            gun.timeBetweenBullets = 0.1f;
            gun.projectileColor = Color.cyan;
            //gun.lockGunToDefault = true;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new []
            {
                new CardInfoStat
                {
                    amount = "+0.20s",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Reload time"
                },
                new CardInfoStat
                {
                    amount = "+1",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Extra burst"
                },
                new CardInfoStat
                {
                    amount = "+3",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Ammo"
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }

        protected override GameObject GetCardArt()
        {
            return null;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.FirepowerYellow;
        }

        public override string GetModName()
        {
            return "BSC";
        }

        public override void OnRemoveCard()
        {
        }
        
    }
}