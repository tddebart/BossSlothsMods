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

        private MoveTransform moveTransform;
        private PhotonView photonView;

        private bool photonViewNotNull;

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
            this.ExecuteAfterSeconds(0.08f, () =>
            {
                start = true;
            });

            photonView = GetComponent<PhotonView>();
            moveTransform = GetComponent<MoveTransform>();

            photonViewNotNull = photonView != null;
        }

        private void FixedUpdate()
        {
            if (!start) return;
            
            if (photonViewNotNull)
            {
                foreach (var player in PlayerManager.instance.players.Where(PlayerStatus.PlayerAlive))
                {
                    if(Math.Round(player.transform.position.x) == Math.Round(transform.position.x) && player.transform.position.y < transform.position.y)
                    {
                        moveTransform.velocity = new Vector2(0,-25f);
                        moveTransform.gravity = 1.0f;
                    }
                }
            }
        }
    }
}