using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace CardBarPatch
{
    // Code from RWF
    public static class CardBarHandlerExtensions
    {
        public static void Rebuild(this CardBarHandler instance) {
            while (instance.transform.childCount > 3) {
                GameObject.DestroyImmediate(instance.transform.GetChild(3).gameObject);
            }

            const int numPlayers = 4;
            var barGo = instance.transform.GetChild(0).gameObject;

            var deltaY = -CardBarPatch.VerticalDistance;
            var cardBars = new List<CardBar>();

            for (int i = 0; i < numPlayers; i++) {
                var newBarGo = GameObject.Instantiate(barGo, instance.transform);
                newBarGo.SetActive(true);
                newBarGo.name = "Bar" + (i + 1);
                newBarGo.transform.localScale = Vector3.one;
                newBarGo.transform.localPosition = barGo.transform.localPosition + new Vector3(0, deltaY * i, 0);

                cardBars.Add(newBarGo.GetComponent<CardBar>());
            }

            barGo.SetActive(false);
            instance.transform.GetChild(1).gameObject.SetActive(false);

            instance.SetFieldValue("cardBars", cardBars.ToArray());
        }
    }
}