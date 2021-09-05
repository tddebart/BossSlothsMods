using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.TempEffects;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class SplittingRounds : CustomCard
    {

        protected override string GetTitle()
        {
            return "Splitting rounds";
        }

        protected override string GetDescription()
        {
            return "Bullets will create an extra bullet on each bounce";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<SplitEffect>();
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = true;
            gun.reflects = 3;

            gun.attackSpeed = 1.15f;
            
            var obj = new GameObject("A_SplittingRounds");
            obj.hideFlags = HideFlags.HideAndDontSave;
            obj.AddComponent<NoSelfCollide>();
            
            var explosiveBullet = (GameObject)Resources.Load("0 cards/Mayhem");
            var A_ScreenEdge = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0];
            
            gun.objectsToSpawn = new[]
            {
                new ObjectsToSpawn
                {
                    AddToProjectile = obj
                },
                A_ScreenEdge
            };

        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    amount = "+3",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Bounces"
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
            return null;
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.FirepowerYellow;
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