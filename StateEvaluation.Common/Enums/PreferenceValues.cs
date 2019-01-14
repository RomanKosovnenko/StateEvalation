
using StateEvaluation.Common.Constants;
using System.Collections.Generic;

namespace StateEvaluation.Common.Enums
{
    public static class PreferenceValues
    {
        public static string Red = "Красная";
        public static string Blue = "Синяя";
        public static string Yellow = "Желтая";
        public static string Gray = "Смешанная";
        public static IEnumerable<string> Preferences = new string[] { "Синяя", "Желтая", "Красная", "Смешанная" };
        public static IEnumerable<string> ShortColorsNumbersList = new string[] { "3", "7", "11" };
        public static IEnumerable<string> ColorsNumbersList = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
    }
}
