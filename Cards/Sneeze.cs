using UnboundLib.Cards;
using UnityEngine;


namespace ActualRoundsMod.Cards
{
    public class Sneeze : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Sneeze";
        }

        protected override string GetDescription()
        {
            return "Make you sneeze your bullets";
        }

        protected override CardInfoStat[] GetStats()
        {
            var info1 = new CardInfoStat {stat = "Damage", amount = "-90%", positive = false};
            var info2 = new CardInfoStat { stat = "Reload time", amount = "+2s", positive = false};
            var info3 = new CardInfoStat {stat = "Ammo", amount = "+15", positive = true};
            var info = new [] {info1, info2, info3};

            return info;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }

        protected override GameObject GetCardArt()
        {
            var art = Startup.Instance.Asset.LoadAsset<GameObject>("C_Sneeze");
            return art;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.PoisonGreen;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            Debug.Log("Setting up Sneeze card");
            cardInfo.allowMultiple = false;
            gun.damage = 0.10f;
            gun.ammo = 15;
            gun.reloadTimeAdd = 2f;
            gun.numberOfProjectiles = 10;  
            gun.spread = 0.40f;
            gun.recoil = 5;
            gun.projectileColor = new Color(55, 230,122, 1);

        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            Debug.Log("Adding Sneeze card");
        }

        public override void OnRemoveCard()
        {
            Debug.Log("Removing Sneeze card");
            
        }
    }
}