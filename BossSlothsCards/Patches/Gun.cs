using BossSlothsCards.MonoBehaviours;
using HarmonyLib;
using ModdingUtils.MonoBehaviours;

namespace BossSlothsCards.Patches
{
    [HarmonyPatch(typeof(Gun),"DoAttack")]
    class Patch_DoAttack
    {
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(Gun __instance)
        {
            if (__instance.GetComponent<Holdable>() && __instance.GetComponent<Holdable>().holder.transform.Find("Particles/Orange circle(Clone)"))
            {
#if DEBUG
                UnityEngine.Debug.LogWarning("shot with 360");
#endif
                __instance.GetComponent<Holdable>().holder.GetComponent<GetCamera_Mono>().hasEnable = false;
            }
        }
    }
}