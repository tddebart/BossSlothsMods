using BossSlothsCards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;


namespace BossSlothsCards.Cards
{
    public class MomGetTheCamera : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Mom get the camera";
        }

        protected override string GetDescription()
        {
            return "Do a 360 every 2s to get a boost.\nFor each 360: ";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding 360 card");
#endif
            player.gameObject.AddComponent<GetCamera_Mono>();
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up 360 card");
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
                    amount = "+45%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.Some
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }

        protected override GameObject GetCardArt()
        {
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