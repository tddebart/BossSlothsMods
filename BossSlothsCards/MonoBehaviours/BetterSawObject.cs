using BossSlothsCards.Extensions;
using BossSlothsCards.TempEffects;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class BetterSawObject : MonoBehaviour
    {
        void Awake()
        {
            if (transform.parent == null) return;
            var player = PlayerManager.instance.GetClosestPlayer(transform.position);
            if (!(player.GetComponent<SawBladeEffect>().timeSinceLastSaw > 15))
            {
                gameObject.SetActive(false);
                return;
            }

            player.GetComponent<SawBladeEffect>().timeSinceLastSaw = 0;
            player.GetComponent<SawBladeEffect>().doEffect = true;
            transform.localRotation = Quaternion.Euler(new Vector3(90,0,0));
        }
    }
}