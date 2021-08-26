namespace BossSlothsCards.Patches
{
    public class BlockPatch
    {
        // [HarmonyPatch(typeof(Block),"TryBlock")]
        // private class Patch_blocked
        // {
        //     // ReSharper disable once UnusedMember.Local
        //     private static bool Prefix(Block __instance)
        //     {
        //         
        //         var gun = (Gun)Traverse.Create(__instance.GetComponent<Holding>()).Field("gun").GetValue();
        //         UnityEngine.Debug.LogWarning("try block");
        //         if (gun.unblockable)
        //         {
        //             UnityEngine.Debug.LogWarning("failed block");
        //             __instance.RPCA_DoBlock(true, false, BlockTrigger.BlockTriggerType.Default, default(Vector3), true);
        //             return false;
        //         }
        //         UnityEngine.Debug.LogWarning("did block");
        //         __instance.RPCA_DoBlock(true, false, BlockTrigger.BlockTriggerType.Default, default(Vector3), false);
        //         return false;
        //     }
        // }
        // [HarmonyPatch(typeof(Block),"blocked")]
        // private class Patch_blocked1
        // {
        //     // ReSharper disable once UnusedMember.Local
        //     private static void Postfix()
        //     {
        //         UnityEngine.Debug.LogWarning("blocked true");
        //     }
        // }
    }
}