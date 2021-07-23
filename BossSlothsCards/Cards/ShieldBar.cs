using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class ShieldBar : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "ShieldBar";
        }

        protected override string GetDescription()
        {
            return "Description";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var shieldBar = Instantiate(player.transform.Find("WobbleObjects/Healthbar"), player.transform.Find("WobbleObjects"));
            shieldBar.name = "ShieldBar";
            shieldBar.Translate(new Vector3(0,0.35f,0));
            player.gameObject.AddComponent<ShieldBar_Mono>();
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;

        }

        protected override CardInfoStat[] GetStats()
        {
            return null;
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
            return CardThemeColor.CardThemeColorType.DestructiveRed;
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