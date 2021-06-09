using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsMod.Cards
{
    public class YinYang : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Yin Yang";
        }

        protected override string GetDescription()
        {
            return "Ups most stats";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding YinYang card");
#endif
            block.cdAdd = 4;
            
            block.additionalBlocks = 1;
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up YinYang card");
#endif
            cardInfo.allowMultiple = false;
                        
            gun.damage = 1.15f;
            gun.reloadTimeAdd = 4;
            gun.ammo = 2;
            gun.numberOfProjectiles = 2;
            gun.attackSpeed = 1.10f;
            statModifiers.health = 1.2f;
            
            gun.spread = 0.05f;
        }
        
        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    stat = "Damage",
                    amount = "+15%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Reload time",
                    amount = "+4s",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Ammo",
                    amount = "+2",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Block cooldown",
                    amount = "+4s",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Bullets",
                    amount = "+2",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "ATKSPD",
                    amount = "+10%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Health",
                    amount = "+20%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override GameObject GetCardArt()
        {
            return Startup.ArtAsset.LoadAsset<GameObject>("C_YinYang");
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
        
        public override void OnRemoveCard()
        {
        }
    }
}