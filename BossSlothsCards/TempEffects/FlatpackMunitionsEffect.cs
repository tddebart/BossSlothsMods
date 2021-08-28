using BossSlothsCards.Extensions;
using BossSlothsCards.Utils;
using ModdingUtils.RoundsEffects;
using Photon.Pun;
using UnboundLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossSlothsCards.TempEffects
{
    public class FlatpackMunitionsEffect : HitSurfaceEffect
    {

        public TimeSince timeSinceLastBox;

        private CharacterStatModifiers stats;

        public void Awake()
        {
            if (GetComponent<CharacterStatModifiers>())
            {
                stats = GetComponent<CharacterStatModifiers>();
            }
        }
        
        public override void Hit(Vector2 position, Vector2 normal, Vector2 velocity)
        {
            if (timeSinceLastBox > 1)
            {
                timeSinceLastBox = 0;
                GetComponent<PhotonView>().RPC("RPCA_SpawnBox", RpcTarget.All, position);
            }
        }

        [PunRPC]
        public void RPCA_SpawnBox(Vector2 position)
        {
            var box = Instantiate(BossSlothCards.instance.betterDesBox, position, Quaternion.identity);
            //box.AddComponent<PhotonMapObject>();
            box.GetComponent<Rigidbody2D>().simulated = true;
            box.GetComponent<PhotonView>().ViewID = 696969;
            box.transform.SetParent(SceneManager.GetSceneAt(1).GetRootGameObjects()[0].transform);
            var rem = box.AddComponent<RemoveAfterSeconds>();
            rem.seconds = 4.5f;
        }
    }
}