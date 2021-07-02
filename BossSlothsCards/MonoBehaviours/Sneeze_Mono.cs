using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class Sneeze_Mono : MonoBehaviour
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