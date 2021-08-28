using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;

namespace BossSlothsCards.Extensions
{
    public class GunAmmoAdditionalData
    {
        public List<GameObject> projectiles;
        public bool destroyAllPongOnNextShot;

        public GunAmmoAdditionalData()
        {
            projectiles = new List<GameObject>();
            destroyAllPongOnNextShot = false;
        }
    }
    
    public static class GunAmmoExtension
    {
        public static readonly ConditionalWeakTable<GunAmmo, GunAmmoAdditionalData> data =
            new ConditionalWeakTable<GunAmmo, GunAmmoAdditionalData>();

        public static GunAmmoAdditionalData GetAdditionalData(this GunAmmo block)
        {
            return data.GetOrCreateValue(block);
        }

        public static void AddData(this GunAmmo block, GunAmmoAdditionalData value)
        {
            try
            {
                data.Add(block, value);
            }
            catch (Exception) { }
        }
    }
}