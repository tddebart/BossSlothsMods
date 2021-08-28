using BossSlothsCards.TempEffects;
using UnboundLib.Cards;
using UnityEngine;
using UnityEngine.Events;

namespace BossSlothsCards.Cards
{
    public class Alpha : CustomCard
    {

        protected override string GetTitle()
        {
            return "Alpha";
        }

        protected override string GetDescription()
        {
            return "The first shot of each magazine will have:";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.AddComponent<AlphaEffect>();
            
            var reloadTrigger = new GameObject("Alpha_A");
            var trigger = reloadTrigger.AddComponent<ReloadTigger>();
            trigger.outOfAmmoEvent = new UnityEvent();
            trigger.outOfAmmoEvent.AddListener(() =>
            {
            });
            trigger.reloadDoneEvent = new UnityEvent();
            trigger.reloadDoneEvent.AddListener(() =>
            {
                reloadTrigger.GetComponentInParent<AlphaEffect>().AlphaActive = true;
            });
            
            reloadTrigger.transform.parent = player.transform;
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;

            gun.reloadTimeAdd = 0.5f;
            statModifiers.automaticReload = false;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    amount = "+75%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Lifesteal"
                },
                new CardInfoStat
                {
                    amount = "+75%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Damage"
                },
                new CardInfoStat
                {
                    amount = "+25%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Bullet Speed"
                },
                new CardInfoStat
                {
                    amount = "+0.5s",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Reload time"
                }
            };
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
            return CardThemeColor.CardThemeColorType.FirepowerYellow;
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