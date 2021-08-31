using BossSlothsCards.Utils;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class ReduceSizeOverSeconds : MonoBehaviour
    {
        public float seconds;
        private TimeSince timeSinceSpawn;
        public float maxSize;
    }
}