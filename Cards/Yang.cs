using UnboundLib.Cards;
using UnityEngine;

namespace ActualRoundsMod.Cards
{
    public class Yang : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Yang";
        }

        protected override string GetDescription()
        {
            return "Ups block stats and makes minimum block cooldown 3s";
        }

        protected override CardInfoStat[] GetStats()
        {
            var info1 = new CardInfoStat {stat = "Damage", amount = "-30%", positive = false};
            var info2 = new CardInfoStat { stat = "Amount of Blocks", amount = "+2", positive = true};
            var info3 = new CardInfoStat { stat = "Reload time", amount = "+0.5s", positive = false};
            var info4 = new CardInfoStat {stat = "Block cooldown", amount = "-5s", positive = true};
            var info5 = new CardInfoStat {stat = "Ammo", amount = "-3", positive = false};
            var info6 = new CardInfoStat {stat = "ATKSPD", amount = "-30%", positive = false};
            var info = new[] {info1, info2, info3, info4, info5, info6};

            return info;
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
            return CardThemeColor.CardThemeColorType.TechWhite;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            Debug.Log("Setting up Yin Yang card");
            cardInfo.allowMultiple = false;
            gun.damage = 0.7f;
            gun.ammo = -3;
            gun.reloadTimeAdd = 0.5f;
            gun.attackSpeed = 0.7f;
            
            gun.projectileColor = Color.white;

            var A_Yang = new GameObject();
            A_Yang.AddComponent<A_Yang>();
            statModifiers.AddObjectToPlayer = A_Yang;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            Debug.Log("Adding Yin Yang card");
            block.additionalBlocks += 2;
            block.cooldown -= 5;
            block.forceToAdd += 3;
        }

        public override void OnRemoveCard()
        {
            Debug.Log("Removing Yin Yang card");
            
        }
        
    }
}