using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Photon.Pun;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Extensions
{
    // parts of class from PCE(https://github.com/pdcook/PCE)
    public abstract class BossSlothCustomCard : CustomCard
    {
        // private bool MoreThan2Players = false;

        public Player GetRandomEnemy(Player player)
        {
            var players = new List<Player>(PlayerManager.instance.players);
            Player enemy = null;
            foreach (var _player in PlayerManager.instance.GetPlayersInTeam(player.teamID))
            {
                players.Remove(_player);
            }
            
            enemy = players[Random.Range(0, players.Count)];

            return enemy;
        }
        
        public static bool IsDeathMatch()
        {
            return PlayerManager.instance.players.Any(player => player.teamID >= 2);
        }
        
        //PCE
        public void AddCardToPlayer(Player player, CardInfo card)
        {
            // adds the card "card" to the player "player"
            if (card == null) { return; }
            else if (PhotonNetwork.OfflineMode)
            {
                // assign card locally
                ApplyCardStats cardStats = card.gameObject.GetComponentInChildren<ApplyCardStats>();
                cardStats.GetComponent<CardInfo>().sourceCard = card;
                cardStats.Pick(player.playerID, true, PickerType.Player);
            }
            else
            {
                // assign card with RPC

                Player[] array = new Player[] { player };
                int[] array2 = new int[array.Length];

                for (int j = 0; j < array.Length; j++)
                {
                    array2[j] = array[j].data.view.ControllerActorNr;
                }
                if (base.GetComponent<PhotonView>().IsMine)
                {

                    base.GetComponent<PhotonView>().RPC("RPCA_AssignCard", RpcTarget.All, new object[] { Cards.instance.GetCardID(card), array2 });

                }
            }
        }
        //PCE
        [PunRPC]
        public void RPCA_AssignCard(int cardID, int[] actorIDs)
        {
            Player playerToUpgrade;

            for (int i = 0; i < actorIDs.Length; i++)
            {
                CardInfo[] cards = global::CardChoice.instance.cards;
                ApplyCardStats cardStats = cards[cardID].gameObject.GetComponentInChildren<ApplyCardStats>();

                // call Start to initialize card stat components for base-game cards
                typeof(ApplyCardStats).InvokeMember("Start",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, cardStats, new object[] { });
                cardStats.GetComponent<CardInfo>().sourceCard = cards[cardID];

                playerToUpgrade = (Player)typeof(PlayerManager).InvokeMember("GetPlayerWithActorID",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, PlayerManager.instance, new object[] { actorIDs[i] });

                Traverse.Create(cardStats).Field("playerToUpgrade").SetValue(playerToUpgrade);

                typeof(ApplyCardStats).InvokeMember("ApplyStats",
                                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                                    BindingFlags.NonPublic, null, cardStats, new object[] { });
                CardBarHandler.instance.AddCard(playerToUpgrade.playerID, cardStats.GetComponent<CardInfo>().sourceCard);
            }
        }
    }
}