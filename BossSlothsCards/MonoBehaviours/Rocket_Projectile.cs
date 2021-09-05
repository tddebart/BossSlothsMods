using BossSlothsCards.Utils;
using SoundImplementation;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class Rocket_Projectile : MonoBehaviour
    {
        private GameObject rocketObj = BossSlothCards.EffectAsset.LoadAsset<GameObject>("Rocket");
        private FollowLocation location;
        
        private bool returnY;
        
        void Awake()
        {
            if (transform.parent == null)
            {
                returnY = true;
                return;
            }

            var obj = Instantiate(rocketObj);
            obj.AddComponent<RemoveWhenNotMoving>();
            var thruster = (GameObject)Resources.Load("0 cards/Thruster");
            var soundLoop = thruster.GetComponent<Gun>().objectsToSpawn[0].effect.GetComponent<SoundUnityEventPlayer>()
                .soundStartLoop;
            obj.GetComponent<SoundUnityEventPlayer>().soundStartLoop = soundLoop;
            
            location = obj.AddComponent<FollowLocation>();
        }

        void Update()
        {
            if (returnY) return;
            location.followLocation = transform.position;
            location.followRotation = transform.rotation;
        }
    }
}