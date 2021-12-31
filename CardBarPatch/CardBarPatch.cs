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
        private const string ModName = "Card Bar Patch";
        private static string CompatibilityModName => CardBarPatch.ModName.Replace(" ", "");
        
        public static CardBarPatch instance;

        private static List<Carderbar> cardBars = new List<Carderbar>();
        public static KeyCode DetectedKey
        {
            get
            {
                return (KeyCode)PlayerPrefs.GetInt(GetConfigKey("keycode"), (int)KeyCode.P);
            }
            set
            {
                PlayerPrefs.SetInt(GetConfigKey("keycode"), (int)value);
            }
        }
        public static float Spacing
        {
            get
            {
                return PlayerPrefs.GetFloat(GetConfigKey("spacing"), 3f);
            }
            set
            {
                PlayerPrefs.SetFloat(GetConfigKey("spacing"), value);
            }
        }
        public static float DistanceFromRight
        {
            get
            {
                return PlayerPrefs.GetFloat(GetConfigKey("distanceFromRight"), 35f);
            }
            set
            {
                PlayerPrefs.SetFloat(GetConfigKey("distanceFromRight"), value);
            }
        }
        public static float HorizontalOffset
        {
            get
            {
                return PlayerPrefs.GetFloat(GetConfigKey("horizontalOffset"), 145f);
            }
            set
            {
                PlayerPrefs.SetFloat(GetConfigKey("horizontalOffset"), value);
            }
        }
        public static float CardSize
        {
            get
            {
                return PlayerPrefs.GetFloat(GetConfigKey("cardSize"), 40f);
            }
            set
            {
                PlayerPrefs.SetFloat(GetConfigKey("cardSize"), value);
            }
        }
        public static float VerticalDistance
        {
            get
            {
                return PlayerPrefs.GetFloat(GetConfigKey("verticalDistance"), 50f);
            }
            set
            {
                PlayerPrefs.SetFloat(GetConfigKey("verticalDistance"), value);
            }
        }
        public static bool AutoHide
        {
            get
            {
                return PlayerPrefs.GetInt(GetConfigKey("autoHide"), 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(GetConfigKey("autoHide"), value ? 1 : 0);
            }
        }
        /*
        public static bool AutoScale
        {
            get
            {
                return PlayerPrefs.GetInt(GetConfigKey("AutoScale"), 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(GetConfigKey("AutoScale"), value ? 1 : 0);
            }
        }
        */
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
        static string GetConfigKey(string key) => $"{CardBarPatch.CompatibilityModName}_{key}";

        private void Awake()
        {   

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
            
            Unbound.RegisterMenu(ModName, () =>
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

            MenuHandler.CreateSlider("Spacing", menu, 40, 0, 10, Spacing, (val) =>
                {
                    Spacing = val;
                    UpdateCardBar();
                },
                out var spacingSlider, true);
            
            MenuHandler.CreateSlider("Right side dist(Def: 35)", menu, 40, 0, 100, DistanceFromRight, (val) =>
            {
                DistanceFromRight = val;
                UpdateCardBar();
            }, out var rightSideSlider, true);
            
            MenuHandler.CreateSlider("Horizontal offset(Def: 145)", menu, 40, 20, 190, HorizontalOffset, (val) =>
                {
                    HorizontalOffset = val;
                    UpdateCardBar();
                },
                out var horizontalSlider, true);
            
            MenuHandler.CreateSlider("Card size(Def: 40)", menu, 40, 20, 50, CardSize, (val) =>
                {
                    CardSize = val;
                    UpdateCardBar();
                },
                out var cardSizeSlider, true);
            
            MenuHandler.CreateSlider("Vertical dist(Def: 50)", menu, 40, 20, 60, VerticalDistance, (val) =>
                {
                    VerticalDistance = val;
                    UpdateCardBar();
                },
                out var verticalSlider, true);
            MenuHandler.CreateSlider( "Opacity(Def: 100)", menu, 40, 0, 100, opacity.Value, (val) =>
                {
                    opacity.Value = val;
                    UpdateCardBar();
                },
                out var opacitySlider, true);
            var toggle = MenuHandler.CreateToggle(AutoHide, "Auto hide during battle", menu,
                arg0 => { AutoHide = arg0;}, 40);
            MenuHandler.CreateButton("Reset all", menu, () =>
            {
                Spacing = 3;
                DistanceFromRight = 35;
                HorizontalOffset = 145;
                CardSize = 40;
                VerticalDistance = 50;
                AutoHide = false;

                spacingSlider.value = Spacing;
                rightSideSlider.value = DistanceFromRight;
                horizontalSlider.value = HorizontalOffset;
                cardSizeSlider.value = CardSize;
                verticalSlider.value = VerticalDistance;
                toggle.GetComponentInChildren<Toggle>().isOn = AutoHide;
            }, 40);

            // MenuHandler.CreateToggle(AutoScale, "Automatic scaling", menu, arg0 =>
            // {
            //     AutoScale = arg0;
            // }, 40);

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
            if (Input.GetKeyDown(DetectedKey) && CardBarHandler.instance != null && !GameManager.lockInput)
            {
                CardBarHandler.instance.gameObject.SetActive(!CardBarHandler.instance.gameObject.activeInHierarchy);
            }

            if (AutoHide && CardBarHandler.instance != null && GameManager.instance.battleOngoing && !hasToggled)
            {
                CardBarHandler.instance.gameObject.SetActive(false);
                hasToggled = true;
            }
            if (AutoHide && CardBarHandler.instance != null && !GameManager.instance.battleOngoing && hasToggled)
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
                        DetectedKey = code;
                        detectKey = false;
                    }                    
                }

                if (!(button is null)) button.GetComponentInChildren<TextMeshProUGUI>().text = "Press any key";
                haveDetectedKey = false;
            }
            else if (!haveDetectedKey && !(keyText is null) && !(button is null))
            {
                keyText.text = "Toggle card bar keybind: " + Enum.GetName(typeof(KeyCode), DetectedKey);
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
            layoutGroup.spacing = Spacing;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childControlHeight = false;
            var rectTransform = cardBar.gameObject.GetComponent<RectTransform>();
            var offset = -(HorizontalOffset * 10);
            rectTransform.offsetMin = new Vector2(offset, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(DistanceFromRight * -1, rectTransform.offsetMax.y);

            if (adjustedCardSize == 0)
            {
                for (var i = 0; i < cardBar.transform.childCount; i++)
                {
                    var rectTrans = cardBar.transform.GetChild(i).GetComponent<RectTransform>();
                    rectTrans.sizeDelta = new Vector2(CardSize, CardSize);
                }
            }

            if (cardBars.All(bar => bar.cardBar != cardBar) && index != -1)
            {
                cardBars.Add(new Carderbar(cardBar, CardSize, -VerticalDistance));
            }

            var deltaY = -VerticalDistance;

            Transform transform1;
            var barGo = (transform1 = cardBar.transform).parent.transform.GetChild(0).gameObject;
            if (index == -1) index++;
            transform1.localPosition = barGo.transform.localPosition + new Vector3(0, deltaY * index, 0);

            cardBar.gameObject.GetOrAddComponent<CanvasGroup>().alpha = opacity.Value / 100f;

            UpdateEstimatedCards();
        }

        private static void UpdateEstimatedCards(bool useAdjusted = false)
        {
            var cardSized = useAdjusted ? adjustedCardSize : CardSize;
            var offset = -(HorizontalOffset * 10);
            estimatedCards = (-offset - (-offset / cardSized * Spacing)) / cardSized;
            estimatedCards += Spacing*0.15f;
            estimatedCards = Mathf.Round(estimatedCards*100)/100;
            estimatedText.text = "Estimated cards that will be visible with these settings: " + estimatedCards;
        }

        public static void CardBar()
        {
            instance.ExecuteAfterSeconds(0.5f, UpdateCardBar);
        }
    }
}
