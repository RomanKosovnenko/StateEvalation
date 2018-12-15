using StateEvaluationDLL.DataStructures.ReferenceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluationDLL.DataStructures
{
    public static class ColorZone
    {
        public const byte BLUE_ZONE_START_INDEX = 1;
        public const byte BLUE_ZONE_END_INDEX = 4;
        public const byte YELLOW_ZONE_START_INDEX = 5;
        public const byte YELLOW_ZONE_END_INDEX = 8;
        public const byte RED_ZONE_START_INDEX = 9;
        public const byte RED_ZONE_END_INDEX = 12;

        public static ColorZones DetectColorZone(Byte colorNumber)
        {
            if ((colorNumber >= BLUE_ZONE_START_INDEX) && (colorNumber <= BLUE_ZONE_END_INDEX))
            {
                return (ColorZones.Blue);
            }
            else if ((colorNumber >= YELLOW_ZONE_START_INDEX) && (colorNumber <= YELLOW_ZONE_END_INDEX))
            {
                return (ColorZones.Yellow);
            }
            else if ((colorNumber >= RED_ZONE_START_INDEX) && (colorNumber <= RED_ZONE_END_INDEX))
            {
                return (ColorZones.Red);
            }
            throw new NotImplementedException();
        }
    }
}
