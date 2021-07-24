using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class KnightsArmor : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Knights armor";
        }

        protected override string GetDescription()
        {
            return "Gives you a set of regenerating knight armor to protect you from damage";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (player.GetComponent<ArmorHandler>())
            {
                data.GetAdditionalData().maxArmor += 50;
                return;
            }
            var armorBar = Instantiate(player.transform.Find("WobbleObjects/Healthbar"), player.transform.Find("WobbleObjects"));
            armorBar.name = "ArmorBar";
            armorBar.Translate(new Vector3(0,0.35f,0));
            var armor = player.gameObject.AddComponent<Armor_Mono>();
            armor.maxArmor = 50;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = true;
            statModifiers.health = 0.7f;
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat
                {
                    amount = "+50",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Max armor"
                },
                new CardInfoStat
                {
                    amount = "-30%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Max HP"
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
            return CardThemeColor.CardThemeColorType.DefensiveBlue;
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