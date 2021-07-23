using BossSlothsCards.Extensions;
using HarmonyLib;
using PCE.Cards;
using UnboundLib;

namespace BossSlothsCards.Patches
{
    public class quickPatchRemove
    {
        [HarmonyPatch(typeof(GambleCard), "SetupCard")]
        private class quickPatch
        {
            private static void Postfix(CardInfo cardInfo)
            {
                BossSlothCards.instance.ExecuteAfterSeconds(3, () =>
                {
                    //UnityEngine.Debug.LogWarning("Gamble: " + cardInfo.GetAdditionalData().canBeReassigned);
                    
                });
            }
        }
    }
}