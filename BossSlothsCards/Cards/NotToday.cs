using UnboundLib.Cards;
using UnityEngine;


namespace BossSlothsCards.Cards
{
    public class NotToday : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Not today";
        }

        protected override string GetDescription()
        {
            return "Have a 10% change to ignore the enemy block";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding NotToday card");
#endif
            gun.unblockable = true;
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up NotToday card");
#endif
            cardInfo.allowMultiple = false;
            
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    stat = "Damage",
                    amount = "-90%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
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
            return CardThemeColor.CardThemeColorType.PoisonGreen;
        }

        public override void OnRemoveCard()
        {
        }
    }
}