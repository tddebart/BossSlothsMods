using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class FollowLocation : MonoBehaviour
    {
        public Vector3 followLocation;
        public Quaternion followRotation;

        public void Update()
        {
            transform.position = followLocation;
            transform.rotation = followRotation;
        }
    }
}