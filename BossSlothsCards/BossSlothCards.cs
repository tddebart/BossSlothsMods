using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BossSlothsCards.Cards;
using BossSlothsCards.Extensions;
using HarmonyLib;
using Jotunn.Utils;
using ModdingUtils.Utils;
using Photon.Pun;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BossSlothsCards
{
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
    [BepInDependency("pykess.rounds.plugins.moddingutils")]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class BossSlothCards : BaseUnityPlugin
    {
        private const string ModId = "com.bosssloth.rounds.BSM";
        private const string ModName = "BossSlothsCards";
        public const string Version = "2.0.0";
        
        internal static AssetBundle ArtAsset;
        internal static AssetBundle EffectAsset;

        internal static BossSlothCards instance;

        private void Start()
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();

            instance = this;
            
            Unbound.RegisterCredits("Boss Sloths Cards (BSC)", new[] {"Boss sloth Inc.", " ","Special thanks to: ","Pykess for some Card frameworks"}, new[] {"Github", "Buy me a coffee"}, new[] {"https://github.com/tddebart/BossSlothsMods", "https://www.buymeacoffee.com/BossSloth"});

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

            CustomCard.BuildCard<GetOverHere>();
            CustomCard.BuildCard<Sloth>();
            CustomCard.BuildCard<Eagle>();
            CustomCard.BuildCard<HigherCaliber>();
            CustomCard.BuildCard<ThisWayUp>();
            CustomCard.BuildCard<DrumMagazine>();
            CustomCard.BuildCard<EliteSneakers>();
            CustomCard.BuildCard<LeMonk>();
            CustomCard.BuildCard<MorningCoffee>();
            CustomCard.BuildCard<SecondGun>();
            
            

            GameModeManager.AddHook(GameModeHooks.HookPointStart, (gm) => DoExplosionThings());
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, gm => IEStopAllCoroutines());
        }

        private IEnumerator DoExplosionThings()
        {
            if (this == null) yield break;
            Wait5SecondsAndDoSomething();
        }

        private IEnumerator IEStopAllCoroutines()
        {
            StopAllCoroutines();
            yield break;
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

                            var loops = 0;
                            while (true)
                            {
                                var rng = new System.Random();
                                var rID = rng.Next(0, objects.Count-1);
                                if (objects[rID] == null) continue;
                                if (Condition(objects[rID].gameObject))
                                {
                                    if (player.GetComponent<PhotonView>().IsMine)
                                    {
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
                    else if (player.data.currentCards[i].cardName == "Snap effect")
                    {
                        this.ExecuteAfterSeconds(i * 0.1f + player.playerID * 0.2f , () =>
                        {
                            StopCoroutine(SnapEffect(player));
                            if (this == null) return;
                            StartCoroutine(SnapEffect(player));
                        });
                    }
                }
            }
        }

        private IEnumerator SnapEffect(Player player)
        {
            yield return new WaitForSeconds(7);
            var scene = SceneManager.GetSceneAt(1);
            if (!scene.IsValid())  yield break;
            var objectsArray = scene.GetRootGameObjects()[0].GetComponentsInChildren<Collider2D>(false);
            if (objectsArray == null)  yield break;
            var objects = new List<Collider2D>();
            foreach (var obj in objectsArray)
            {
                if (Condition(obj.gameObject))
                {
                    objects.Add(obj);
                }
            }

            var loops = 0;
            while (true)
            {
                var rng = new System.Random();
                var rID = rng.Next(0, objects.Count-1);
                if (objects[rID] == null) continue;
                if (Condition(objects[rID].gameObject))
                {
                    if (player.GetComponent<PhotonView>().IsMine)
                    {
                        player.GetComponent<PhotonView>().RPC("RPCA_ExplodeBlock", RpcTarget.All, rID);
                    }

                    StartCoroutine(SnapEffect(player));
                    break;
                }

                loops++;
                if (loops >= 100)
                {
                    UnityEngine.Debug.LogError("Couldn't find object in 100 iterations");
                    yield break;
                }
            }
        }

        private static bool Condition(GameObject obj)
        {
            return obj.activeInHierarchy &&obj.GetComponent<Collider2D>() && !obj.name.Contains("Color") && !obj.name.Contains("Lines") && !obj.name.Contains("Grid");
        }
    }
}
