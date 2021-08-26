using System.Reflection;
using UnityEngine;

namespace BossSlothsCards.Extensions
{
    public static class PlayerVelocityExtension
    {
        public static void AddForce(this PlayerVelocity pv, Vector2 force)
        {
            typeof(PlayerVelocity).InvokeMember("AddForce",
                BindingFlags.Instance | BindingFlags.InvokeMethod |
                BindingFlags.NonPublic, null, pv, new object[]{force, ForceMode2D.Force});
        }
    }
}