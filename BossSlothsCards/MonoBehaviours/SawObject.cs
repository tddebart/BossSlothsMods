using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class SawObject : MonoBehaviour
    {
        void Awake()
        {
            transform.localRotation = Quaternion.Euler(new Vector3(90,0,0));
        }
    }
}