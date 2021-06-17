using UnboundLib.Cards;
using UnityEngine;


namespace BossSlothsCards.Cards
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
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding Sneeze card");
#endif
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up Sneeze card");
#endif
            cardInfo.allowMultiple = false;
            
            gun.damage = 0.10f;
            gun.reloadTimeAdd = 2f;
            gun.ammo = 15;
            gun.numberOfProjectiles = 10;

            gun.spread = 0.40f;
            gun.recoil = 500;
            
            gun.projectileColor = new Color(0.216f, 0.902f,0.478f);
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    stat = "Damage",
                    amount = "-90%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Reload time",
                    amount = "+2s",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Ammo",
                    amount = "+15",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat
                {
                    stat = "Bullets",
                    amount = "+10",
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
            return BossSlothCards.ArtAsset.LoadAsset<GameObject>("C_Sneeze");
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.PoisonGreen;
        }

        public override void OnRemoveCard()
        {
        }
    }
}