using BossSlothsCards.Extensions;
using UnboundLib;
using UnityEngine;
using UnityEngine.UI;

namespace BossSlothsCards.MonoBehaviours
{
    public class Armor_Mono : BossSlothMonoBehaviour
    {
        private CharacterData data;

        private GameObject armorBarObj;

        private ArmorBar armorBar;

        private ArmorHandler armorHandler;

        public float maxArmor = 100;

        private void Start()
        {
            data = GetComponent<CharacterData>();
            armorBarObj = gameObject.transform.Find("WobbleObjects/ArmorBar").gameObject;
            armorBar = armorBarObj.GetOrAddComponent<ArmorBar>();
            Destroy(armorBarObj.GetComponent<HealthBar>());
            armorHandler = gameObject.AddComponent<ArmorHandler>();
            gameObject.GetOrAddComponent<ArmorDamageOverTime>();

            armorBarObj.transform.Find("Canvas/PlayerName").gameObject.SetActive(false);
            armorBarObj.transform.Find("Canvas/CrownPos").gameObject.SetActive(false);
            
            armorBarObj.transform.parent.Find("Healthbar/Canvas/PlayerName").Translate(0,0.5f,0);
            armorBarObj.transform.parent.Find("Healthbar/Canvas/CrownPos").Translate(0,1f,0);
            
            armorBarObj.transform.Find("Canvas/Image/Health").GetComponent<Image>().color = Color.cyan * 0.6f;
            armorBarObj.transform.Find("Canvas/Image/Health").GetComponent<Image>().SetAlpha(1);

            data.GetAdditionalData().maxArmor = maxArmor;
            data.GetAdditionalData().armor = maxArmor;
        }

        private void OnDestroy()
        {
            armorBarObj.transform.parent.Find("Healthbar/Canvas/PlayerName").Translate(0,-0.5f,0);
            armorBarObj.transform.parent.Find("Healthbar/Canvas/CrownPos").Translate(0,-1f,0);
            data.GetAdditionalData().maxArmor = 0;
            data.GetAdditionalData().armor = 0;
            Destroy(armorBarObj);
            Destroy(armorHandler);
            Destroy(gameObject.GetComponent<ArmorDamageOverTime>());

        }
    }
}