using BepInEx;
using BossSlothsCards.Cards;
using Jotunn.Utils;
using UnboundLib.Cards;
using UnityEngine;


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
        public const string Version = "0.1.4";
        
        internal static AssetBundle ArtAsset;
        internal static AssetBundle EffectAsset;

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


        }
    }
}