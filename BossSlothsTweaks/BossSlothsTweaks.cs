using System;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Photon.Pun;
using UnboundLib;
using UnityEngine;
using UnityEngine.UI;

namespace BossSlothsTweaks
{
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInPlugin("com.BossSloth.Rounds.Tweaks", "BossSlothsTweaks", "0.1.0")]
    [BepInProcess("Rounds.exe")]
    public class BossSlothsTweaks : BaseUnityPlugin
    {

        private static ConfigEntry<bool> PHOENIX;
        private static ConfigEntry<bool> GROW;
        private static ConfigEntry<bool> EMP;
        private static ConfigEntry<bool> SCAVENGER;
        private static ConfigEntry<bool> SAW;
        internal static ConfigEntry<bool> ExtendedCards;
        
        internal static BossSlothsTweaks Instance;

        private void Start()
        {
            Unbound.RegisterGUI("BossSlothTweaks", DrawGUI);
            Unbound.RegisterHandshake("com.willis.rounds.unbound", OnHandShakeCompleted);

            Instance = this;
            
            PHOENIX = Config.Bind("Cards", "Phoenix", false, "Added -50% damage, Reduced health to -50% (from -35%)");
            GROW = Config.Bind("Cards", "Grow", false, "Only one per game");
            EMP = Config.Bind("Cards", "Emp", false, "Only one per game");
            SCAVENGER = Config.Bind("Cards", "Scavenger", false, "Only one per game");
            SAW = Config.Bind("Cards", "Saw", false, "Reduced range to 4 (from 4.5), Only one per game");
            ExtendedCards = Config.Bind("Gameplay", "More cards top right", true, "The card bar in the top right will now show a max of 34 cards (from 11)");
            
            ChangeCards();

            var harmony = new Harmony("com.BossSloth.Rounds.Tweaks.Harmony");
            harmony.PatchAll();
        }

        public static void ChangeCards()
        {
            //Balancing cards
            foreach (var info in CardChoice.instance.cards)
            {
                switch (info.cardName)
                {
                    case "PHOENIX":
                    {
                        if (PHOENIX.Value)
                        {
                            var infoList = info.cardStats.ToList();
                            if (infoList.Count > 2)
                            {
                                break;
                            }
                            var damage = new CardInfoStat {stat = "Damage", amount = "-50%", positive = false};
                            infoList.Add(damage);
                            infoList[0].amount = "-50%";
                            info.cardStats = infoList.ToArray();
                        
                            var gun = info.GetComponent<Gun>();
                            gun.damage = 0.5f;
                            var charstat = info.GetComponent<CharacterStatModifiers>();
                            charstat.health = 0.5f;
                        }
                        else
                        {
                            info.cardStats = new[]
                            {
                                new CardInfoStat {stat = "Health", amount = "-35%", positive = false}
                            };
                            info.GetComponent<Gun>().damage = 1;
                            info.GetComponent<CharacterStatModifiers>().health = 0.65f;
                            
                        }
                        break;
                    }
                    case "GROW":
                    {
                        if (GROW.Value)
                        {
                            info.allowMultiple = false;
                        }
                        else
                        {
                            info.allowMultiple = true;
                        }
                        break;
                    }
                    case "EMP":
                    {
                        if (EMP.Value)
                        {
                            info.allowMultiple = false;
                        }
                        else
                        {
                            info.allowMultiple = true;
                        }
                        break;
                    }
                    case "SCAVENGER":
                    {
                        if (SCAVENGER.Value)
                        {
                            info.allowMultiple = false;
                        }
                        else
                        {
                            info.allowMultiple = true;
                        }
                        break;
                    }
                    case "SAW":
                    {
                        if (!SAW.Value)
                        {
                            info.allowMultiple = false;
                            var saw = info.gameObject.GetComponent<CharacterStatModifiers>().AddObjectToPlayer.GetComponent<SpawnObjects>().objectToSpawn[0].GetComponent<Saw>();
                            saw.range = 4;
                        }
                        else
                        {
                            info.allowMultiple = true;
                            info.gameObject.GetComponent<CharacterStatModifiers>().AddObjectToPlayer.GetComponent<SpawnObjects>().objectToSpawn[0].GetComponent<Saw>().range = 4.5f;
                        }
                        break;
                    }
                    // case "SUPERNOVA":
                    // {
                    //     info.allowMultiple = false;
                    //     var nova = info.gameObject.GetComponent<CharacterStatModifiers>().AddObjectToPlayer.GetComponent<SpawnObjects>().objectToSpawn[0].GetComponent<SpawnObjects>().objectToSpawn[0].GetComponents<Explosion>();
                    //     nova[1].damage = 25;
                    //     break;
                    // }
                }
                
            }
            
            Instance.ExecuteAfterSeconds(0.1f, CardBar);
        }
        
        private static void CardBar()
        {
            if (ExtendedCards.Value)
            {
                foreach (var cardBar in (CardBar[]) Traverse.Create(CardBarHandler.instance).Field("cardBars").GetValue()) 
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
            else
            {
                foreach (var cardBar in (CardBar[]) Traverse.Create(CardBarHandler.instance).Field("cardBars").GetValue()) 
                {
                    var layoutGroup = cardBar.gameObject.GetComponent<HorizontalLayoutGroup>();
                    layoutGroup.spacing = 6.71f;
                    var rectTransform = cardBar.gameObject.GetComponent<RectTransform>();
                    switch (rectTransform.name)
                    {
                        case "Bar1":
                            rectTransform.offsetMin = new Vector2(-513.1996f, -69);
                            break;
                        case "Bar2":
                        case "Bar3":
                        case "Bar4":
                            rectTransform.offsetMin = new Vector2(-513.1996f, -122.1006f);
                            break;
                    }
                }
            }
        }

        private void DrawGUI()
        {
            bool flag1 = GUILayout.Toggle(PHOENIX.Value, "Phoenix", Array.Empty<GUILayoutOption>());
            bool flag2 = GUILayout.Toggle(GROW.Value, "Grow", Array.Empty<GUILayoutOption>());
            bool flag3 = GUILayout.Toggle(EMP.Value, "Emp", Array.Empty<GUILayoutOption>());
            bool flag4 = GUILayout.Toggle(SCAVENGER.Value, "Scavenger", Array.Empty<GUILayoutOption>());
            bool flag5 = GUILayout.Toggle(SAW.Value, "Saw", Array.Empty<GUILayoutOption>());
            bool flag6 = GUILayout.Toggle(ExtendedCards.Value, "More cards display top right", Array.Empty<GUILayoutOption>());
            if (flag1 != PHOENIX.Value || flag2 != GROW.Value || flag3 != EMP.Value || flag4 != SCAVENGER.Value || flag5 != SAW.Value || flag6 != ExtendedCards.Value)
            {
                NetworkingManager.RaiseEvent("com.BossSloth.Rounds.Tweaks_SyncTweaks", new object[]
                {
                    flag1,
                    flag2,
                    flag3,
                    flag4,
                    flag5,
                    flag6
                });
                ChangeCards();
            }

            PHOENIX.Value = flag1;
            GROW.Value = flag2;
            EMP.Value = flag3;
            SCAVENGER.Value = flag4;
            SAW.Value = flag5;
            ExtendedCards.Value = flag6;
        }
        
        private void Awake()
        {
            NetworkingManager.RegisterEvent("com.BossSloth.Rounds.Tweaks_SyncTweaks", delegate(object[] e)
            {
                PHOENIX.Value = (bool)e[0];
                GROW.Value = (bool)e[1];
                EMP.Value = (bool)e[2];
                SCAVENGER.Value = (bool)e[3];
                SAW.Value = (bool)e[4];
            });
        }
        
        private void OnHandShakeCompleted()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RaiseEvent("com.BossSloth.Rounds.Tweaks_SyncTweaks", new object[]
                {
                    PHOENIX.Value,
                    GROW.Value,
                    EMP.Value,
                    SCAVENGER.Value,
                    SAW.Value,
                    ExtendedCards.Value
                })
                ;
            }
        }
    }
}