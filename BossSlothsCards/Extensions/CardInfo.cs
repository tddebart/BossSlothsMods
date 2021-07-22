using System;
using System.Runtime.CompilerServices;


// Class from PCE(https://github.com/pdcook/PCE)
namespace BossSlothsCards.Extensions
{
    // ADD FIELDS TO BLOCK
    [Serializable]
    public class CardInfoAdditionalData
    {
        public bool canBeReassigned;
        public bool isRandom;

        public CardInfoAdditionalData()
        {
            canBeReassigned = true;
            isRandom = false;
        }
    }
    public static class CardInfoExtension
    {
        public static readonly ConditionalWeakTable<CardInfo, CardInfoAdditionalData> data =
            new ConditionalWeakTable<CardInfo, CardInfoAdditionalData>();

        public static CardInfoAdditionalData GetAdditionalData(this CardInfo block)
        {
            return data.GetOrCreateValue(block);
        }

        public static void AddData(this CardInfo block, CardInfoAdditionalData value)
        {
            try
            {
                data.Add(block, value);
            }
            catch (Exception) { }
        }
    }
}
