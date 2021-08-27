using BossSlothsCards.MonoBehaviours;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class SawbladeBullets : CustomCard
    {

        protected override string GetTitle()
        {
            return "Sawblade bullets";
        }

        protected override string GetDescription()
        {
            return "Your bullet turn into tiny sawblades";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;

            gun.projectileSpeed = 0.5f;
            gun.damage = 1.5f;
            gun.reflects = 2;
            gun.ammo = -2;

            var saw = (GameObject)Resources.Load("4 map objects/MapObject_Saw_Stat");
            var betterSaw = Instantiate(saw);
            DestroyImmediate(betterSaw.GetComponent<PhotonMapObject>());
            betterSaw.transform.Rotate(new Vector3(90,0 ));
            betterSaw.transform.position = new Vector3(1000, 0, 0);
            var betterSprite = betterSaw.transform.Find("Sprite").gameObject;
            betterSprite.transform.parent = null;
            betterSprite.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            betterSprite.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            betterSprite.hideFlags = HideFlags.HideAndDontSave;
            betterSprite.AddComponent<SawObject>();

            var explosiveBullet = (GameObject)Resources.Load("0 cards/Mayhem");
            var A_ScreenEdge = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0];

            gun.objectsToSpawn = new[]
            {
                new ObjectsToSpawn()
                {
                    AddToProjectile = betterSprite
                },
                A_ScreenEdge
            };
        }

        protected override CardInfoStat[] GetStats()
        {
            return new []
            {
                new CardInfoStat
                {
                    amount = "-50%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Bullet speed"
                },
                new CardInfoStat
                {
                    amount = "+50%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Damage"
                },
                new CardInfoStat
                {
                    amount = "+2",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Bounces"
                },
                new CardInfoStat
                {
                    amount = "-2",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Ammo"
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