using BossSlothsCards.Extensions;
using BossSlothsCards.Utils.Text;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using ModdingUtils.Extensions;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;


namespace BossSlothsCards.Cards
{
    public class CopyCat : CustomCard
    {
        public AssetBundle Asset;

        protected override string GetTitle()
        {
            return "Copycat";
        }

        protected override string GetDescription()
        {
            return "Copy a random valid card from a random enemy";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding Copycat card");
#endif
            var enemy = PlayerManager.instance.GetRandomEnemy(player);
            if (enemy == null || enemy.data.currentCards.Count == 0) return;

            var tries = 0;
            while (!(tries > 50))
            {
                var randomNum = Random.Range(0, enemy.data.currentCards.Count);
                tries++;
                if (!ModdingUtils.Utils.Cards.instance.PlayerIsAllowedCard(player, enemy.data.currentCards[randomNum])) continue;
                var randomCard = enemy.data.currentCards[randomNum];
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, false, "", 0, 0, true);
                ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
                break;
            }
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up Copycat card");
#endif
            cardInfo.allowMultiple = true;
            cardInfo.GetAdditionalData().canBeReassigned = false;
            cardInfo.categories = new[] { CustomCardCategories.instance.CardCategory("CardManipulation") };
            transform.Find("CardBase(Clone)(Clone)/Canvas/Front/Grid/EffectText")?.gameObject.GetOrAddComponent<RainbowText>();
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

        public override string GetModName()
        {
            return "BSC";
        }

        public override void OnRemoveCard()
        {
        }
    }
}