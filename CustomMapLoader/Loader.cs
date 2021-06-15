using System.Collections.Generic;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using Jotunn.Utils;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace CustomMapLoader
{
    [BepInPlugin("com.net.customMap.BSM", "BossSlothsMaps", "0.0.0.3")]
    [BepInProcess("Rounds.exe")]
    public class Loader : BaseUnityPlugin
    {
        internal static AssetBundle levelAsset;
        private string[] scenePaths;
        private List<string> levelList = new List<string>();
        

        private void Start()
        {
            var harmony = new Harmony("com.rounds.BSM.Loader.Harmony");
            harmony.PatchAll();
            
            levelAsset = AssetUtils.LoadAssetBundleFromResources("customlevel", typeof(Loader).Assembly);
            if (levelAsset == null)
            {
                UnityEngine.Debug.LogError("Couldn't find levelAsset?");
            }

            scenePaths = levelAsset.GetAllScenePaths();
            foreach (var levelAsset in scenePaths)
            {
                levelList.Add(levelAsset);
            }
        }

        private async void Update()
        {
            if (GameManager.instance.isPlaying && PhotonNetwork.OfflineMode)
            {
                MapManager.instance.levels = levelList.ToArray();
            }
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (levelAsset == null)
                {
                    UnityEngine.Debug.LogError("Couldn't find levelAsset?");
                }
                if (scenePaths[0] == null)
                {
                    UnityEngine.Debug.LogError("Couldn't find scene?");
                }
                SceneManager.LoadScene(scenePaths[0], LoadSceneMode.Additive);

                // var scene = SceneManager.GetSceneByPath(scenePath[0]);
                // var objects = new List<GameObject>();
                // scene.GetRootGameObjects(objects);
                //
                // foreach (var o in objects)
                // {
                //     UnityEngine.Debug.Log(o.name);
                // }

                await Task.Delay(100);
                

            }
        }
    }
}