using BossSlothsCards.Extensions;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class RocketJump : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Rocket jump";
        }

        protected override string GetDescription()
        {
            return "You become explosion resistant and your bullets explode on impact";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.GetAdditionalData().explosiveResistant = true;
            characterStats.GetAdditionalData().recoil += 1.5f;
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;

            var explosiveBullet = (GameObject)Resources.Load("0 cards/Explosive bullet");
            var a_Explosion = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0].effect;
            var explo = Instantiate(a_Explosion);
            explo.transform.position = new Vector3(1000, 0, 0);
            explo.hideFlags = HideFlags.HideAndDontSave;
            explo.name = "customExplo";
            DestroyImmediate(explo.GetComponent<RemoveAfterSeconds>());
            var explodsion = explo.GetComponent<Explosion>();
            explodsion.force = 15000;
            // On.Explosion.Explode += (orig, self) =>
            // {
            //     if (self == explodsion)
            //     {
            //         var rem = self.gameObject.AddComponent<RemoveAfterSeconds>();
            //         rem.seconds = 5f;
            //     }
            //     
            //     orig(self);
            // };

            gun.objectsToSpawn = new[]
            {
                new ObjectsToSpawn
                {
                    effect = explo,
                    normalOffset = 0.1f,
                    numberOfSpawns = 1,
                    scaleFromDamage = 0.5f,
                    scaleStackM = 0.7f,
                    scaleStacks = true,
                }
            };

        }

        protected override CardInfoStat[] GetStats()
        {
            return new []
            {
                new CardInfoStat
                {
                    amount = "+150%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Recoil"
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