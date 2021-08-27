using BossSlothsCards.Extensions;
using BossSlothsCards.Utils;
using ModdingUtils.RoundsEffects;
using Photon.Pun;
using UnboundLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossSlothsCards.TempEffects
{
    public class SawBladeEffect : HitSurfaceEffect
    {

        public TimeSince timeSinceLastSaw;
        public bool doEffect;
        
        private CharacterStatModifiers stats;

        public void Awake()
        {
            if (GetComponent<CharacterStatModifiers>())
            {
                stats = GetComponent<CharacterStatModifiers>();
            }
        }
        
        public override void Hit(Vector2 position, Vector2 normal, Vector2 velocity)
        {
            if (doEffect)
            {
                timeSinceLastSaw = 0;
                doEffect = false;
                var saw = PhotonNetwork.Instantiate("MapObject_Saw_Stat", position, Quaternion.identity);
                saw.transform.localScale = Vector3.one;
                saw.transform.SetParent(SceneManager.GetSceneAt(1).GetRootGameObjects()[0].transform);
                var rem = saw.AddComponent<RemoveAfterSeconds>();
                rem.seconds = 4.5f;
            }
        }

        public void Update()
        {
            if (timeSinceLastSaw > 15)
            {
                //stats.GetAdditionalData().shouldSpawnSaw = true;
            }
            
            // if (!startedCounting && isstatsNotNull && !stats.GetAdditionalData().shouldSpawnSaw)
            // {
            //     startedCounting = true;
            //     this.ExecuteAfterSeconds(15, () =>
            //     {
            //         stats.GetAdditionalData().shouldSpawnSaw = true;
            //     });
            // }
        }
    }
}