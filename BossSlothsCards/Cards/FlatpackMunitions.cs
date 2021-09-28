using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.TempEffects;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class FlatpackMunitions : CustomCard
    {

        protected override string GetTitle()
        {
            return "Flatpack munitions";
        }

        protected override string GetDescription()
        {
            return "Your bullets will drop a destructible box on hit with a cooldown of half a seconds";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.GetOrAddComponent<FlatpackMunitionsEffect>();
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;
            gun.reloadTimeAdd = 0.1f;
            gun.reflects = 2;
            
            var box = (GameObject)Resources.Load("4 map objects/Box_Destructible");
            var spriteRen = box.GetComponent<SpriteRenderer>();
            var obj = new GameObject();
            obj.transform.position = new Vector3(1000, 0, 0);
            obj.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            var rendrer = obj.AddComponent<SpriteRenderer>();
            rendrer.sprite = spriteRen.sprite;
            rendrer.color = spriteRen.color;
            obj.AddComponent<EffectBulletRotate>();
            var sf = obj.AddComponent<SFPolygon>();
            sf.verts = spriteRen.GetComponent<SFPolygon>().verts;
            sf._looped = true;
            sf.shadowLayers = -1;
            sf.opacity = 1;

            gun.objectsToSpawn = new[]
            {
                new ObjectsToSpawn()
                {
                    AddToProjectile = obj
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
                    amount = "+2",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Bounces"
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
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