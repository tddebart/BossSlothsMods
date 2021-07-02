using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class A_Sneeze : MonoBehaviour
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