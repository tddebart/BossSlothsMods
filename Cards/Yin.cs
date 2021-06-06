using UnboundLib.Cards;
using UnityEngine;

namespace ActualRoundsMod.Cards
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

        protected override CardInfoStat[] GetStats()
        {
            var info1 = new CardInfoStat {stat = "Damage", amount = "+30%", positive = true};
            var info2 = new CardInfoStat { stat = "Reload time", amount = "-0.5s", positive = true};
            var info3 = new CardInfoStat {stat = "Block cooldown", amount = "+5s", positive = false};
            var info4 = new CardInfoStat {stat = "Ammo", amount = "+3", positive = true};
            var info5 = new CardInfoStat {stat = "ATKSPD", amount = "+30%", positive = true};
            var info = new[] {info1, info2, info3, info4, info5};

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
            return CardThemeColor.CardThemeColorType.NatureBrown;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            Debug.Log("Setting up Yin Yang card");
            cardInfo.allowMultiple = false;
            gun.damage = 1.30f;
            gun.ammo = 3;
            gun.reloadTimeAdd = -0.5f;
            gun.attackSpeed = 1.30f;

            gun.projectileColor = Color.black;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            Debug.Log("Adding Yin Yang card");
            block.additionalBlocks += -1;
            block.cdAdd += 5;
        }

        public override void OnRemoveCard()
        {
            Debug.Log("Removing Yin Yang card");
            
        }
    }
}