using System.Collections.Generic;
using System.Reflection;
using BossSlothsCards.Extensions;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;


namespace BossSlothsCards.Cards
{
    public class Larcenist : BossSlothCustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Larcenist";
        }

        protected override string GetDescription()
        {
            return "Steal the most recent card of the enemy";
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
            cardInfo.allowMultiple = true;

        }

        private void DoLarcenistThings(Player player)
        {

            foreach (var enemy in PlayerManager.instance.GetPlayersInTeam(
                PlayerManager.instance.GetOtherTeam(player.teamID)))
            {
                if (enemy.data.currentCards.Count == 0)
                {
                    return;
                }
                // get amount in currentCards
                var count = enemy.data.currentCards.Count - 1;
                while (true)
                {
                    if (enemy.data.currentCards.Count <= -1)
                    {
                        return;
                    }
                    // check if card is not larcenist
                    if (enemy.data.currentCards[count].cardName == "Larcenist")
                    {
                        count--;
                        continue;
                    }

                    // Add card to player
                    AddCardToPlayer(player, enemy.data.currentCards[count]);
                    // rpca event
                    if (base.GetComponent<PhotonView>().IsMine)
                    {
                        GetComponent<PhotonView>().RPC("RPCA_RemoveCard", RpcTarget.All,
                            new object[] { Extensions.Cards.instance.GetCardID(enemy.data.currentCards[count]), enemy.playerID, player.playerID});
                    }
                    break;
                }
                
            }


        }

        [PunRPC]
        public void RPCA_RemoveCard(int cardID, int enemyID, int assignerID)
        {
            var enemy = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithID",
                BindingFlags.Instance | BindingFlags.InvokeMethod |
                BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { enemyID });
            var assigner = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithID",
                BindingFlags.Instance | BindingFlags.InvokeMethod |
                BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { assignerID });
            var cardsList = global::CardChoice.instance.cards;
            var cardToRemove = cardsList[cardID];
            // get copy of currentCards
            var copyOfCurrentCards = new List<CardInfo>(enemy.data.currentCards);
            copyOfCurrentCards.Remove(cardToRemove);
            // clear enemy currentCards
            enemy.data.currentCards.Clear();
            // remove card from enemy in cardBar
            var cardBars = (CardBar[]) Traverse.Create(CardBarHandler.instance).Field("cardBars").GetValue();
            foreach (var cardBar in cardBars)
            {
                if (cardBar.gameObject.name == "Bar"+(enemyID+1))
                {
                    UnityEngine.Debug.LogWarning(cardBar.gameObject.name);
                    cardBar.ClearBar();
                }
            }
            // reset enemy stats
            typeof(Player).InvokeMember("FullReset", BindingFlags.Instance | BindingFlags.InvokeMethod |
                                                     BindingFlags.NonPublic, null, enemy, new object[] { });
            foreach(var cardC in copyOfCurrentCards)
            {
                //#TODO check ifit is a card that should not be readded
                if (!CardShouldNotBeAdded(cardC))
                {
                    AddCardToPlayer(enemy, cardC);
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Card: " + cardC.cardName + ". Should not be added");
                    
                    enemy.data.currentCards.Add(cardC);
                    
                    foreach (var cardBar in cardBars)
                    {
                        if (cardBar.gameObject.name == "Bar"+(enemyID+1))
                        {
                            cardBar.AddCard(cardC);
                        }
                    }
                }
            }
        }

        private static bool CardShouldNotBeAdded(CardInfo _card)
        {
            return _card.cardName.Contains("Gamble") || _card.cardName == "Larcenist" || _card.cardName.Contains("Jackpot");
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


        public override void OnRemoveCard()
        {
        }
        
    }
}