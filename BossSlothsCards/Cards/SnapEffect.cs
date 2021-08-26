using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.Utils.Text;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;


namespace BossSlothsCards.Cards
{
    public class SnapEffect : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Snap effect";
        }

        protected override string GetDescription()
        {
            return "Randomly explodes a part of the map every 7s";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<ExplosionMap>();
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = true;

            if (transform.Find("CardBase(Clone)(Clone)/Canvas/Front/Text_Name"))
            {
                transform.Find("CardBase(Clone)(Clone)/Canvas/Front/Text_Name").gameObject.GetOrAddComponent<RainbowText>();
            }
            if (transform.Find("CardBase(Clone)(Clone)/Canvas/Front/Grid/EffectText"))
            {
                transform.Find("CardBase(Clone)(Clone)/Canvas/Front/Grid/EffectText").gameObject.GetOrAddComponent<RainbowText>();
            }
        }

        protected override CardInfoStat[] GetStats()
        {
            return null;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
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