using System.Linq;
using BossSlothsCards.MonoBehaviours;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;


namespace BossSlothsCards.Cards
{
    public class NoThanks : CustomCard
    {
        public AssetBundle Asset;

        public string cardRemovedName;
        
        protected override string GetTitle()
        {
            return "No thanks";
        }

        protected override string GetDescription()
        {
            return "Replace your most recent card with a random card";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding NoThanks card");
#endif
            if (player.data.currentCards.Count == 0)
            {
                return;
            }
            // get amount in currentCards
            var count = player.data.currentCards.Count - 1;
            var tries = 0;
            while (!(tries > 50))
            {
                tries++;
                if (player.data.currentCards.Count <= -1)
                {
                    return;
                }
                // check if card is not NoThanks
                if (player.data.currentCards[count].cardName == "No thanks")
                {
                    count--;
                    continue;
                }
                
                var randomCard = Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, condition);
                UnityEngine.Debug.LogWarning(randomCard.cardName);
                
                cardRemovedName = player.data.currentCards[count].cardName;

                Utils.Cards.AddCardToPlayer(player, randomCard);
                UnityEngine.Debug.LogWarning(player.data.currentCards[count].cardName);
                Utils.Cards.RemoveCardFromPlayer(player, player.data.currentCards[count]);

                break;
            }
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up NoThanks card");
#endif
            cardInfo.allowMultiple = true;
            
            if (transform.Find("CardBase(Clone)(Clone)/Canvas/Front/Grid/EffectText"))
            {
                transform.Find("CardBase(Clone)(Clone)/Canvas/Front/Grid/EffectText").gameObject.GetOrAddComponent<RainbowText>();
            }
        }

        private void Update()
        {
            UnityEngine.Debug.LogWarning("Test");
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
            return CardThemeColor.CardThemeColorType.MagicPink;
        }


        public override void OnRemoveCard()
        {
        }
        
        public override string GetModName()
        {
            return "BSC";
        }
        
        // From PCE
        private bool condition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // do not allow duplicates of cards with allowMultiple == false
            // card cannot be the card that got removed
            // card cannot be from a blacklisted category of any other card

            bool blacklisted = false;

            foreach (CardInfo currentCard in player.data.currentCards)
            {
                if (card.categories.Intersect(currentCard.blacklistedCategories).Any())
                {
                    blacklisted = true;
                }
            }

            return !blacklisted && (card.allowMultiple || player.data.currentCards.All(cardinfo => cardinfo.name != card.name)) && !card.cardName.Contains(cardRemovedName) && !card.cardName.Contains("No thanks");

        }

    }
}
