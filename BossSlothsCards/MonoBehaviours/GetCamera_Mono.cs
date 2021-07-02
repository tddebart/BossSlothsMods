using System.Linq;
using HarmonyLib;
using Photon.Pun;
using UnboundLib;
using UnityEngine;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace BossSlothsCards.Cards
{
    public class GetCamera_Mono : MonoBehaviour
    {
        public Aim _aim;

        public bool hasEnable;

        private bool onCooldown;
        private Holding _holding;
        private Gun _gun;

        private GameObject cube;

        public Vector3 aimDirection;
        
        private float origDamage;

        public float m_currentAngle;
        public float m_lastAngle;
        public float m_cumulativeGunRotation;

        public float rotation;
        public float zeroRotationTime;

        public GameObject circle;

        private void Start()
        {
            _holding = GetComponent<Holding>();
            _gun = _holding.holdable.GetComponent<Gun>();

            _aim = GetComponent<Aim>();

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = _gun.transform;
            cube.transform.localPosition = new Vector3(0, 100, 0);
            
            circle = Instantiate(BossSlothCards.EffectAsset.LoadAsset<GameObject>("Orange circle"), transform.Find("Particles"));
            circle.transform.localPosition = Vector3.zero;
        }
        
        private void Update()
        {
            // Create and update circle effect
            if (hasEnable)
            {
                circle.gameObject.SetActive(true);
            }
            else
            {
                if (origDamage != 0f)
                {
                    _gun.damage = origDamage;
                }
                origDamage = _gun.damage;
                circle.gameObject.SetActive(false);
            }

            #region 360 calculation
            // calculate if did 360
            
            // angle goes from 0 to 360
            m_currentAngle = Vector2.SignedAngle(transform.position, cube.transform.position) + 180;

            rotation = Vector2.SignedAngle(DegreesToVector(m_currentAngle), DegreesToVector(m_lastAngle));
            rotation = Mathf.Clamp(rotation, -90, 90);
            if (Mathf.Abs(rotation) < 120f * Mathf.Min(0.1f, Time.deltaTime))
            {
                zeroRotationTime += Time.deltaTime;
                if (zeroRotationTime < 0.0333f)
                {
                    return;
                }
                rotation = 0f;
                m_cumulativeGunRotation = 0f;
            }
            else
            {
                zeroRotationTime = 0f;
            }

            m_lastAngle = Vector2.SignedAngle(transform.position, cube.transform.position) + 180;
            m_cumulativeGunRotation += rotation;
            if (m_cumulativeGunRotation > 360f)
            {
                m_cumulativeGunRotation = 0f;
                done360();
            }
            else if (m_cumulativeGunRotation < -360f)
            {
                m_cumulativeGunRotation = 0f;
                done360();
            }
            #endregion


            void done360()
            {
                if (onCooldown) return;
                #if DEBUG
                UnityEngine.Debug.LogWarning("Did 360");
                #endif
                _gun.damage += origDamage * 0.45f;
                hasEnable = true;
                onCooldown = true;
                this.ExecuteAfterSeconds(2f, () =>
                {
                    onCooldown = false;
                });
                if (GetComponent<PhotonView>().IsMine)
                {
                    GetComponent<PhotonView>().RPC("RPCA_doCircleAnimation", RpcTarget.All);
                }
               
            }
        }
        
        public static Vector2 DegreesToVector(float angle, float magnitude = 1f)
        {
            float f = angle * 0.017453292f;
            return new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * magnitude;
        }

        [PunRPC]
        public void RPCA_doCircleAnimation()
        {
            circle.GetComponent<Animator>().SetTrigger("Activaded");
        }
    }
    
    
    [HarmonyPatch(typeof(GM_ArmsRace),"PointOver")]
    class Patch_GM_Armsrace
    {
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(int winningTeamID)
        {
            foreach (var player in PlayerManager.instance.players.Where(player => player.transform.Find("Particles/Orange circle(Clone)")))
            {
                player.GetComponent<GetCamera_Mono>().hasEnable = false;
            }
        }
    }
    
    [HarmonyPatch(typeof(Gun),"DoAttack")]
    class Patch_DoAttack
    {
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(Gun __instance)
        {
            if (__instance.GetComponent<Holdable>().holder.transform.Find("Particles/Orange circle(Clone)"))
            {
                UnityEngine.Debug.LogWarning("shot with 3670");
                __instance.GetComponent<Holdable>().holder.GetComponent<GetCamera_Mono>().hasEnable = false;
            }
        }
    }
}