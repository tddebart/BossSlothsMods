using System.Linq;
using BossSlothsCards.Extensions;
using Photon.Pun;
using UnityEngine;


namespace BossSlothsCards.Cards
{
    public class NoThanks : BossSlothCustomCard
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
            DoCardThings(player, gun, gunAmmo, data, health, gravity, block, characterStats);
        }

        private void DoCardThings(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (player.data.currentCards.Count == 0)
            {
                return;
            }
            // get amount in currentCards
            var count = player.data.currentCards.Count - 1;
            while (true)
            {
                if (player.data.currentCards.Count <= -1)
                {
                    return;
                }
                // check if card is not larcenist
                if (player.data.currentCards[count].cardName == "Larcenist")
                {
                    count--;
                    continue;
                }
                
                cardRemovedName = player.data.currentCards[count].cardName;

                var randomCard = Extensions.Cards.instance.GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, condition);
                
                
                // Add card to player
                AddCardToPlayer(player, randomCard);
                
                // rpca event
                if (GetComponent<PhotonView>().IsMine)
                {
                    GetComponent<PhotonView>().RPC("RPCA_RemoveCard", RpcTarget.All,
                        new object[] { Extensions.Cards.instance.GetCardID(player.data.currentCards[count]), player.playerID});
                }

                
                break;
            }
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up NoThanks card");
#endif
            cardInfo.allowMultiple = true;

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

            return !blacklisted && (card.allowMultiple || !player.data.currentCards.Where(cardinfo => cardinfo.name == card.name).Any()) && !card.cardName.Contains(cardRemovedName) && !card.cardName.Contains("No thanks");

        }

    }
}
