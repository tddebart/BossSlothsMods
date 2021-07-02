using System.Linq;
using BossSlothsCards.Cards;
using HarmonyLib;
using Sonigon;
using UnityEngine;

namespace BossSlothsCards.Patches
{
    public class GM_ArmsracePatch
    {
        [HarmonyPatch(typeof(GM_ArmsRace),"PointOver")]
        private class Patch_GM_Armsrace
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(int winningTeamID)
            {
                UnityEngine.Debug.LogWarning("point over !!!!!!!!");
                foreach (var player in PlayerManager.instance.players.Where(player => player.transform.Find("Particles/Orange circle(Clone)")))
                {
                    player.GetComponent<A_GetCamera>().hasEnable = false;
                }
            }
        }
    }
}