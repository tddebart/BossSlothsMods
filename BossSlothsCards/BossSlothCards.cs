using System.Linq;
using BepInEx;
using BossSlothsCards.Cards;
using HarmonyLib;
using Jotunn.Utils;
using Photon.Pun;
using UnboundLib.Cards;
using UnityEngine;
using UnboundLib;


namespace BossSlothsCards
{
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInDependency("pykess.rounds.plugins.playerjumppatch")]
    [BepInPlugin("MyCoolPlugin.ActualRounds.Yes", "BossSlothsCards", "0.1.3")]
    [BepInProcess("Rounds.exe")]
    public class BossSlothCards : BaseUnityPlugin
    {
        internal static AssetBundle ArtAsset;
        internal static AssetBundle EffectAsset;


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
            //var harmony = new Harmony("com.rounds.BSM.Startup.Harmony");
            //harmony.PatchAll();

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
            CustomCard.BuildCard<Knockback>();
            //CustomCard.BuildCard<OneShot>();
            //CustomCard.BuildCard<Nice>();

            levelAsset = AssetUtils.LoadAssetBundleFromResources("customlevel", typeof(BossSlothCards).Assembly);
            if (levelAsset == null)
            {
                UnityEngine.Debug.LogError("Couldn't find levelAsset?");
            }

            //Unbound.BuildLevel(levelAsset);

        }
    }
}