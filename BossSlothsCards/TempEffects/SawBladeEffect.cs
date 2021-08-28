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
                GetComponent<PhotonView>().RPC("RPCA_SpawnSaw_Stat", RpcTarget.All, position, stats.GetAdditionalData().sawBladeScale);
            }
        }

        [PunRPC]
        public void RPCA_SpawnSaw_Stat(Vector2 position, float scale)
        {
            var saw = Instantiate(BossSlothCards.instance.betterSawObj, position, Quaternion.identity);
            saw.GetComponent<DamageBox>().damage = 27*scale;
            saw.transform.localScale = new Vector3(scale, scale);
            saw.transform.SetParent(SceneManager.GetSceneAt(1).GetRootGameObjects()[0].transform);
            var rem = saw.AddComponent<RemoveAfterSeconds>();
            rem.seconds = 4.5f;
            //TODO see if this network works
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