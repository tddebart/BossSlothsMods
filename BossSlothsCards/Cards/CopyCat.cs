using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using UnityEngine;


namespace BossSlothsCards.Cards
{
    public class CopyCat : BossSlothCustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Copycat";
        }

        protected override string GetDescription()
        {
            return "Copy a random card from each enemy";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding Copycat card");
#endif
            foreach (var enemy in PlayerManager.instance.GetPlayersInTeam(
                PlayerManager.instance.GetOtherTeam(player.teamID)))
            {
                if (enemy.data.currentCards.Count == 0)
                {
                    return;
                }

                var randomNum = Random.Range(0, enemy.data.currentCards.Count-1);
                AddCardToPlayer(player, enemy.data.currentCards[randomNum]);
            }
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up Copycat card");
#endif
            cardInfo.allowMultiple = true;

            
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
}