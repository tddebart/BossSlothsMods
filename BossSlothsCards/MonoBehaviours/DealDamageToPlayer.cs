using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class DealDamageToPlayer : MonoBehaviour
    {
        public float damage = 25f;
        
        public bool lethal = true;
        
        public TargetPlayer targetPlayer;

        public bool doPercentageDamage;

        public float damagePercentage;

        public bool doConsistentDamage;

        public float timeBetweenDamage;

        public bool stopAtPercentage;

        public float percentageStop;

        private bool coroutineStarted;
        
        private CharacterData data;

        private float damageDone;
        
        private Player target;
        
        public enum TargetPlayer
        {
            Own,
            Other
        }
        
        private void Start()
        {
            data = GetComponentInParent<CharacterData>();
        }

        private void Consistent()
        {
            if (target.gameObject.activeInHierarchy == false) return;
            if (stopAtPercentage && percentageStop > (target.data.HealthPercentage))
            {
                target.data.healthHandler.Heal(damageDone);
                damageDone = 0;
                return;
            }
            Go();
        }
        private void Update()
        {
            if (!target)
            {
                if (!(data is null)) target = data.player;
                if (targetPlayer == TargetPlayer.Other)
                {
                    target = PlayerManager.instance.GetOtherPlayer(target);
                }
            }

            if(!(target is null) && doConsistentDamage && target.gameObject.activeInHierarchy && !coroutineStarted)
            {
                coroutineStarted = true;
                InvokeRepeating(nameof(Consistent), 0, timeBetweenDamage);
            }
            
        }
        
        public void Go()
        {
            if (doPercentageDamage) damage = target.data.maxHealth * damagePercentage;
            target.data.healthHandler.TakeDamage(damage * Vector2.up, transform.position, null, data.player, lethal, true);
            damageDone += damage;
        }
    }
}