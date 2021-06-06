using UnboundLib.Cards;
using UnityEngine;

namespace ActualRoundsMod.Cards
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

        protected override CardInfoStat[] GetStats()
        {
            var info1 = new CardInfoStat {stat = "Health", amount = "+30%", positive = true};
            var info = new[] {info1};

            return info;
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

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            Debug.Log("Setting up Sneeze card");
            cardInfo.allowMultiple = false;
            statModifiers.health = 1.3f;
            statModifiers.numberOfJumps = 1;

        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            Debug.Log("Adding Sneeze card");
            _data = data;
        }

        public override void OnRemoveCard()
        {
            
        }
    }
}