using System;
using System.Text.RegularExpressions;
using HarmonyLib;
using UnityEngine;
// ReSharper disable UnusedMember.Local

namespace CardBarPatch.Patches
{
    internal class cardBarPatch
    {

        [HarmonyPatch(typeof (CardBarHandler), "Start")]
        private class Patch_Start_Handler
        {
            private static void Postfix()
            {
                CardBarPatch.CardBar();
            }
        }

        [HarmonyPatch(typeof(CardBar), "Start")]
        private class Patch_Start_Bar
        {
            private static void Postfix(CardBar __instance)
            {
                var deltaY = -CardBarPatch.verticalDistance.Value;
                Transform transform1;
                var index = 0;
                var numbers = Regex.Split(__instance.name, @"\D+");
                foreach (var value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        index = int.Parse(value)-1;
                    }
                }
                var barGo = (transform1 = __instance.transform).parent.transform.GetChild(0).gameObject;
                transform1.localPosition = barGo.transform.localPosition + new Vector3(0, deltaY * index, 0);
            }
        }
        
        

        
    }
}