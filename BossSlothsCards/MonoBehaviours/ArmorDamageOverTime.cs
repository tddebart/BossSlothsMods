using System.Collections;
using Sonigon;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class ArmorDamageOverTime : MonoBehaviour
    {
        private ArmorHandler armorHandler;
        private CharacterData data;

        private void Start()
        {
            armorHandler = GetComponent<ArmorHandler>();
            data = GetComponent<CharacterData>();
        }
        
        public void DoDamageOverTimeVoid(Vector2 damage, Vector2 position, float time, float interval, Color color, SoundEvent soundDamageOverTime, GameObject damagingWeapon = null, Player damagingPlayer = null, bool lethal = true)
        {
            StartCoroutine(DoDamageOverTime(damage, position, time, interval, color, soundDamageOverTime,
                damagingWeapon, damagingPlayer, lethal));
        }
        
        private IEnumerator DoDamageOverTime(Vector2 damage, Vector2 position, float time, float interval, Color color, SoundEvent soundDamageOverTime, GameObject damagingWeapon = null, Player damagingPlayer = null, bool lethal = true)
        {
            float damageDealt = 0f;
            float damageToDeal = damage.magnitude;
            float dpt = damageToDeal / time * interval;
            while (damageDealt < damageToDeal)
            {
                if (soundDamageOverTime != null && this.data.isPlaying && !this.data.dead)
                {
                    SoundManager.Instance.Play(soundDamageOverTime, base.transform);
                }
                damageDealt += dpt;
                armorHandler.DoDamage((damage.normalized * dpt).magnitude, position, color, damagingWeapon, damagingPlayer, lethal);
                yield return new WaitForSeconds(interval / TimeHandler.timeScale);
            }
            yield break;
        }
    }
}