using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using TMPro;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine.UI;
using UnityEngine;

namespace CardBarPatch
{
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInPlugin(GUID, "CardBarPatch", "2.0.0")]
    [BepInProcess("Rounds.exe")]
    public class CardBarPatch : BaseUnityPlugin
    {
        private const string GUID = "com.BossSloth.CardBarPatch";
        
        public static CardBarPatch instance;

        private static ConfigEntry<KeyCode> detectedKey;
        public static ConfigEntry<float> spacing;
        private static ConfigEntry<float> distanceFromRight;
        public static ConfigEntry<float> horizontalOffset;
        public static ConfigEntry<float> cardSize;
        public static ConfigEntry<float> verticalDistance;
        public static ConfigEntry<bool> autoHide;
        public static ConfigEntry<float> opacity;

        private static float previewCards = 10;
        private static float previewTeams = 1;

        private static TextMeshProUGUI keyText;
        public static TextMeshProUGUI estimatedText;

        private static float adjustedCardSize = 0;

        private static GameObject button;

        private bool detectKey;

        private bool firstTime = true;
        
        public static float estimatedCards;

        private void Awake()
        {
            spacing = Config.Bind("Card bar patch", "Spacing", 3f, "");
            distanceFromRight = Config.Bind("Card bar patch", "Distance from right", 35f, "");
            horizontalOffset = Config.Bind("Card bar patch", "Horizontal offset", 145f, "");
            detectedKey = Config.Bind("Card bar patch", "Toggle keybind", KeyCode.P, "");
            cardSize = Config.Bind("Card bar patch", "Card size", 40f, "");
            verticalDistance = Config.Bind("Card bar patch", "Vertical distance", 50f, "");
            autoHide = Config.Bind("Card bar patch", "Auto hide during battle", false, "");
            opacity = Config.Bind("Card bar patch", "Opacity", 100f, "");

            Unbound.RegisterClientSideMod(GUID);
        }

        private void Start()
        {
            instance = this;
            On.MainMenuHandler.Awake += (orig, self) =>
            {
                self.ExecuteAfterFrames(3, () =>
                {
                    if (CardBarHandler.instance != null)
                    {
                        CardBarHandler.instance.ResetCardBards();
                    }
                });

                orig(self);
            };
            
            var harmony = new Harmony("com.BossSloth.CardBarPatch");
            harmony.PatchAll();
            
            Unbound.RegisterMenu("Card bar patch", () =>
            {
                CardBarHandler.instance.gameObject.SetActive(true);
                CardBarHandler.instance.Rebuild();
                this.ExecuteAfterSeconds(0.1f, UpdatePreview);
                UpdateCardBar();
            }, CreateMenu, null);

        }

        private void CreateMenu(GameObject menu)
        {

            button = MenuHandler.CreateButton("Set toggle card bar keybind", menu, () => { detectKey = true; }, 35);
            MenuHandler.CreateText("Toggle card bar keybind is: ", menu, out keyText, 40);
            
            MenuHandler.CreateSlider("Preview cards", menu, 40, 1, 50, 10, (val) =>
                {
                    previewCards = val;
                    UpdatePreview();
                },
                out _, true);
            MenuHandler.CreateSlider("Preview teams", menu, 40, 1, 4, 1, (val) =>
                {
                    previewTeams = val;
                    UpdatePreview();
                },
                out _, true);

            MenuHandler.CreateText("Estimated cards that will be visible with these settings: ", menu,
                out estimatedText, 30);

            MenuHandler.CreateSlider("Spacing", menu, 40, 0, 10, spacing.Value, (val) =>
                {
                    spacing.Value = val;
                    UpdateCardBar();
                },
                out var spacingSlider, true);
            
            MenuHandler.CreateSlider("Right side dist(Def: 35)", menu, 40, 0, 100, distanceFromRight.Value, (val) =>
            {
                distanceFromRight.Value = val;
                UpdateCardBar();
            }, out var rightSideSlider, true);
            
            MenuHandler.CreateSlider("Horizontal offset(Def: 145)", menu, 40, 20, 190, horizontalOffset.Value, (val) =>
                {
                    horizontalOffset.Value = val;
                    UpdateCardBar();
                },
                out var horizontalSlider, true);
            
            MenuHandler.CreateSlider("Card size(Def: 40)", menu, 40, 20, 50, cardSize.Value, (val) =>
                {
                    cardSize.Value = val;
                    UpdateCardBar();
                },
                out var cardSizeSlider, true);
            
            MenuHandler.CreateSlider("Vertical dist(Def: 50)", menu, 40, 20, 60, verticalDistance.Value, (val) =>
                {
                    verticalDistance.Value = val;
                    UpdateCardBar();
                },
                out var verticalSlider, true);
            MenuHandler.CreateSlider( "Opacity(Def: 100)", menu, 40, 0, 100, opacity.Value, (val) =>
                {
                    opacity.Value = val;
                    UpdateCardBar();
                },
                out var opacitySlider, true);

            var toggle = MenuHandler.CreateToggle(autoHide.Value, "Auto hide during battle", menu,
                arg0 => { autoHide.Value = arg0;}, 40);
            MenuHandler.CreateButton("Reset all", menu, () =>
            {
                spacing.Value = 3;
                distanceFromRight.Value = 35;
                horizontalOffset.Value = 145;
                cardSize.Value = 40;
                verticalDistance.Value = 50;
                opacity.Value = 100;
                autoHide.Value = false;

                spacingSlider.value = spacing.Value;
                rightSideSlider.value = distanceFromRight.Value;
                horizontalSlider.value = horizontalOffset.Value;
                cardSizeSlider.value = cardSize.Value;
                verticalSlider.value = verticalDistance.Value;
                opacitySlider.value = opacity.Value;
                toggle.GetComponentInChildren<Toggle>().isOn = autoHide.Value;
            }, 40);

            // Create back actions
            menu.GetComponentInChildren<GoBack>(true).goBackEvent.AddListener(() =>
            {
                CardBarHandler.instance.ResetCardBards();
            });
            menu.transform.Find("Group/Back").gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                CardBarHandler.instance.ResetCardBards();
            });
            
            CardBarHandler.instance.ResetCardBards();
            firstTime = false;
        }

        private bool hasToggled;
        private bool haveDetectedKey;

        private void Update()
        {
            if (Input.GetKeyDown(detectedKey.Value) && CardBarHandler.instance != null && !GameManager.lockInput)
            {
                CardBarHandler.instance.gameObject.SetActive(!CardBarHandler.instance.gameObject.activeInHierarchy);
            }

            if (autoHide.Value && CardBarHandler.instance != null && GameManager.instance.battleOngoing && !hasToggled)
            {
                CardBarHandler.instance.gameObject.SetActive(false);
                hasToggled = true;
            }
            if (autoHide.Value && CardBarHandler.instance != null && !GameManager.instance.battleOngoing && hasToggled)
            {
                CardBarHandler.instance.gameObject.SetActive(true);
                hasToggled = false;
            }
            
            if (detectKey)
            {
                var values = Enum.GetValues(typeof(KeyCode));
                foreach(KeyCode code in values){
                    if (Input.GetKeyDown(code))
                    {
                        detectedKey.Value = code;
                        detectKey = false;
                    }                    
                }

                if (!(button is null)) button.GetComponentInChildren<TextMeshProUGUI>().text = "Press any key";
                haveDetectedKey = false;
            }
            else if (!haveDetectedKey && !(keyText is null) && !(button is null))
            {
                keyText.text = "Toggle card bar keybind: " + Enum.GetName(typeof(KeyCode), detectedKey.Value);
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Set toggle card bar keybind";
                haveDetectedKey = true;
            }
        }

        private void UpdatePreview()
        {
            if (firstTime) return;
            this.ExecuteAfterFrames(1, () =>
            {
                if (CardBarHandler.instance != null) CardBarHandler.instance.ResetCardBards();
                for (int y = 0; y < previewTeams; y++)
                {
                    for (int i = 0; i < previewCards; i++)
                    {
                        var card = CardChoice.instance.cards[i];
                        if (card == null) card = CardChoice.instance.cards[0];
                        CardBarHandler.instance.AddCard(y,card);
                    }
                }
            });
        }

        private static void UpdateCardBar()
        {
            var cardBars = (CardBar[]) Traverse.Create(CardBarHandler.instance).Field("cardBars").GetValue();
            for (int i = 0; i < cardBars.Length; i++)
            {
                UpdateLocalCardBar(cardBars[i], i);
            }

            var orgCardBar = CardBarHandler.instance.transform.GetChild(0).GetComponent<CardBar>();
            UpdateLocalCardBar(orgCardBar, -1);
        }

        private static void UpdateLocalCardBar(CardBar cardBar, int index)
        {
            var layoutGroup = cardBar.gameObject.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.spacing = spacing.Value;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childControlHeight = false;
            var rectTransform = cardBar.gameObject.GetComponent<RectTransform>();
            var offset = -(horizontalOffset.Value * 10);
            
            rectTransform.offsetMin = new Vector2(offset, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(distanceFromRight.Value * -1, rectTransform.offsetMax.y);

            
            if (adjustedCardSize == 0)
            {
                for (var i = 0; i < cardBar.transform.childCount; i++)
                {
                    var rectTrans = cardBar.transform.GetChild(i).GetComponent<RectTransform>();
                    rectTrans.sizeDelta = new Vector2(cardSize.Value, cardSize.Value);
                }
            }

            var deltaY = -verticalDistance.Value;
            Transform transform1;
            var barGo = (transform1 = cardBar.transform).parent.transform.GetChild(0).gameObject;
            if (index == -1) index++;
            transform1.localPosition = barGo.transform.localPosition + new Vector3(0, deltaY * index, 0);

            cardBar.gameObject.GetOrAddComponent<CanvasGroup>().alpha = opacity.Value / 100f;

            UpdateEstimatedCards();
        }

        private static void UpdateEstimatedCards(bool useAdjusted = false)
        {
            var cardSized = useAdjusted ? adjustedCardSize : cardSize.Value;
            var offset = -(horizontalOffset.Value * 10);
            estimatedCards = (-offset - (-offset / cardSized * spacing.Value)) / cardSized;
            estimatedCards += spacing.Value*0.15f;
            estimatedCards = Mathf.Round(estimatedCards*100)/100;
            estimatedText.text = "Estimated cards that will be visible with these settings: " + estimatedCards;
        }

        public static void CardBar()
        {
            instance.ExecuteAfterSeconds(0.5f, UpdateCardBar);
        }
    }
}
