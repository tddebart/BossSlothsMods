using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace ActualRoundsMod
{
    internal class jumpPatch : BaseUnityPlugin
    {

        [HarmonyPatch(typeof (CharacterStatModifiers), "ResetStats")]
        private class Patch_Jump
        {
            private static void Postfix(CharacterData ___data)
            {
                UnityEngine.Debug.Log("JumpFix");
                ___data.jumps = 1;
            }
        }
        
        

        
    }
}