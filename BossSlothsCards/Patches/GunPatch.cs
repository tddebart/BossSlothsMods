using BossSlothsCards.Cards;
using HarmonyLib;
using Sonigon;
using UnityEngine;

namespace BossSlothsCards.Patches
{
    public class GunPatch
    {
        [HarmonyPatch(typeof(Gun),"DoAttack")]
        private class Patch_blocked
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(Gun __instance)
            {
                if (__instance.GetComponent<Holdable>().holder.transform.Find("Particles/Orange circle(Clone)"))
                {
                    UnityEngine.Debug.LogWarning("shot with 3670");
                    __instance.GetComponent<Holdable>().holder.GetComponent<A_GetCamera>().hasEnable = false;
                }
            }
        }
    }
}