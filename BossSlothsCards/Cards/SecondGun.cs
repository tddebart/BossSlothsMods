using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class SecondGun : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Second gun";
        }

        protected override string GetDescription()
        {
            return "Why not two? \n(Disclaimer: Does not actually give you a second gun.)"; 
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            gun.numberOfProjectiles *= 2;
            gun.damage *= 0.5f;
            gun.spread = 0.05f;
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    amount = "Double",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Projectile amount"
                },
                new CardInfoStat
                {
                    amount = "Half",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Damage"
                },
                new CardInfoStat
                {
                    amount = "+5%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Spread"
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }

        protected override GameObject GetCardArt()
        {
            return BossSlothCards.ArtAsset.LoadAsset<GameObject>("C_SecondGun");
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
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