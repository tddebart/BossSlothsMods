using HarmonyLib;

namespace BossSlothsMod
{
    internal class jumpPatch
    {

        [HarmonyPatch(typeof (CharacterStatModifiers), "ResetStats")]
        private class Patch_Jump
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(CharacterData ___data)
            {
                UnityEngine.Debug.Log("JumpFix");
                ___data.jumps = 1;
            }
        }
        
        

        
    }
}