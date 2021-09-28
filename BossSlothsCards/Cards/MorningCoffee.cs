using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.TempEffects;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class MorningCoffee : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Morning coffee";
        }

        protected override string GetDescription()
        {
            return "Don't talk to me until I've had my morning coffee\nYou gain <color=green>+150%</color> movement speed but this trails of by 10% every second while you are running recharge by standing still";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.AddComponent<MorningCoffeeEffect>();
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            //statModifiers.movementSpeed = 1.5f;
            statModifiers.health = 0.80f;

        }

        protected override CardInfoStat[] GetStats()
        {
            return new []
            {
                new CardInfoStat
                {
                    amount = "-20%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "HP"
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
            return CardThemeColor.CardThemeColorType.NatureBrown;
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