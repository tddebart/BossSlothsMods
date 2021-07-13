using System.Collections.Generic;
using System.Reflection;
using BossSlothsCards.Extensions;
using HarmonyLib;
using Photon.Pun;
using UnboundLib;
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
            return "Steal the most recent card of a random enemy";
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
            var enemy = GetRandomEnemy(player);   
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
                if (GetComponent<PhotonView>().IsMine)
                {
                    GetComponent<PhotonView>().RPC("RPCA_RemoveCard", RpcTarget.All,
                        new object[] { Extensions.Cards.instance.GetCardID(enemy.data.currentCards[count]), enemy.playerID});
                }
                break;
            }
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