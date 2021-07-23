using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class HitMeBabyOneMoreTime : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Hit me baby one more time";
        }

        protected override string GetDescription()
        {
            return "Every second you take <b><color=#a52a2aff>10%</color></b> damage, but when you get below <b><color=#a52a2aff>30%</color></b> <color=#00ff00ff>health</color> you get all damage this card had done back(since last heal)";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding " + cardInfo.cardName + " card");
#endif
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up " + cardInfo.cardName + " card");
#endif
            cardInfo.allowMultiple = false;

            statModifiers.lifeSteal = 0.35f;
            
            var ConsistentDamage = new GameObject("ConsistentDamage");
            var dealDamage = ConsistentDamage.AddComponent<MonoBehaviours.DealDamageToPlayer>();
            dealDamage.doPercentageDamage = true;
            dealDamage.damagePercentage = 0.1f;
            dealDamage.lethal = false;
            dealDamage.targetPlayer = MonoBehaviours.DealDamageToPlayer.TargetPlayer.Own;
            dealDamage.doConsistentDamage = true;
            dealDamage.timeBetweenDamage = 1;
            dealDamage.stopAtPercentage = true;
            dealDamage.percentageStop = 0.3f;
            
            statModifiers.AddObjectToPlayer = ConsistentDamage;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    amount = "+35%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Life steal"
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
        }
        
    }
}