using BossSlothsCards.Extensions;
using BossSlothsCards.Utils;
using ModdingUtils.RoundsEffects;
using Photon.Pun;
using UnboundLib;
using UnboundLib.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BossSlothsCards.TempEffects
{
    public class WreckingBallEffect : HitSurfaceEffect
    {

        public TimeSince timeSinceLastBall;

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
            if (timeSinceLastBall > 1)
            {
                timeSinceLastBall = 0;
                if (GetComponent<PhotonView>().IsMine)
                {
                    PhotonNetwork.Instantiate("4 map objects/Box_Destructible_Small", (position + 0.75f*normal), Quaternion.identity);
                }
                // var rem = box.AddComponent<RemoveAfterSeconds>();
                // rem.seconds = 4;
                
                this.ExecuteAfterSeconds(0.02f, () =>
                {
                    var currentObj =
                        MapManager.instance.currentMap.Map.gameObject.transform.GetChild(MapManager.instance.currentMap.Map.gameObject.transform.childCount - 1);
                    var rigid = currentObj.GetComponent<Rigidbody2D>();
                    rigid.isKinematic = false;
                    rigid.simulated = true;
                    var renderers = currentObj.GetComponentsInChildren<SpriteRenderer>();
                    renderers[0].sprite = BossSlothCards.EffectAsset.LoadAsset<Sprite>("CircleHollow4");
                    renderers[1].sprite = BossSlothCards.EffectAsset.LoadAsset<Sprite>("CircleHollow4");
                    Destroy(currentObj.GetComponent<BoxCollider2D>());
                    currentObj.gameObject.AddComponent<CircleCollider2D>();
                    currentObj.transform.localScale = new Vector3(0.2f, 0.2f);
                    currentObj.GetComponent<DamagableEvent>().deathEvent = new UnityEvent();
                    currentObj.GetComponent<DamagableEvent>().deathEvent.AddListener(() =>
                    {
                        Destroy(currentObj.gameObject);
                    });
                });
            }
        }

        // [PunRPC]
        // public void RPCA_SpawnBox(Vector2 position)
        // {
        //     var box = Instantiate(BossSlothCards.instance.betterDesBox, position, Quaternion.identity);
        //     //box.AddComponent<PhotonMapObject>();
        //     box.GetComponent<Rigidbody2D>().simulated = true;
        //     box.GetComponent<PhotonView>().ViewID = 696969;
        //     box.transform.SetParent(SceneManager.GetSceneAt(1).GetRootGameObjects()[0].transform);
        //     var rem = box.AddComponent<RemoveAfterSeconds>();
        //     rem.seconds = 4.5f;
        // }
    }
}