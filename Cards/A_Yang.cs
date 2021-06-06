using UnityEngine;
using Logger = BepInEx.Logging.Logger;

namespace ActualRoundsMod
{
    public class A_Yang : MonoBehaviour
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