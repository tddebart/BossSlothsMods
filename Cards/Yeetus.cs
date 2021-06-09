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
            return "Gives more bullet knockback and block knockback";
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            UnityEngine.Debug.Log("Setting up Yeetus card");
            cardInfo.allowMultiple = true;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            UnityEngine.Debug.Log("Adding Yeetus card");
            
            gun.knockback = 2;
            //characterStats.AddObjectToPlayer = Startup.EffectAsset.LoadAsset<GameObject>("A_Explode_Y");
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    stat = "Knockback",
                    amount = "+200%",
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
            return CardThemeColor.CardThemeColorType.EvilPurple;
        }
        
        public override void OnRemoveCard()
        {
        }
    }
}