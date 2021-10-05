using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using Photon.Pun;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using UnityEngine.Events;

namespace BossSlothsCards.Cards
{
    public class RecyclingDay : CustomCard
    {
        private GameObject reclyObj;

        protected override string GetTitle()
        {
            return "Recycling day";
        }

        protected override string GetDescription()
        {
            return "Blocking will create a box";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            block.cooldown -= 0.2f;
            
            player.gameObject.AddComponent<RecyclingDay_Mono>();
            reclyObj = new GameObject("recycling");
            reclyObj.AddComponent<BossSlothMonoBehaviour>();
            var jump = reclyObj.AddComponent<PlayerDoJump>();
            jump.multiplier = 1;
            var trigger = reclyObj.AddComponent<BlockTrigger>();
            trigger.blockRechargeEvent = new UnityEvent();
            trigger.successfulBlockEvent = new UnityEvent();
            trigger.triggerSuperFirstBlock = new UnityEvent();
            trigger.triggerFirstBlockThatDelaysOthers = new UnityEvent();
            trigger.triggerEventEarly = new UnityEvent();
            trigger.triggerEvent = new UnityEvent();
            trigger.triggerEvent.AddListener(() =>
            {
                if (jump.GetComponentInParent<CharacterStatModifiers>().GetAdditionalData().timeSinceLastBlockBox > 1.5f)
                {
                    jump.GetComponentInParent<CharacterStatModifiers>().GetAdditionalData().timeSinceLastBlockBox = 0;
                    jump.DoJump();
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.Instantiate("4 map objects/Box_Destructible_Small", player.transform.position, Quaternion.identity);
                        var scale = jump.transform.parent.transform.localScale;
                        jump.ExecuteAfterSeconds(0.08f, () =>
                        {
                            var parent = jump.transform.parent;
                            parent.GetComponent<PhotonView>().RPC("RPCA_FixBox", RpcTarget.All);
                            parent.GetComponent<PhotonView>().RPC("RPCA_BigBox", RpcTarget.All);
                        });
                        jump.ExecuteAfterSeconds(0.15f, () =>
                        {
                            var parent = jump.transform.parent;
                            parent.GetComponent<PhotonView>().RPC("RPCA_FixBox", RpcTarget.All);
                        });
                    }
                    // var rem = box.AddComponent<RemoveAfterSeconds>();
                    // rem.seconds = 4;
                    
                }
            });
            reclyObj.transform.SetParent(player.gameObject.transform);
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;

            statModifiers.health = 1.1f;

        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    amount = "+10%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Health"
                },
                new CardInfoStat
                {
                    amount = "-0.2s",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Block cooldown"
                }
            };
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
            return CardThemeColor.CardThemeColorType.NatureBrown;
        }

        public override string GetModName()
        {
            return "BSC";
        }

        public override void OnRemoveCard()
        {
            DestroyImmediate(reclyObj);
        }
        
    }
}