using System;
using System.Linq;
using ModdingUtils.Extensions;
using Photon.Pun;
using UnboundLib;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class Quadratics_Bullet : BossSlothMonoBehaviour
    {
        private bool start;

        void Awake()
        {
            if (transform.parent != null)
            {
                transform.parent.gameObject.AddComponent<Quadratics_Bullet>();
                return;
            }
            else
            {
                foreach (var obj in GetComponentsInChildren<Quadratics_Bullet>().Where(bullet => bullet != this))
                {
                    Destroy(obj.gameObject);
                }
            }
            this.ExecuteAfterSeconds(0.05f, () =>
            {
                start = true;
            });
        }
        
        void FixedUpdate()
        {
            if (!start) return;
            
            if (GetComponent<PhotonView>() && PlayerManager.instance.players.Any(pl => PlayerStatus.PlayerAlive(pl) && !pl.GetComponent<PhotonView>().IsMine && Math.Round(pl.transform.position.x) == Math.Round(transform.position.x)))
            {
                GetComponent<PhotonView>().RPC("RPCA_GoDown", RpcTarget.AllViaServer);
            }
        }

        [PunRPC]
        public void RPCA_GoDown()
        {
            GetComponent<MoveTransform>().velocity = new Vector2(0,-25f);
        }
    }
}