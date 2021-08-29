using BossSlothsCards.Extensions;
using Photon.Pun;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using UnityEngine.Events;

namespace BossSlothsCards.Cards
{
    public class RecyclingDay : CustomCard
    {

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
            var reclyObj = new GameObject("recycling");
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
                    if (jump.GetComponentInParent<PhotonView>().IsMine)
                    {
                        PhotonNetwork.Instantiate("4 map objects/Box_Destructible_Small", (player.transform.position), Quaternion.identity);
                    }
                    // var rem = box.AddComponent<RemoveAfterSeconds>();
                    // rem.seconds = 4;
                    
                    jump.ExecuteAfterSeconds(0.02f, () =>
                    {
                        var currentObj = MapManager.instance.currentMap.Map.gameObject.transform.GetChild(MapManager.instance.currentMap.Map.gameObject.transform.childCount - 1);
                        var rigid = currentObj.GetComponent<Rigidbody2D>();
                        rigid.isKinematic = false;
                        rigid.simulated = true;
                        currentObj.GetComponent<DamagableEvent>().deathEvent = new UnityEvent();
                        currentObj.GetComponent<DamagableEvent>().deathEvent.AddListener(() =>
                        {
                            Destroy(currentObj.gameObject);
                        });
                    });
                }
            });
            reclyObj.transform.SetParent(player.gameObject.transform);
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            
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