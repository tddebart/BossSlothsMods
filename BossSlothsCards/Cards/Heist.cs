using BossSlothsCards.Extensions;
using BossSlothsCards.Utils.Text;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using ModdingUtils.Extensions;
using ModdingUtils.Utils;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class Heist : CustomCard
    {

        private CardInfo.Rarity rarity;
        private string randomCardName;

        protected override string GetTitle()
        {
            return "Heist";
        }

        protected override string GetDescription()
        {
            return "Steal a random card from a random opponent but give them a random card of the same rarity";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            BossSlothCards.instance.ExecuteAfterSeconds(0.1f, () =>
            {
                DoHeistThings(player,gun,gunAmmo,data,health,gravity,block,characterStats);
            });
        }
        
        private void DoHeistThings(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var enemy = PlayerManager.instance.GetRandomEnemy(player);
            var tris = 0;
            while (enemy.data.currentCards.Count == 0 && tris < 5)
            {
                tris++;
                enemy = PlayerManager.instance.GetRandomEnemy(player);
            }
            if (enemy == null || enemy.data.currentCards.Count == 0)
            {
                return;
            }
            
            // get amount in currentCards
            var tries = 0;
            while (!(tries > 50))
            {
                tries++;
                if (enemy.data.currentCards.Count <= -1)
                {
                    return;
                }
                var randomCard = enemy.data.currentCards[Random.Range(0, enemy.data.currentCards.Count - 1)];
                // make sure the card is not
                if (!ModdingUtils.Utils.Cards.instance.CardIsNotBlacklisted(randomCard, new[] { CustomCardCategories.instance.CardCategory("CardManipulation"), CustomCardCategories.instance.CardCategory("NoRemove")}))
                {
                    continue;
                }
                if (!ModdingUtils.Utils.Cards.instance.PlayerIsAllowedCard(player, randomCard))
                {
                    continue;
                }

                rarity = randomCard.rarity;
                randomCardName = randomCard.cardName;

                var replacementCard = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, condition);
                
                ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(enemy, randomCard, ModdingUtils.Utils.Cards.SelectionType.Random);

                player.ExecuteAfterSeconds(0.2f, () =>
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, false, "", 0, 0, true);
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(enemy, replacementCard, false, "", 0, 0, true);
                    CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
                });
                break;
            }
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = true;
            cardInfo.GetAdditionalData().canBeReassigned = false;
            cardInfo.categories = new[] { CustomCardCategories.instance.CardCategory("CardManipulation") };
            
            transform.Find("CardBase(Clone)(Clone)/Canvas/Front/Grid/EffectText")?.gameObject.GetOrAddComponent<RainbowText>();
        }
        
        //From PCE
        public bool condition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // do not allow duplicates of cards with allowMultiple == false
            // card rarity must be as desired
            // card cannot be another Gamble / Jackpot card
            // card cannot be from a blacklisted category of any other card

            return card.cardName != randomCardName && card.cardName != "Heist" && card.rarity == rarity;
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