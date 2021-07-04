using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class Sneeze_Mono : BossSlothMonoBehaviour
    {

        private void Awake()
        {
            if (transform.parent)
            {
                transform.parent.Find("Collider").gameObject.SetActive(false);
            }
        }
    }
}