using HarmonyLib;
using ModdingUtils.Extensions;
using UnityEngine;
using CharacterDataExtension = BossSlothsCards.Extensions.CharacterDataExtension;

namespace BossSlothsCards.MonoBehaviours
{
    public class Hazmat_Mono : BossSlothMonoBehaviour
    {
        private Player player;
        private OutOfBoundsHandler ooB;
        private CharacterData data;
        
        private void Start()
        {
            player = GetComponent<Player>();
            ooB = CharacterDataExtension.GetAdditionalData(GetComponent<CharacterData>()).outOfBoundsHandler;
            data = GetComponent<CharacterData>();
        }
        
        private void Update()
        {
            if ((bool) Traverse.Create(ooB).Field("outOfBounds").GetValue() && PlayerStatus.PlayerAlive(player) && transform.position.y >= -18.5f)
            {
                data.block.sinceBlock = 0.299f;
            }
            if ((bool) Traverse.Create(ooB).Field("almostOutOfBounds").GetValue() && PlayerStatus.PlayerAlive(player) && transform.position.y >= -18.5f)
            {
                data.block.sinceBlock = 0.299f;
            }
        }
    }
}