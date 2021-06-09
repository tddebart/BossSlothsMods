<<<<<<< HEAD
﻿using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsMod.Unused_cards
=======
﻿using System.IO;
using UnboundLib.Cards;
using UnityEngine;

namespace ActualRoundsMod
>>>>>>> parent of b94a634 (Removed problem files (#3))
{
    public class Nice : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Nice";
        }

        protected override string GetDescription()
        {
            return "Adds Nice";
        }

        protected override CardInfoStat[] GetStats()
        {
            var info1 = new CardInfoStat {stat = "Nice", amount = "+100%", positive = true};
            var info2 = new CardInfoStat { stat = "Coolness", amount = "+73%", positive = true};
            var info = new CardInfoStat[2] {info1, info2};

            return info;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
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
            return CardThemeColor.CardThemeColorType.MagicPink;
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            Debug.Log("Setting up Nice card");
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity,
            Block block, CharacterStatModifiers characterStats)
        {
            Debug.Log("Adding Nice card");
        }

        public override void OnRemoveCard()
        {
            Debug.Log("Removing Nice card");
            
        }

        private void OnDestroy()
        {
            
        }
    }
}