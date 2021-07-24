using System.Reflection;
using BossSlothsCards.Extensions;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class ArmorHandler : MonoBehaviour
    {
        private CharacterData data;
        private HealthHandler healthHandler;
        private CharacterStatModifiers characterStatModifier;

        public float regeneration = 10;
        public bool armorIsZero;

        private void Awake()
        {
            data = GetComponent<CharacterData>();
            healthHandler = GetComponent<HealthHandler>();
            characterStatModifier = GetComponent<CharacterStatModifiers>();
        }

        private void Update()
        {
            if (data.GetAdditionalData().armor < 0) data.GetAdditionalData().armor = 0;
        }

        public void CallCallTakeDamage(float damage, Vector2? position = null, GameObject damagingWeapon = null,
            Player damagingPlayer = null, bool lethal = true)
        {
            CallTakeDamage(damage,position, damagingWeapon, damagingPlayer, lethal);
        }

        public void CallTakeDamage(float damage, Vector2? position = null, GameObject damagingWeapon = null, Player damagingPlayer = null, bool lethal = true)
        {
            if (damage == 0 || data.block.IsBlocking() || data.GetAdditionalData().armor <= 0) return;
            if (position == null) position = Vector2.zero;
            data.view.RPC("RPCA_SendTakeDamageArmor", RpcTarget.All, new object[]
            {
                damage,
                position,
                lethal,
                (damagingPlayer != null) ? damagingPlayer.playerID : -1
            });

        }
        
        [PunRPC]
        public void RPCA_SendTakeDamageArmor(float damage, Vector2 position, bool lethal = true, int playerID = -1)
        {
            if (damage == 0) return;

            var playerWithID = PlayerManager.instance.GetPlayerWithID(playerID);
            GameObject damagingWeapon = null;
            if (playerWithID)
            {
                damagingWeapon = playerWithID.data.weaponHandler.gun.gameObject;
            }
            TakeDamage(damage, position, damagingWeapon, playerWithID, lethal, true);
        }

        public void TakeDamage(float damage, Vector2 position, GameObject damagingWeapon = null, Player damagingPlayer = null,
            bool lethal = true, bool ignoreBlock = false)
        {
            if (damage == 0) return;
            TakeDamage(damage, position, Color.cyan, damagingWeapon, damagingPlayer, lethal, ignoreBlock);
        }

        public void TakeDamage(float damage, Vector2 position, Color dmgColor, GameObject damagingWeapon = null,
            Player damagingPlayer = null, bool lethal = true, bool ignoreBlock = false)
        {
            if (damage == 0)
            {
                return;
            }
            if (!data.isPlaying)
            {
                return;
            }
            if (!(bool)Traverse.Create(data.playerVel).Field("simulated").GetValue())
            {
                return;
            }
            if (data.dead)
            {
                return;
            }
            if (data.block.IsBlocking() && !ignoreBlock)
            {
                return;
            }
            if (dmgColor == Color.black)
            {
                dmgColor = Color.white * 0.85f;
            }

            if (characterStatModifier.secondsToTakeDamageOver == 0f)
            {
                DoDamage(damage, position, dmgColor, damagingWeapon, damagingPlayer, lethal, ignoreBlock);
            }
            else
            {
                GetComponent<ArmorDamageOverTime>().DoDamageOverTimeVoid(new Vector2(damage, 0), position, characterStatModifier.secondsToTakeDamageOver, 0.25f, dmgColor, healthHandler.soundDamagePassive, damagingWeapon, damagingPlayer, lethal);
            }
        }

        public void DoDamage(float damage, Vector2 position, Color blinkColor, GameObject damagingWeapon = null, Player damagingPlayer = null, bool lethal = true, bool ignoreBlock = false)
        {
            
            if (damage == 0)
            {
                return;
            }
            if (!data.isPlaying)
            {
                return;
            }
            if (data.dead)
            {
                return;
            }
            if (data.block.IsBlocking() && !ignoreBlock)
            {
                return;
            }
            if (healthHandler.isRespawning)
            {
                return;
            }
            if (damage > data.GetAdditionalData().armor)
            {
                armorIsZero = true;
                healthHandler.CallTakeDamage(new Vector2(damage-data.GetAdditionalData().armor, 0), Vector2.zero);
                damage = data.GetAdditionalData().armor;
                armorIsZero = false;
            }
            if (damagingPlayer)
            {
                damagingPlayer.GetComponent<CharacterStatModifiers>().DealtDamage(new Vector2(damage, 0), damagingPlayer != null && damagingPlayer.transform.root == transform, data.player);
            }
            StopAllCoroutines();
            typeof(HealthHandler).InvokeMember("DisplayDamage",
                BindingFlags.Instance | BindingFlags.InvokeMethod |
                BindingFlags.NonPublic, null, healthHandler, new object[]{blinkColor});
            data.lastSourceOfDamage = damagingPlayer;
            data.GetAdditionalData().armor -= damage;
            characterStatModifier.WasDealtDamage(new Vector2(damage, 0), damagingPlayer != null && damagingPlayer.transform.root == transform);
        }

        public void Heal(float healAmount)
        {
            if (healAmount == 0f || data.GetAdditionalData().armor == data.GetAdditionalData().maxArmor)
            {
                return;
            }
            data.GetAdditionalData().armor += healAmount;
            data.GetAdditionalData().armor = Mathf.Clamp(data.GetAdditionalData().armor, float.NegativeInfinity, data.GetAdditionalData().maxArmor);
        }
    }
}