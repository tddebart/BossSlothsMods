using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BossSlothsCards.MonoBehaviours
{
    public class ChargeCounter : MonoBehaviour
    {
        public string text;
        public TextMeshProUGUI uGUI;
        public Image backgroundImage;
        public Color backgroundColor;
        public GameObject chargeCounterObj;
        public GameObject canvasObj;

        public virtual void Start()
        {
            chargeCounterObj = Instantiate(BossSlothCards.EffectAsset.LoadAsset<GameObject>("ChargeCounter"),transform.Find("Limbs/ArmStuff"));
            var incative = chargeCounterObj.AddComponent<FollowInactiveHand>();
            incative.leftHand = transform.Find("Limbs/ArmStuff/Arm_Left/Arm/Elbow/Hand").gameObject;
            incative.rightHand = transform.Find("Limbs/ArmStuff/Arm_Right/Arm/Elbow/Hand").gameObject;
            incative.offSet = new Vector2(0, 0.85f);
            chargeCounterObj.transform.localPosition = Vector3.zero;
            uGUI = chargeCounterObj.transform.Find("Canvas/Text").GetComponent<TextMeshProUGUI>();
            backgroundImage = chargeCounterObj.transform.Find("Canvas/Background").GetComponent<Image>();
            canvasObj = chargeCounterObj.GetComponentInChildren<Canvas>().gameObject;
        }

        public virtual void Update()
        {
            uGUI.text = text;
            backgroundImage.color = backgroundColor;
            var transformLocalPosition = canvasObj.transform.localPosition;
            transformLocalPosition.x = chargeCounterObj.transform.localPosition.x/2;
            canvasObj.transform.localPosition = transformLocalPosition;
        }
    }
}