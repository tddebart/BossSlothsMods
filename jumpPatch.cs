using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace ActualRoundsMod
{
    [BepInPlugin("MyCoolPlugin.gsgsgs.Yes.patch", "JumpPatch", "1.0.0")]
    class jumpPatch : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("com.round.MyEpicRoundsMod.Yeetus.Harmony");
        
        private void Start()
        {
            harmony.PatchAll();
        }
        
        [HarmonyPatch(typeof (CharacterStatModifiers), "ResetStats")]
        private class Patch_Jump
        {
            private static void Postfix(CharacterData ___data)
            {
                Startup.Instance.Message("JumpFix");
                ___data.jumps = 1;
            }
        }
        
        

        
    }
}