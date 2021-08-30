using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class Pong_Bullet : MonoBehaviour
    {
        private Player player;
        private bool isplayerNotNull;

        void Start()
        {
            if (GetComponentInParent<SpawnedAttack>())
            {
                player = GetComponentInParent<SpawnedAttack>().spawner;
            }
            isplayerNotNull = player != null;
        }
        
        void Update()
        {
            if (isplayerNotNull && player.gameObject.activeInHierarchy == false)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}