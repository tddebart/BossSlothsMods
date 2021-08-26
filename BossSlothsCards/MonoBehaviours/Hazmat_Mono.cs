using HarmonyLib;
using ModdingUtils.Extensions;
using UnityEngine;
using CharacterDataExtension = BossSlothsCards.Extensions.CharacterDataExtension;

namespace BossSlothsCards.MonoBehaviours
{
    public class Hazmat_Mono : MonoBehaviour
    {
        private void Update()
        {
            var ooB = CharacterDataExtension.GetAdditionalData(GetComponent<CharacterData>()).outOfBoundsHandler;
            if ((bool) Traverse.Create(ooB).Field("outOfBounds").GetValue() && PlayerStatus.PlayerAlive(GetComponent<Player>()))
            {
                GetComponent<CharacterData>().block.sinceBlock = 0.299f;
            }
            if ((bool) Traverse.Create(ooB).Field("almostOutOfBounds").GetValue() && PlayerStatus.PlayerAlive(GetComponent<Player>()))
            {
                GetComponent<CharacterData>().block.sinceBlock = 0.299f;
            }
        }
    }
}