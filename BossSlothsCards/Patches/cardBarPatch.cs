using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace BossSlothsCards.Patches
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
                    switch (rectTransform.name)
                    {
                        case "Bar1":
                            rectTransform.offsetMin = new Vector2(-1450, -69);
                            break;
                        case "Bar2":
                        case "Bar3":
                        case "Bar4":
                            rectTransform.offsetMin = new Vector2(-1450, -122.5f);
                            break;
                    }
                }
            }
        }
        
        

        
    }
}