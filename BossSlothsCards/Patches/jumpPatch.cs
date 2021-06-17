using System.Linq;
using HarmonyLib;

namespace BossSlothsCards.Patches
{
    internal class jumpPatch
    {

        [HarmonyPatch(typeof (CharacterStatModifiers), "ResetStats")]
        private class Patch_ResetJump
        {
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(CharacterData ___data)
            {
                UnityEngine.Debug.Log("JumpFix");
                ___data.jumps = 1;
            }
        }
        
        [HarmonyPatch(typeof (PlayerJump), "Jump")]
        private class Patch_DoubleJump
        {
            // ReSharper disable once UnusedMember.Local
            private static void Prefix(CharacterData ___data, ref bool __state)
            {
                var cardList = CardChoice.instance.cards.ToList();
                CardInfo jumpCard = null;
                foreach (var card in cardList)
                {
                    if (card.name == "Double jump")
                    {
                        jumpCard = card;
                        break;
                    }
                }

                if (___data.currentCards.Contains(jumpCard) && ___data.currentJumps == 1)
                {
                    __state = true;
                }
                else
                {
                    __state = false;
                }
            }
            
            // ReSharper disable once UnusedMember.Local
            private static void Postfix(CharacterData ___data, bool __state)
            {
                if (__state )
                {
                    ___data.currentJumps--;
                }
            }
        }
        

        
    }
}