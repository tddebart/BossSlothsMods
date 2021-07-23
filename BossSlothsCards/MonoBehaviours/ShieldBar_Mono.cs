using BossSlothsCards.Extensions;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class ShieldBar_Mono : MonoBehaviour
    {
        private CharacterData data;

        private GameObject shieldBar;

        void Start()
        {
            data = GetComponent<CharacterData>();
            shieldBar = gameObject.transform.Find("WobbleObjects/ShieldBar").gameObject;
        }
        
        void Update()
        {
            UnityEngine.Debug.LogWarning(data.GetAdditionalData().shield);
            if (Input.GetKeyDown(KeyCode.K)) data.GetAdditionalData().shield--;
        }
    }
}