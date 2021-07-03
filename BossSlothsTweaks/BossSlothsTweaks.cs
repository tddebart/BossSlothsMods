using System;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Photon.Pun;
using UnboundLib;
using UnityEngine;

namespace BossSlothsTweaks
{
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class BossSlothsTweaks : BaseUnityPlugin
    {
        
        private const string ModId = "com.BossSloth.Rounds.Tweaks";
        private const string ModName = "BossSlothsTweaks";
        public const string Version = "0.1.3";

        private static ConfigEntry<bool> PHOENIX;
        private static ConfigEntry<bool> GROW;
        private static ConfigEntry<bool> SCAVENGER;
        private static ConfigEntry<bool> SAW;
        private static ConfigEntry<bool> TACTICALSHIELDUP;

        //private static BossSlothsTweaks Instance;

        private static CardCategory[] tacticalReload;
        private static CardCategory[] shieldsUp;

        private void Start()
        {
            Unbound.RegisterGUI("BossSlothTweaks", DrawGUI);
            Unbound.RegisterHandshake("com.willis.rounds.unbound", OnHandShakeCompleted);

            //Instance = this;
            
            PHOENIX = Config.Bind("Cards", "Phoenix", false, "Added -50% damage, Reduced health to -50% (from -35%)");
            GROW = Config.Bind("Cards", "Grow", false, "Only one per game");
            SCAVENGER = Config.Bind("Cards", "Scavenger", false, "Only one per game");
            SAW = Config.Bind("Cards", "Saw", false, "Reduced range to 4 (from 4.5), Only one per game");
            TACTICALSHIELDUP = Config.Bind("Cards", "Tactical reload + Shields Up", false, "makes it so you can't get Shields up when you have Tactical reload and vice versa");


            var harmony = new Harmony("com.BossSloth.Rounds.Tweaks.Harmony");
            harmony.PatchAll();

            var catergoryTactical = ScriptableObject.CreateInstance<CardCategory>();
            catergoryTactical.name = "TACTICAl RELOAD";
            
            tacticalReload = new[]
            {
                catergoryTactical
            };

            var catergoryShields = ScriptableObject.CreateInstance<CardCategory>();
            catergoryShields.name = "TACTICAl RELOAD";
            
            shieldsUp = new[]
            {
                catergoryShields
            };
            ChangeCards();
        }

        private static void ChangeCards()
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
                        if (SAW.Value)
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
                    case "TACTICAL RELOAD":
                    {
                        if (TACTICALSHIELDUP.Value)
                        {
                            info.categories = tacticalReload;
                            info.blacklistedCategories = shieldsUp;
                        }
                        else
                        {
                            info.categories = Array.Empty<CardCategory>();
                            info.blacklistedCategories = Array.Empty<CardCategory>();
                        }
                        break;
                    }
                    case "SHIELDS UP":
                    {
                        if (TACTICALSHIELDUP.Value)
                        {
                            info.categories = shieldsUp;
                            info.blacklistedCategories = tacticalReload;
                        }
                        else
                        {
                            info.categories = Array.Empty<CardCategory>();
                            info.blacklistedCategories = Array.Empty<CardCategory>();
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
        }

        private void DrawGUI()
        {
            bool flag1 = GUILayout.Toggle(PHOENIX.Value, "Phoenix", Array.Empty<GUILayoutOption>());
            bool flag2 = GUILayout.Toggle(GROW.Value, "Grow", Array.Empty<GUILayoutOption>());
            bool flag4 = GUILayout.Toggle(SCAVENGER.Value, "Scavenger", Array.Empty<GUILayoutOption>());
            bool flag5 = GUILayout.Toggle(SAW.Value, "Saw", Array.Empty<GUILayoutOption>());
            bool flag6 = GUILayout.Toggle(TACTICALSHIELDUP.Value, "TacticalReload + ShieldsUp combo", Array.Empty<GUILayoutOption>());
            if (flag1 != PHOENIX.Value || flag2 != GROW.Value || flag4 != SCAVENGER.Value || flag5 != SAW.Value || flag6 != TACTICALSHIELDUP.Value)
            {
                NetworkingManager.RaiseEvent("com.BossSloth.Rounds.Tweaks_SyncTweaks", new object[]
                {
                    flag1,
                    flag2,
                    flag4,
                    flag5,
                    flag6
                });
                PHOENIX.Value = flag1;
                GROW.Value = flag2;
                SCAVENGER.Value = flag4;
                SAW.Value = flag5;
                TACTICALSHIELDUP.Value = flag6;
                ChangeCards();
                return;
            }
            PHOENIX.Value = flag1;
            GROW.Value = flag2;
            SCAVENGER.Value = flag4;
            SAW.Value = flag5;
            TACTICALSHIELDUP.Value = flag6;
        }
        
        private void Awake()
        {
            NetworkingManager.RegisterEvent("com.BossSloth.Rounds.Tweaks_SyncTweaks", delegate(object[] e)
            {
                PHOENIX.Value = (bool)e[0];
                GROW.Value = (bool)e[1];
                SCAVENGER.Value = (bool)e[2];
                SAW.Value = (bool)e[3];
                TACTICALSHIELDUP.Value = (bool)e[4];
            });
        }
        
        private static void OnHandShakeCompleted()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RaiseEvent("com.BossSloth.Rounds.Tweaks_SyncTweaks", new object[]
                {
                    PHOENIX.Value,
                    GROW.Value,
                    SCAVENGER.Value,
                    SAW.Value,
                    TACTICALSHIELDUP.Value
                })
                ;
            }
        }
    }
}