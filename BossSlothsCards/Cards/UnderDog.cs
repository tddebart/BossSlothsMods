using BossSlothsCards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class UnderDog : CustomCard
    {

        protected override string GetTitle()
        {
            return "Underdog";
        }

        protected override string GetDescription()
        {
            return "Your bullets will do 50% more <color=red>damage</color> to player with more <color=green>HP</color> than you";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.AddComponent<Underdog_Mono>();
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;

            gun.reloadTimeAdd = 0.2f;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    amount = "+0.2s",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = ""
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
            return CardThemeColor.CardThemeColorType.MagicPink;
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