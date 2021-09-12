using BossSlothsCards.Extensions;
using BossSlothsCards.Utils.Text;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using ModdingUtils.Extensions;
using Photon.Pun;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class Larcenist : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Larcenist";
        }

        protected override string GetDescription()
        {
            return "Steal the most recent valid card from a random enemy";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding larcenist card");
#endif
            DoLarcenistThings(player);
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up larcenist card");
#endif
            cardInfo.GetAdditionalData().canBeReassigned = false;
            cardInfo.categories = new[] { CustomCardCategories.instance.CardCategory("CardManipulation") };
            cardInfo.allowMultiple = true;
            transform.Find("CardBase(Clone)(Clone)/Canvas/Front/Grid/EffectText")?.gameObject.GetOrAddComponent<RainbowText>();

        }

        private static void DoLarcenistThings(Player player)
        {
            var enemy = PlayerManager.instance.GetRandomEnemy(player);
            if (enemy == null || enemy.data.currentCards.Count == 0) return;
            
            // get amount in currentCards
            var count = enemy.data.currentCards.Count - 1;
            var tries = 0;
            while (!(tries > 50))
            {
                if (enemy.data.currentCards.Count <= -1)
                {
                    return;
                }
                tries++;
                // check if card is not larcenist
                if (enemy.data.currentCards[count].cardName == "Larcenist")
                {
                    count--;
                    continue;
                }

                if (!ModdingUtils.Utils.Cards.instance.PlayerIsAllowedCard(player, enemy.data.currentCards[count]))
                {
                    count--;
                    continue;
                }

                var card = enemy.data.currentCards[count];

                // Add card to player
                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, card, false, "", 0, 0, true);
                ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, card);
                // Remove card from enemy
                ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(enemy, card, ModdingUtils.Utils.Cards.SelectionType.Newest);
                break;
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