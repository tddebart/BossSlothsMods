using BossSlothsCards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class BulletProofBullets : CustomCard
    {

        protected override string GetTitle()
        {
            return "BulletProof bullets";
        }

        protected override string GetDescription()
        {
            return "If you don't want your bullets to be bulleted, un-bullet your bullet so they can bullet better.\n\nYour bullets will bounce back any bullet it collides with";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;

            gun.reloadTimeAdd = 0.1f;
            gun.attackSpeed = 1.15f;
            
            var obj = new GameObject("NoCollide");
            obj.hideFlags = HideFlags.HideAndDontSave;
            obj.AddComponent<NoSelfCollide>();
            gun.objectsToSpawn = new[]
            {
                new ObjectsToSpawn
                {
                    AddToProjectile = obj,
                }
            };
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    amount = "+0.10s",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Reload time"
                },
                new CardInfoStat
                {
                    amount = "-15%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Attack speed"
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }

        protected override GameObject GetCardArt()
        {
            return BossSlothCards.ArtAsset.LoadAsset<GameObject>("C_BulletProof");
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
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