using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace BossSlothsMod.Patches
{
    internal class cardBarPatch
    {

        [HarmonyPatch(typeof (CardBarHandler), "Start")]
        private class Patch_Jump
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(CardBar[] ___cardBars)
            {
                foreach (var cardBar in ___cardBars)
                {
                    var layoutGroup = cardBar.gameObject.GetComponent<HorizontalLayoutGroup>();
                    layoutGroup.spacing = 3;
                    var rectTransform = cardBar.gameObject.GetComponent<RectTransform>();
                    rectTransform.offsetMin = new Vector2(-1000, -69);
                }
            }
        }
        
        

        
    }
}