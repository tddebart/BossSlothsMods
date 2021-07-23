using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class GiveMeAnother : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Give me another";
        }

        protected override string GetDescription()
        {
            return "Clone a random valid card you have";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
#if DEBUG
            UnityEngine.Debug.Log("Adding " + cardInfo.cardName + " card");
#endif
            player.ExecuteAfterSeconds(0.2f, () =>
            {
                var tries = 0;
                while (!(tries > 50))
                {
                    var randomNum = Random.Range(0, player.data.currentCards.Count);
                    tries++;
                    if (!Utils.Cards.PlayerIsAllowedCard(player, player.data.currentCards[randomNum]) || player.data.currentCards[randomNum].cardName == "Give me another") continue;
                    var randomCard = player.data.currentCards[randomNum];
                    Utils.Cards.AddCardToPlayer(player, randomCard);
                    Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
                    break;
                }
            });
        }
        
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
#if DEBUG
            UnityEngine.Debug.Log("Setting up " + cardInfo.cardName + " card");
#endif
            cardInfo.allowMultiple = true;
            var cardData = new CardInfoAdditionalData()
            {
                canBeReassigned = false
            };
            cardInfo.AddData(cardData);
            transform.Find("CardBase(Clone)(Clone)/Canvas/Front/Grid/EffectText")?.gameObject.GetOrAddComponent<RainbowText>();
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
            return CardThemeColor.CardThemeColorType.MagicPink;
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