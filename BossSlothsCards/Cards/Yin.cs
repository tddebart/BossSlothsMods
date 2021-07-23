using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class Yin : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Yin";
        }

        protected override string GetDescription()
        {
            return "Ups gun stats";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding yin card");
#endif
            block.cdAdd += 5;
            
            block.additionalBlocks += -1;
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up yin card");
#endif
            cardInfo.allowMultiple = false;
            
            gun.damage = 1.30f;
            gun.reloadTimeAdd = -0.5f;
            gun.ammo = 3;
            gun.attackSpeed = 1.30f;

            gun.projectileColor = Color.black;
        }
        
        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    stat = "Damage",
                    amount = "+30%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Reload time",
                    amount = "-0.5s",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Block cooldown",
                    amount = "+5s",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Ammo",
                    amount = "+3",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "ATKSPD",
                    amount = "+30%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Additional blocks",
                    amount = "-1",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }

        protected override GameObject GetCardArt()
        {
            //var art = Testing.Instance.Asset.LoadAsset<GameObject>("C_Sneeze");
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