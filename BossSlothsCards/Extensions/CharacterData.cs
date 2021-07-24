using System;
using System.Runtime.CompilerServices;

namespace BossSlothsCards.Extensions
{
    public class CharacterDataAdditionalData
    {
        public float armor;
        public float maxArmor;

        public CharacterDataAdditionalData()
        {
            armor = 0;
            maxArmor = 0;
        }
    }
    
    public static class CharacterDataExtension
    {
        public static readonly ConditionalWeakTable<CharacterData, CharacterDataAdditionalData> data =
            new ConditionalWeakTable<CharacterData, CharacterDataAdditionalData>();

        public static CharacterDataAdditionalData GetAdditionalData(this CharacterData block)
        {
            return data.GetOrCreateValue(block);
        }

        public static void AddData(this CharacterData block, CharacterDataAdditionalData value)
        {
            try
            {
                data.Add(block, value);
            }
            catch (Exception) { }
        }
    }
}