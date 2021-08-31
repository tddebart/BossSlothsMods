using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BossSlothsCards.Cards;
using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.TempEffects;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using Jotunn.Utils;
using Photon.Pun;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnboundLib.Utils;
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

        public GameObject betterSawObj;
        public GameObject betterDesBox;
        
        public static bool firstNoThanks = true;

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
            
            // Create bettersaw
            var saw = (GameObject)Resources.Load("4 map objects/MapObject_Saw_Stat");
            var betterSaw = Instantiate(saw);
            DestroyImmediate(betterSaw.GetComponent<PhotonMapObject>());
            DontDestroyOnLoad(betterSaw);
            betterSaw.GetComponent<DamageBox>().damage = 27;
            //DestroyImmediate(betterSaw.GetComponent<Collider2D>());
            //betterSaw.transform.Rotate(new Vector3(90,0 ));
            betterSaw.transform.localScale = Vector3.one;
            betterSaw.transform.position = new Vector3(1000, 0, 0);
            //PhotonNetwork.PrefabPool.RegisterPrefab("MapObject_Saw_Stat", betterSaw);
            betterSawObj = betterSaw;

            // //Create betterBox
            // var box = (GameObject) Resources.Load("4 map objects/Box_Destructible_Small");
            // var betterBox = Instantiate(box);
            // DestroyImmediate(betterBox.GetComponent<PhotonMapObject>());
            // betterBox.GetComponent<Rigidbody2D>().simulated = false;
            // DontDestroyOnLoad(betterBox);
            // betterBox.transform.localScale = Vector3.one;
            // betterBox.transform.position = new Vector3(1000, 0, 0);
            // betterDesBox = betterBox;
            
            // 6
            CustomCard.BuildCard<Sneeze>();
            CustomCard.BuildCard<YinYang>();
            CustomCard.BuildCard<DoubleJump>();
            CustomCard.BuildCard<Yin>();
            CustomCard.BuildCard<Yang>();
            CustomCard.BuildCard<Yeetus>();
            //CustomCard.BuildCard<NotToday>();
            
            // 4
            CustomCard.BuildCard<MomGetTheCamera>();
            CustomCard.BuildCard<RandomConfringo>();
            CustomCard.BuildCard<Larcenist>();
            CustomCard.BuildCard<CopyCat>();
            
            // 7
            CustomCard.BuildCard<NoThanks>();
            CustomCard.BuildCard<GiveMeAnother>();
            CustomCard.BuildCard<HitMeBabyOneMoreTime>();
            CustomCard.BuildCard<SnapEffect>();
            CustomCard.BuildCard<KnightsArmor>();
            CustomCard.BuildCard<KingsArmor>();
            CustomCard.BuildCard<Thorns>();
            
            // 40
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
            CustomCard.BuildCard<CompactedShot>();
            CustomCard.BuildCard<FoldableStock>();
            CustomCard.BuildCard<FuturisticStock>();
            CustomCard.BuildCard<RocketJump>();
            CustomCard.BuildCard<WoodenStock>();
            CustomCard.BuildCard<SplittingRounds>();
            CustomCard.BuildCard<HazmatSuit>();
            CustomCard.BuildCard<BoltAction>();
            CustomCard.BuildCard<SawbladeBullets>();
            CustomCard.BuildCard<SluggishRounds>();
            CustomCard.BuildCard<Quadratics>();
            CustomCard.BuildCard<SpinningDeath>();
            CustomCard.BuildCard<UnderDog>();
            CustomCard.BuildCard<RollingThunder>();
            CustomCard.BuildCard<Alpha>();
            CustomCard.BuildCard<Omega>();
            CustomCard.BuildCard<Attract>();
            CustomCard.BuildCard<Boing>();
            CustomCard.BuildCard<Repel>();
            CustomCard.BuildCard<SpreadTheLove>();
            CustomCard.BuildCard<SquiresArmor>();
            CustomCard.BuildCard<Pong>();
            CustomCard.BuildCard<FlatpackMunitions>();
            CustomCard.BuildCard<Whale>();
            CustomCard.BuildCard<SleightOfHand>();
            CustomCard.BuildCard<RecyclingDay>();
            //CustomCard.BuildCard<WreckingBall>();
            CustomCard.BuildCard<LongFallBoots>();
            CustomCard.BuildCard<BulletProofBullets>();
            CustomCard.BuildCard<OverclockedFlywheels>();
            CustomCard.BuildCard<FireHydrant>();


            GameModeManager.AddHook(GameModeHooks.HookPointStart, (gm) => DoExplosionThings());

            GameModeManager.AddHook(GameModeHooks.HookPointEnd, gm => IEStopAllCoroutines());
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, gm => DestroyAllRemoveOnRoundsEnds());
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, gm => ResetStats());
            
            this.ExecuteAfterSeconds(0.4f, () =>
            {
                CustomCardCategories.instance.MakeCardsExclusive(
                    CardManager.cards.Values.First(card => card.cardInfo.cardName == "Pong").cardInfo,
                    CardManager.cards.Values.First(card => card.cardInfo.cardName == "GROW").cardInfo);
                CustomCardCategories.instance.MakeCardsExclusive(
                    CardManager.cards.Values.First(card => card.cardInfo.cardName == "Pong").cardInfo,
                    CardManager.cards.Values.First(card => card.cardInfo.cardName == "Spinning death").cardInfo);
                
                if (CardManager.cards.Values.Any(card => card.cardInfo.cardName == "Comb"))
                {
                    CustomCardCategories.instance.MakeCardsExclusive(
                        CardManager.cards.Values.First(card => card.cardInfo.cardName == "Pong").cardInfo,
                        CardManager.cards.Values.First(card => card.cardInfo.cardName == "Comb").cardInfo);
                }

                if (CardManager.cards.Values.Any(card => card.cardInfo.cardName == "Star"))
                {
                    CustomCardCategories.instance.MakeCardsExclusive(
                        CardManager.cards.Values.First(card => card.cardInfo.cardName == "Pong").cardInfo,
                        CardManager.cards.Values.First(card => card.cardInfo.cardName == "Star").cardInfo);
                }
                
                if (CardManager.cards.Values.Any(card => card.cardInfo.cardName == "Crow"))
                {
                    CustomCardCategories.instance.MakeCardsExclusive(
                        CardManager.cards.Values.First(card => card.cardInfo.cardName == "Pong").cardInfo,
                        CardManager.cards.Values.First(card => card.cardInfo.cardName == "Crow").cardInfo);
                }
                
            });
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

        private IEnumerator DestroyAllRemoveOnRoundsEnds()
        {
            foreach (var obj in SceneManager.GetSceneAt(0).GetRootGameObjects())
            {
                if(obj.name == "REMOVE ME" || obj.GetComponentInChildren<RemoveOnRoundEnd>()) Destroy(obj);
            }

            yield break;
        }

        private IEnumerator ResetStats()
        {
            // foreach (var player in PlayerManager.instance.players)
            // {
            //     if (player.GetComponent<SawBladeEffect>())
            //     {
            //         
            //     }
            // }
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
