using UnityEngine;

// "borrowed" from PCE
namespace BossSlothsCards.Utils
{
    public class PreventRecursion
    {
        private static GameObject _stopRecursion = null;

        internal static GameObject stopRecursion
        {
            get
            {
                if (_stopRecursion != null) { return _stopRecursion; }
                else
                {
                    _stopRecursion = new GameObject("StopRecursion", typeof(StopRecursion), typeof(DestroyOnUnparentAfterInitialized));
                    GameObject.DontDestroyOnLoad(_stopRecursion);

                    return _stopRecursion;
                }
            }
            set { }
        }
        internal static ObjectsToSpawn stopRecursionObjectToSpawn
        {
            get
            {
                ObjectsToSpawn obj = new ObjectsToSpawn() { };
                obj.AddToProjectile = stopRecursion;

                return obj;
            }
            set { }
        }
    }

    public class DestroyOnUnparentAfterInitialized : MonoBehaviour
    {
        private static bool initialized = false;
        private bool isOriginal = false;

        void Start()
        {
            if (!initialized) { isOriginal = true; }
        }
        void LateUpdate()
        {
            if (isOriginal) { return; }
            else if (gameObject.transform.parent == null) { Destroy(gameObject); }
        }
    }
}