using BossSlothsCards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
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
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding yang card");
#endif
            block.additionalBlocks += 2;
            block.cooldown -= 5;
            
            block.forceToAdd += 3;
            
            var A_Yang = new GameObject("A_Yang");
            A_Yang.AddComponent<Yang_Mono>();
            statModifiers.AddObjectToPlayer = A_Yang;
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up yang card");
#endif
            cardInfo.allowMultiple = false;

            gun.damage = 0.7f;
            gun.reloadTimeAdd = 0.5f;
            gun.ammo = -3;
            gun.attackSpeed = 0.7f;

            gun.projectileColor = Color.white;



        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    stat = "Damage",
                    amount = "-30%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Reload time",
                    amount = "+0.5s",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Amount of Blocks",
                    amount = "+2",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Block cooldown",
                    amount = "-4s",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Ammo",
                    amount = "-3",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "ATKSPD",
                    amount = "-30%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
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
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
        
        public override void OnRemoveCard()
        {
        }
    }
}