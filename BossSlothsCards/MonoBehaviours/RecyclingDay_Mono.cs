using Photon.Pun;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class RecyclingDay_Mono : MonoBehaviour
    {
        [PunRPC]
        public void RPCA_FixBox()
        {
            var currentObj = MapManager.instance.currentMap.Map.gameObject.transform.GetChild(MapManager.instance.currentMap.Map.gameObject.transform.childCount - 1);
            var rigid = currentObj.GetComponent<Rigidbody2D>();
            rigid.isKinematic = false;
            rigid.simulated = true;
            // currentObj.GetComponent<DamagableEvent>().deathEvent = new UnityEvent();
            // currentObj.GetComponent<DamagableEvent>().deathEvent.AddListener(() =>
            // {
            //     Destroy(currentObj.gameObject);
            // });
        }
    }
}