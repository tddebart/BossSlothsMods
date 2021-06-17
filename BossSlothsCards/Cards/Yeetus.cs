using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
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
            return "Gives more bullet knockback";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Addig yeetus card");
#endif
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up yeetus card");
#endif
            cardInfo.allowMultiple = true;
            gun.knockback = 2;
            
            //statModifiers.AddObjectToPlayer = Startup.EffectAsset.LoadAsset<GameObject>("A_Explode_Y");
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
            return Startup.ArtAsset.LoadAsset<GameObject>("C_Yeetus");
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