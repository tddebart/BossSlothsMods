using UnboundLib;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class EffectBulletRotate : MonoBehaviour
    {
        public Vector3 scale = new Vector3(1.3f, 1.3f, 1.3f);
        public Vector3 rotation = new Vector3(90,0,0);
        
        void Awake()
        {
            if (transform.parent == null) return;
            transform.localRotation = Quaternion.Euler(rotation);
            this.ExecuteAfterSeconds(0.05f, () =>
            {
                transform.localScale = scale * (transform.parent.Find("Trail").GetComponent<TrailRenderer>().startWidth+0.7f);
            });
        }
    }
}