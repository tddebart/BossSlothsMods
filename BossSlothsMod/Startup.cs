using System.Linq;
using BepInEx;
using BossSlothsMod.Cards;
using HarmonyLib;
using Jotunn.Utils;
using Photon.Pun;
using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;


namespace BossSlothsMod
{
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInPlugin("MyCoolPlugin.ActualRounds.Yes", "BossSlothsMod", "0.0.5")]
    [BepInProcess("Rounds.exe")]
    public class Startup : BaseUnityPlugin
    {
        internal static AssetBundle ArtAsset;
        internal static AssetBundle EffectAsset;

        public static GameObject Instance;
        
        
        internal static AssetBundle levelAsset;

#if DEBUG
        private Vector2 scrollposision;
        private readonly GUILayoutOption[] gl = new GUILayoutOption[0];

        private void OnGUI()
        {
            var area = new Rect(50, 50, 2000, 2000);
            GUILayout.BeginArea(area);
            scrollposision = GUILayout.BeginScrollView(scrollposision, GUILayout.MaxWidth(200), GUILayout.MaxHeight(1000));

            if(CardChoice.instance != null && CardChoice.instance.cards != null)
            {
                GUILayout.Label("cards: ", gl);
                var cardArray = CardChoice.instance.cards;
                foreach (var info in cardArray)
                {
                    GUILayout.Label(info.cardName);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        #endif
        
        private void Start()
        {
            Instance = this.gameObject;
            
            var harmony = new Harmony("com.rounds.BSM.Startup.Harmony");
            harmony.PatchAll();

            ArtAsset = AssetUtils.LoadAssetBundleFromResources("bossslothsart", typeof(Startup).Assembly);
            if (ArtAsset == null)
            {
                UnityEngine.Debug.LogError("Couldn't find ArtAsset?");
            }
            EffectAsset = AssetUtils.LoadAssetBundleFromResources("bossslothseffects", typeof(Startup).Assembly);
            if (EffectAsset == null)
            {
                UnityEngine.Debug.LogError("Couldn't find EffectAsset?");
            }

            CustomCard.BuildCard<Sneeze>();
            CustomCard.BuildCard<YinYang>();
            CustomCard.BuildCard<DoubleJump>();
            CustomCard.BuildCard<Yin>();
            CustomCard.BuildCard<Yang>();
            CustomCard.BuildCard<Knockback>();
            //CustomCard.BuildCard<OneShot>();
            //CustomCard.BuildCard<Nice>();
            
            levelAsset = AssetUtils.LoadAssetBundleFromResources("customlevel", typeof(Startup).Assembly);
            if (levelAsset == null)
            {
                UnityEngine.Debug.LogError("Couldn't find levelAsset?");
            }

            Unbound.BuildLevel(levelAsset);
            
            //Balancing cards
            foreach (var info in CardChoice.instance.cards)
            {
                switch (info.cardName)
                {
                    case "PHOENIX":
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
                        break;
                    }
                    case "GROW":
                    {
                        info.allowMultiple = false;
                        break;
                    }
                    case "EMP":
                    {
                        info.allowMultiple = false;
                        break;
                    }
                    case "SCAVENGER":
                    {
                        info.allowMultiple = false;
                        break;
                    }
                    case "SAW":
                    {
                        info.allowMultiple = false;
                        var saw = info.gameObject.GetComponent<CharacterStatModifiers>().AddObjectToPlayer.GetComponent<SpawnObjects>().objectToSpawn[0].GetComponent<Saw>();
                        saw.range = 4;
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

        private void Update()
        {
            
            if (GameManager.instance.isPlaying || PhotonNetwork.OfflineMode)
            {
                foreach (var info in CardChoice.instance.cards.ToList().Where(info => info.cardName == "BUCKSHOT"))
                {
                    var _cardArray = CardChoice.instance.cards.ToList();
                    _cardArray.Remove(info);
                    CardChoice.instance.cards = _cardArray.ToArray();
                }
            }
        }
    }
}