using System.Collections.Generic;
using BossSlothsCards.TempEffects;
using HarmonyLib;
using UnityEngine;

namespace BossSlothsCards.Patches
{
    public class ResetPatch
    {
        [HarmonyPatch(typeof(Player),"FullReset")]
        private class Patch_blocked
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(Player __instance)
            {
                foreach (var resetObj in __instance.GetComponents<BossSlothMonoBehaviour>())
                {
                    Object.Destroy(resetObj);
                }
                foreach (var resetObj in __instance.GetComponentsInChildren<BossSlothMonoBehaviour>())
                {
                    if (!resetObj.gameObject.name.Contains("Player"))
                    {
                        Object.Destroy(resetObj.gameObject);
                    }
                }
                foreach (var resetObj in __instance.GetComponents<SplittingGun>())
                {
                    Object.Destroy(resetObj);
                }
                __instance.data.currentCards = new List<CardInfo> { };
            }
        }
    }
}