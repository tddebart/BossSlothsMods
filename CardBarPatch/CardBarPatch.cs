using BepInEx;
using HarmonyLib;
using UnboundLib;
using UnityEngine.UI;
using UnityEngine;

namespace CardBarPatch
{
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInPlugin("com.BossSloth.CardBarPatch", "CardBarPatch", "1.0.0")]
    [BepInProcess("Rounds.exe")]
    public class CardBarPatch : BaseUnityPlugin
    {
        private static CardBarPatch instance;

        private void Start()
        {
            instance = this;
            
            var harmony = new Harmony("com.BossSloth.CardBarPatch");
            harmony.PatchAll();
        }
        
        public static void CardBar()
        {
            instance.ExecuteAfterSeconds(0.5f, () =>
            {
                foreach (var cardBar in (CardBar[]) Traverse.Create(CardBarHandler.instance).Field("cardBars").GetValue()) 
                {
                    var layoutGroup = cardBar.gameObject.GetComponent<HorizontalLayoutGroup>();
                    layoutGroup.spacing = 3;
                    var rectTransform = cardBar.gameObject.GetComponent<RectTransform>();
                    // ReSharper disable once NotAccessedVariable
                    rectTransform.offsetMin = new Vector2(-1450, rectTransform.offsetMin.y);
                }
            });
        }
    }
}