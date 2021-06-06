using UnboundLib.Cards;
using UnityEngine;


namespace ActualRoundsMod
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

        protected override CardInfoStat[] GetStats()
        {
            var info1 = new CardInfoStat {stat = "Damage", amount = "+10%", positive = true};
            var info2 = new CardInfoStat { stat = "Reload time", amount = "+4s", positive = false};
            var info3 = new CardInfoStat {stat = "Block cooldown", amount = "+4s", positive = false};
            var info4 = new CardInfoStat {stat = "Ammo", amount = "+2", positive = true};
            var info5 = new CardInfoStat {stat = "ATKSPD", amount = "+10%", positive = true};
            var info6 = new CardInfoStat {stat = "Projectiles", amount = "+2", positive = true};
            var info7 = new CardInfoStat {stat = "Health", amount = "+20%", positive = true};
            var info = new [] {info1, info2, info3, info4, info5, info6, info7};

            return info;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override GameObject GetCardArt()
        {
            //var art = Testing.Instance.Asset.LoadAsset<GameObject>("C_Sneeze");
            return null;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            Debug.Log("Setting up Yin Yang card");
            cardInfo.allowMultiple = false;
            gun.damage = 1.10f;
            gun.ammo = 2;
            gun.reloadTimeAdd = 4;
            gun.numberOfProjectiles = 2;
            gun.attackSpeed = 1.10f;
            gun.spread = 0.01f;

            statModifiers.health = 1.2f;
            
            

        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            Debug.Log("Adding Yin Yang card");
            block.additionalBlocks = 1;
            block.cdAdd = 4;

        }

        public override void OnRemoveCard()
        {
            Debug.Log("Removing Yin Yang card");
            
        }
    }
}