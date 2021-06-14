using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BossSlothsMod.Cards;
using HarmonyLib;
using HarmonyLib.Tools;
using Jotunn.Utils;
using Photon.Pun;
using PickTwoPlugin;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;


namespace BossSlothsMod
{
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInPlugin("MyCoolPlugin.ActualRounds.Yes", "BossSlothsMod", "0.0.0.3")]
    [BepInProcess("Rounds.exe")]
    public class Startup : BaseUnityPlugin
    {
        internal static AssetBundle ArtAsset;
        internal static AssetBundle EffectAsset;

        public static GameObject Instance;

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

            ArtAsset = AssetUtils.LoadAssetBundleFromResources("actualart", typeof(Startup).Assembly);
            if (ArtAsset == null)
            {
                Debug.LogError("Couldn't find ArtAsset?");
            }
            EffectAsset = AssetUtils.LoadAssetBundleFromResources("effects", typeof(Startup).Assembly);

            CustomCard.BuildCard<Sneeze>();
            CustomCard.BuildCard<YinYang>();
            CustomCard.BuildCard<InfJump>();
            CustomCard.BuildCard<Yin>();
            CustomCard.BuildCard<Yang>();
            CustomCard.BuildCard<Knockback>();

            //CustomCard.BuildCard<OneShot>();
            //CustomCard.BuildCard<Nice>();
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
                }

            }
        }

        public void Message(string message)
        {
            Logger.LogWarning(message);
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