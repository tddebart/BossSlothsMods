using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Unused_cards
{
    public class OneShot : CustomCard
    {
        public AssetBundle Asset;

        protected override string GetTitle()
        {
            return "One shot";
        }

        protected override string GetDescription()
        {
            return "Adds a lot of damage";
        }

        protected override CardInfoStat[] GetStats()
        {
            var info1 = new CardInfoStat {stat = "Damage", amount = "+1000%", positive = true};
            var info2 = new CardInfoStat { stat = "Reload time", amount = "+5s", positive = false};
            var info3 = new CardInfoStat {stat = "Ammo", amount = "-100", positive = false};
            var info4 = new CardInfoStat {stat = "ATKSPD", amount = "-300%", positive = false};
            var info = new CardInfoStat[4] {info1, info2, info3, info4};

            return info;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override GameObject GetCardArt()
        {
            /*
            var asset = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "niceart"));
            Asset = asset;
            var art = asset.LoadAsset<GameObject>("C_Nice"); */
            
            return null;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            Debug.Log("Setting up Nice card");
            gun.damage = 50;
            gun.ammo = -100;
            gun.reloadTimeAdd = 5;
            gun.attackSpeed = 3;
            gun.projectileSpeed = 0.5f;
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            Debug.Log("Adding Nice card");
            gunAmmo.maxAmmo = 1;
        }

        public override void OnRemoveCard()
        {
            Debug.Log("Removing Nice card");
            
        }
    }
}