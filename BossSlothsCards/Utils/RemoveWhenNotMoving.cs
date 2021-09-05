using BossSlothsCards.Extensions;
using UnboundLib.Utils;
using UnityEngine;

namespace BossSlothsCards.Utils
{
    public class RemoveWhenNotMoving : MonoBehaviour
    {
        public Vector3 lastPosition;
        public Quaternion lastRotation;

        public TimeSince timeSinceStatic;

        public void Update()
        {
            if (transform.position.Rounded() == lastPosition.Rounded() && lastRotation == transform.rotation && !GameManager.lockInput)
            {
                foreach(var obj in GetComponentsInChildren<MeshRenderer>())
                {
                    obj.enabled = false;
                }

                var emissionModule = GetComponentInChildren<ParticleSystem>().emission;
                emissionModule.enabled = false;
                if (timeSinceStatic > 2.6f)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                timeSinceStatic = 0;
            }

            lastRotation = transform.rotation;
            lastPosition = transform.position;
        }
    }
}