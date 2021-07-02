using UnboundLib;
using UnityEngine;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace BossSlothsCards.Cards
{
    public class A_GetCamera : MonoBehaviour
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

        private void Start()
        {
            _holding = GetComponent<Holding>();
            _gun = _holding.holdable.GetComponent<Gun>();

            _aim = GetComponent<Aim>();

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = _gun.transform;
            cube.transform.localPosition = new Vector3(0, 100, 0);
        }
        
        private void Update()
        {
            // Create and update circle effect
            var circle = transform.Find("Particles/Orange circle(Clone)");
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
                transform.Find("Particles/Orange circle(Clone)").GetComponent<Animator>().SetTrigger("Activaded");
            }
        }
        
        public static Vector2 DegreesToVector(float angle, float magnitude = 1f)
        {
            float f = angle * 0.017453292f;
            return new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * magnitude;
        }
    }
}