using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomMapLoader
{
    internal class cardBarPatch
    {

        [HarmonyPatch(typeof (MapManager), "OnLevelFinishedLoading")]
        private class Patch_Jump
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(Scene scene, LoadSceneMode mode)
            {
                var sfPoly = Resources.FindObjectsOfTypeAll<SFPolygon>();
                var sfFilter = new List<SFPolygon>();
                foreach (var sf in sfPoly)
                {
                    if (sf.name.Contains("Ground"))
                    {
                        sfFilter.Add(sf);
                    }
                }
                if (sfFilter.Count == 0)
                {
                    UnityEngine.Debug.LogError("No ground found?");
                }
                foreach (var sf in sfFilter)
                {
                    //UnityEngine.Debug.Log(sf.gameObject.name);
                    // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
                    if (sf.GetComponent<SpriteRenderer>() != null)
                    {
                        sf.GetComponent<SpriteRenderer>().material.shader = Shader.Find("Sprites/SFSoftShadowStencil");
                        //UnityEngine.Debug.LogWarning(shader.ToString());
                    }
                }
            }
        }
        
        

        
    }
}