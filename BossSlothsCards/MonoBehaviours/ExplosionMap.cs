using System.Collections.Generic;
using Photon.Pun;
using UnboundLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossSlothsCards.MonoBehaviours
{
    public class ExplosionMap : BossSlothMonoBehaviour
    {
        private static bool Condition(GameObject obj)
        {
            return obj.activeInHierarchy &&obj.GetComponent<SpriteRenderer>() && !obj.name.Contains("Color") && !obj.name.Contains("Lines") && !obj.name.Contains("Grid");
        }

        [PunRPC]
        public void RPCA_ExplodeBlock(int index)
        {
            var scene = SceneManager.GetSceneAt(1);
            if (!scene.IsValid())  return;
            var objectsArray = scene.GetRootGameObjects()[0].GetComponentsInChildren<SpriteRenderer>(false);
            if (objectsArray == null)  return;
            var objects = new List<SpriteRenderer>();
            foreach (var obj in objectsArray)
            {
                if (Condition(obj.gameObject))
                {
                    objects.Add(obj);
                }
            }
            var randomObject = objects[index];

            var pieces = BossSlothCards.EffectAsset.LoadAsset<GameObject>("Pieces");
            var parent = randomObject.transform.parent;
            var _pieces = Instantiate(pieces, parent);
            var transform1 = randomObject.transform;
            _pieces.transform.position = transform1.position;
            _pieces.transform.rotation = transform1.rotation;
            randomObject.gameObject.SetActive(false);
            this.ExecuteAfterSeconds(6, () =>
            {
                Destroy(_pieces);
            });
        }
    }
}