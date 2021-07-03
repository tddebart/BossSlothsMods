using BossSlothsCards.MonoBehaviours;
using HarmonyLib;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BossSlothsCards.Cards
{
    public class WhereDidItGo : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Where did it go";
        }

        protected override string GetDescription()
        {
            return "Randomly explodes a part of the map every round after 5s";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding WDIG card");
#endif
            player.gameObject.AddComponent<WhereDidItGo_Mono>();
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up WDIG card");
#endif
            cardInfo.allowMultiple = false;

        }

        protected override CardInfoStat[] GetStats()
        {
            return null;
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

        public override void OnRemoveCard()
        {
        }
    }
    
    [HarmonyPatch(typeof(MapManager),"OnLevelFinishedLoading")]
    internal class Patch_MapManager
    {
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(Scene scene, MapManager __instance)
        {
            __instance.ExecuteAfterSeconds(5, () =>
            {
                var objects = scene.GetRootGameObjects()[0].GetComponentsInChildren<SpriteRenderer>(false);
                foreach (var player in PlayerManager.instance.players)
                {
                    foreach (var behaviour in player.GetComponents<WhereDidItGo_Mono>())
                    {
                        behaviour.RemoveRandomObject(objects);
                    }
                }
            });
        }
    }
}