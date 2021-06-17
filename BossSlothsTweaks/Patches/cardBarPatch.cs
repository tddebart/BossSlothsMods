using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace BossSlothsTweaks.Patches
{
    internal class cardBarPatch
    {

        [HarmonyPatch(typeof (CardBarHandler), "Start")]
        private class Patch_Jump
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix()
            {
                BossSlothsTweaks.ChangeCards();
            }
        }
        
        

        
    }
}