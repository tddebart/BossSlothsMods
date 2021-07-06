using BossSlothsCards.MonoBehaviours;
using HarmonyLib;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BossSlothsCards.Cards
{
    public class RandomConfringo : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Random confringo";
        }

        protected override string GetDescription()
        {
            return "Randomly explodes a part of the map every round after 5s";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding confringo card");
#endif
            player.gameObject.AddComponent<Confringo_Mono>();
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up confringo card");
#endif
            cardInfo.allowMultiple = false;

        }

        protected override CardInfoStat[] GetStats()
        {
            return null;
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
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }

        public override void OnRemoveCard()
        {
        }
    }
}