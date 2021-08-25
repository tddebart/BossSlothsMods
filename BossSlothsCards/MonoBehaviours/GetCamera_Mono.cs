using System.Collections;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Photon.Pun;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace BossSlothsCards.MonoBehaviours
{
    public class GetCamera_Mono : BossSlothMonoBehaviour
    {
        public Aim _aim;

        public bool hasEnable;

        public bool onCooldown;
        private Holding _holding;
        private Gun _gun;

        private GameObject cube;

        public Vector3 aimDirection;
        
        private float originDamage;

        public float m_currentAngle;
        public float m_lastAngle;
        public float m_cumulativeGunRotation;

        public float rotation;
        public float zeroRotationTime;

        public GameObject circle;
        private static readonly int Activaded = Animator.StringToHash("Activaded");

        private void Start()
        {
            _holding = GetComponent<Holding>();
            _gun = _holding.holdable.GetComponent<Gun>();

            _aim = GetComponent<Aim>();

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = _gun.transform;
            cube.transform.localPosition = new Vector3(0, 100, 0);

            if (!transform.Find("Particles/Orange circle(Clone)"))
            {
                circle = Instantiate(BossSlothCards.EffectAsset.LoadAsset<GameObject>("Orange circle"), transform.Find("Particles"));
                circle.transform.localPosition = Vector3.zero;
            }

            GameModeManager.AddHook(GameModeHooks.HookPointEnd, (gm) => ResetBetweenRounds());
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
                if (originDamage != 0f)
                {
                    _gun.damage = originDamage;
                }
                originDamage = _gun.damage;
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
                if (GetComponent<PhotonView>().IsMine)
                {
                    GetComponent<PhotonView>().RPC("RPCA_Do360", RpcTarget.All, GetComponent<Player>().playerID, originDamage);
                }
            }
        }

        private static Vector2 DegreesToVector(float angle, float magnitude = 1f)
        {
            var f = angle * 0.017453292f;
            return new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * magnitude;
        }

        [PunRPC]
        public void RPCA_Do360(int playerID, float _originDamage)
        {
            var player = (Player) typeof(PlayerManager).InvokeMember("GetPlayerWithID",
                BindingFlags.Instance | BindingFlags.InvokeMethod |
                BindingFlags.NonPublic, null, PlayerManager.instance, new object[] {playerID});
            // ReSharper disable once LocalVariableHidesMember
            var getCameraMono = player.GetComponent<GetCamera_Mono>();
            if (getCameraMono.onCooldown) return;
#if DEBUG
            UnityEngine.Debug.LogWarning("Did 360");
#endif
            getCameraMono.hasEnable = true;
            getCameraMono.onCooldown = true;
            getCameraMono.ExecuteAfterSeconds(2f, () =>
            {
                getCameraMono.onCooldown = false;
            });
            if (getCameraMono.GetComponent<PhotonView>().IsMine)
            {
                getCameraMono.GetComponent<PhotonView>().RPC("RPCA_doCircleAnimation", RpcTarget.All, new object[] { getCameraMono.GetComponent<Player>().playerID, _originDamage});
            }

        }

        [PunRPC]
        public void RPCA_doCircleAnimation(int playerID, float _origDamage)
        {
            var player = (Player) typeof(PlayerManager).InvokeMember("GetPlayerWithID",
                BindingFlags.Instance | BindingFlags.InvokeMethod |
                BindingFlags.NonPublic, null, PlayerManager.instance, new object[] {playerID});
            var _circle = player.transform.Find("Particles/Orange circle(Clone)");
            _circle.gameObject.SetActive(true);
            _circle.GetComponent<Animator>().SetTrigger(Activaded);
            player.GetComponent<Holding>().holdable.GetComponent<Gun>().damage += _origDamage * 0.45f;
        }

        private static IEnumerator ResetBetweenRounds()
        {
            foreach (var player in PlayerManager.instance.players.Where(player => player.transform.Find("Particles/Orange circle(Clone)")))
            {
                var mono = player.GetComponent<GetCamera_Mono>();
                mono.hasEnable = false;
                mono.onCooldown = false;
            }
            yield break;
        }

        private void OnDestroy()
        {
            Destroy(circle);
            Destroy(cube);
            GameModeManager.RemoveHook(GameModeHooks.HookPointEnd, (gm) => ResetBetweenRounds());
        }
    }
}