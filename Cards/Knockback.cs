using UnboundLib.Cards;
using UnityEngine;

namespace ActualRoundsMod.Cards
{
    public class Knockback : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Yeetus";
        }

        protected override string GetDescription()
        {
            return "";
        }

        protected override CardInfoStat[] GetStats()
        {
            var info1 = new CardInfoStat {stat = "Knockback", amount = "+200%", positive = true};
            var info = new[] {info1};

            return info;
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
            return CardThemeColor.CardThemeColorType.EvilPurple;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            Debug.Log("Setting up Sneeze card");
            cardInfo.allowMultiple = true;
            gun.knockback = 2;


        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            Debug.Log("Adding Sneeze card");
        }

        public override void OnRemoveCard()
        {
            Debug.Log("Removing Sneeze card");
            
        }
    }
}