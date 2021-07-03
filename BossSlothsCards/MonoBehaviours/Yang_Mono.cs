using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class Yang_Mono : MonoBehaviour
    {
        private Block _block;

        private void Update()
        {
            if (gameObject.transform.parent != null) _block = gameObject.GetComponentInParent<Block>();
            
            if (_block != null && _block.cooldown < 3f)
            {
                _block.cooldown = 3f;
            }
        }
    }
}