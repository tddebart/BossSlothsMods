using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class DoubleJump : CustomCard
    {
        public AssetBundle Asset;
        public CharacterData _data;
        
        protected override string GetTitle()
        {
            return "Double jump";
        }

        protected override string GetDescription()
        {
            return "Gives you the ability to double jump";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding jump card");
#endif
            _data = data;
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up jump card");
#endif
            cardInfo.allowMultiple = true;
            
            statModifiers.health = 1.3f;
            statModifiers.numberOfJumps = 1;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    stat = "Health",
                    amount = "+30%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Jumps",
                    amount = "+1",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }

        protected override GameObject GetCardArt()
        {
            //var art = Testing.Instance.Asset.LoadAsset<GameObject>("C_Sneeze");
            return null;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.FirepowerYellow;
        }

        public override void OnRemoveCard()
        {
        }
    }
}