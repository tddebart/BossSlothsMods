using System;
using Sonigon;
using SoundImplementation;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class SpecialEffectFrontGun : MonoBehaviour
    {
        private CharacterData data;

        private Transform particleTransform;
        
        private SoundEvent soundSpawn;
        private SoundEvent soundShoot;

        private bool alreadyActivated;

        private ParticleSystem[] parts;

        public bool Active;

        public Color particleColor;
        
        public void Start()
        {
            data = GetComponentInParent<CharacterData>();
            
            var empower = (GameObject)Resources.Load("0 cards/Empower");
            var empowerObj = empower.GetComponent<CharacterStatModifiers>().AddObjectToPlayer;
            soundSpawn = empowerObj.GetComponent<Empower>().soundEmpowerSpawn;
            soundShoot = empowerObj.GetComponent<Empower>().addObjectToBullet.GetComponent<SoundUnityEventPlayer>()
                .soundStart;
            
            var particleObj = empowerObj.transform.GetChild(0).gameObject;
            particleTransform = Instantiate(particleObj, transform).transform;
            parts = GetComponentsInChildren<ParticleSystem>();
            foreach (var system in parts)
            {
                var systemMain = system.main;
                systemMain.startColor = particleColor;
            }
            var gun = data.weaponHandler.gun;
            gun.ShootPojectileAction = (Action<GameObject>)Delegate.Combine(gun.ShootPojectileAction, new Action<GameObject>(Attack));
        }

        private void Attack(GameObject projectile)
        {
            if (Active)
            {
                SoundManager.Instance.PlayAtPosition(soundShoot, SoundManager.Instance.GetTransform(), transform);
            }
        }

        public void Update()
        {
            if (Active)
            {
                var transform1 = data.weaponHandler.gun.transform;
                particleTransform.position = transform1.position;
                particleTransform.rotation = transform1.rotation;
                if (!alreadyActivated)
                {
                    SoundManager.Instance.PlayAtPosition(soundSpawn, SoundManager.Instance.GetTransform(), transform);
                    foreach (var system in parts)
                    {
                        system.Play();
                    }

                    alreadyActivated = true;
                }
            } else if (alreadyActivated)
            {
                foreach (var sytem in parts)
                {
                    sytem.Stop();
                }

                alreadyActivated = false;
            }
        }
    }
}