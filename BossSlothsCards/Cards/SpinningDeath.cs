using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.TempEffects;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class SpinningDeath : CustomCard
    {

        protected override string GetTitle()
        {
            return "Spinning death";
        }

        protected override string GetDescription()
        {
            return "Your bullets will create working saws where they hit when your countdown is 0";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<SawBladeEffect>();
            player.GetComponent<SawBladeEffect>().timeSinceLastSaw = 15;
            player.gameObject.GetOrAddComponent<SawCounter>();
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;

            statModifiers.health = 1.1f;

            var saw = (GameObject)Resources.Load("4 map objects/MapObject_Saw_Stat");
            var betterSaw = Instantiate(saw);
            DestroyImmediate(betterSaw.GetComponent<PhotonMapObject>());
            betterSaw.transform.Rotate(new Vector3(90,0 ));
            betterSaw.transform.position = new Vector3(1000, 0, 0);
            var betterSprite = betterSaw.transform.Find("Sprite").gameObject;
            betterSprite.transform.parent = null;
            betterSprite.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            betterSprite.transform.localScale = Vector3.one;
            betterSprite.hideFlags = HideFlags.HideAndDontSave;
            betterSprite.AddComponent<BetterSawObject>();

            gun.objectsToSpawn = new[]
            {
                new ObjectsToSpawn()
                {
                    AddToProjectile = betterSprite
                },
            };
        }

        protected override CardInfoStat[] GetStats()
        {
            return new []
            {
                new CardInfoStat
                {
                    amount = "+10%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Health"
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override GameObject GetCardArt()
        {
            return null;
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