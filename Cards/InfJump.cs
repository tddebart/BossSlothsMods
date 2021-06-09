using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsMod.Cards
{
    public class InfJump : CustomCard
    {
        public AssetBundle Asset;
        public CharacterData _data;
        
        protected override string GetTitle()
        {
            return "Infinite jumps";
        }

        protected override string GetDescription()
        {
            return "";
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
            cardInfo.allowMultiple = false;
            
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
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
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