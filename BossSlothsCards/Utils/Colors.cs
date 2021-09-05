using System.Collections;
using UnityEngine;

namespace BossSlothsCards.Utils
{
    public static class Colors
    {
        public enum HueColorNames{
            Lime,
            Green,
            Aqua,
            Blue,
            Navy,
            Purple,
            Pink,
            Red,
            Orange,
            Yellow
        }
 
        private static Hashtable hueColourValues = new Hashtable{
            { HueColorNames.Lime,     new Color32( 166 , 254 , 0, 255 ) },
            { HueColorNames.Green,     new Color32( 0 , 254 , 111, 255 ) },
            { HueColorNames.Aqua,     new Color32( 0 , 201 , 254, 255 ) },
            { HueColorNames.Blue,     new Color32( 0 , 122 , 254, 255 ) },
            { HueColorNames.Navy,     new Color32( 60 , 0 , 254, 255 ) },
            { HueColorNames.Purple, new Color32( 143 , 0 , 254, 255 ) },
            { HueColorNames.Pink,     new Color32( 232 , 0 , 254, 255 ) },
            { HueColorNames.Red,     new Color32( 254 , 9 , 0, 255 ) },
            { HueColorNames.Orange, new Color32( 254 , 161 , 0, 255 ) },
            { HueColorNames.Yellow, new Color32( 254 , 224 , 0, 255 ) },
        };
 
        public static Color HueColourValue( HueColorNames color ) {
            return (Color32) hueColourValues [color];
        }
    }
}