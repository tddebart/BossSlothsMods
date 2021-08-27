using System;
using System.Linq;
using ModdingUtils.Extensions;
using UnboundLib;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class Quadratics_Mono : BossSlothMonoBehaviour
    {
        private bool start;
        
        void Awake()
        {
            this.ExecuteAfterSeconds(0.05f, () =>
            {
                start = true;
            });
        }
        
        void FixedUpdate()
        {
            if (!start) return;
            
            if (transform.parent != null && PlayerManager.instance.players.Any(pl => PlayerStatus.PlayerAlive(pl) && Math.Round(pl.transform.position.x) == Math.Round(transform.position.x)))
            {
                GetComponentInParent<MoveTransform>().velocity = new Vector2(0,-25f);
            }
        }
    }
}