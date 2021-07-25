using System.Collections;
using System.Collections.Generic;
using BepInEx;
using BossSlothsCards.Cards;
using BossSlothsCards.Extensions;
using HarmonyLib;
using Jotunn.Utils;
using Photon.Pun;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BossSlothsCards
{
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInDependency("pykess.rounds.plugins.playerjumppatch")]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class BossSlothCards : BaseUnityPlugin
    {
        
        private const string ModId = "com.bosssloth.rounds.BSM";
        private const string ModName = "BossSlothsCards";
        public const string Version = "1.1.0";
        
        internal static AssetBundle ArtAsset;
        internal static AssetBundle EffectAsset;

        internal static BossSlothCards instance;

        private void Start()
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();

            instance = this;
            
            Unbound.RegisterCredits("Boss Sloths Cards (BSC)", new[] {"Boss sloth Inc.", " ","Special thanks to: ","Pykess for Card frameworks"}, "Github", "https://github.com/tddebart/BossSlothsMods");

            ArtAsset = AssetUtils.LoadAssetBundleFromResources("bossslothsart", typeof(BossSlothCards).Assembly);
            if (ArtAsset == null)
            {
                UnityEngine.Debug.LogError("Couldn't find ArtAsset?");
            }

            EffectAsset = AssetUtils.LoadAssetBundleFromResources("bossslothseffects", typeof(BossSlothCards).Assembly);
            if (EffectAsset == null)
            {
                UnityEngine.Debug.LogError("Couldn't find EffectAsset?");
            }

            CustomCard.BuildCard<Sneeze>();
            CustomCard.BuildCard<YinYang>();
            CustomCard.BuildCard<DoubleJump>();
            CustomCard.BuildCard<Yin>();
            CustomCard.BuildCard<Yang>();
            CustomCard.BuildCard<Yeetus>();
            //CustomCard.BuildCard<NotToday>();
            
            CustomCard.BuildCard<MomGetTheCamera>();
            CustomCard.BuildCard<RandomConfringo>();
            CustomCard.BuildCard<Larcenist>();
            CustomCard.BuildCard<CopyCat>();
            
            CustomCard.BuildCard<NoThanks>();
            CustomCard.BuildCard<GiveMeAnother>();
            CustomCard.BuildCard<HitMeBabyOneMoreTime>();
            CustomCard.BuildCard<SnapEffect>();
            CustomCard.BuildCard<KnightsArmor>();
            CustomCard.BuildCard<KingsArmor>();
            CustomCard.BuildCard<Thorns>();
            


            //CustomCard.BuildCard<OneShot>();
            //CustomCard.BuildCard<Nice>();

            GameModeManager.AddHook(GameModeHooks.HookPickEnd, (gm) => Utils.CardBarUtils.instance.EndPickPhaseShow());
            GameModeManager.AddHook(GameModeHooks.HookPointStart, (gm) => DoExplosionThings());
            
            // Patch some cards from PCE
            this.ExecuteAfterSeconds(2, () =>
            {
                foreach (var info in CardChoice.instance.cards)
                {
                    if (info.cardName.Contains("Gamble") || info.cardName.Contains("Jackpot"))
                    {
                        info.GetAdditionalData().canBeReassigned = false;
                    }
                }
                
            });
        }
        private IEnumerator DoExplosionThings()
        {
            if (this == null) yield break;
            Wait5SecondsAndDoSomething();
        }

        private void Wait5SecondsAndDoSomething()
        {
            foreach(var player in PlayerManager.instance.players)
            {
                for (int i = 0; i < player.data.currentCards.Count; i++)
                {
                    if (player.data.currentCards[i].cardName == "Random confringo")
                    {
                        this.ExecuteAfterSeconds(5+(i*0.4f)+(player.playerID*0.6f), () =>
                        {
                            var scene = SceneManager.GetSceneAt(1);
                            if (!scene.IsValid())  return;
                            var objectsArray = scene.GetRootGameObjects()[0].GetComponentsInChildren<Collider2D>(false);
                            if (objectsArray == null)  return;
                            var objects = new List<Collider2D>();
                            foreach (var obj in objectsArray)
                            {
                                if (Condition(obj.gameObject))
                                {
                                    objects.Add(obj);
                                }
                            }

                            UnityEngine.Debug.LogWarning(objects.Count-1);

                            var loops = 0;
                            while (true)
                            {
                                var rng = new System.Random();
                                var rID = rng.Next(0, objects.Count-1);
                                if (objects[rID] == null) continue;
                                if (Condition(objects[rID].gameObject))
                                {
                                    UnityEngine.Debug.LogWarning("checking if photon is mine");
                                    if (player.GetComponent<PhotonView>().IsMine)
                                    {
                                        UnityEngine.Debug.LogWarning("photon was mine exc");
                                        player.GetComponent<PhotonView>().RPC("RPCA_ExplodeBlock", RpcTarget.All, rID);
                                    }
                                    break;
                                }
                
                                loops++;
                                if (loops >= 100)
                                {
                                    UnityEngine.Debug.LogError("Couldn't find object in 100 iterations");
                                    return;
                                }
                            }
                        });
                    }
                }
            }
        }

        private static bool Condition(GameObject obj)
        {
            return obj.activeInHierarchy &&obj.GetComponent<Collider2D>() && !obj.name.Contains("Color") && !obj.name.Contains("Lines") && !obj.name.Contains("Grid");
        }
    }
}
