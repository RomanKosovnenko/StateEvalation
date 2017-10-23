using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluationDLL.DataStructures.ReferenceData
{
    public static class Coefficients
    {
        public const int COLOR_COUNT = 12;
        public const double SIGNIFICANT_WEIGHT = 0.22;
        public const double LESS_SIGNIFICANT_WEIGHT = 0.18;
        public static byte InSeries = 1;
        public static int CountInSeries_4 = 3;
        public static int CountFirstGroup_3_or_4 = 2;
        public static byte[] Position = new byte[] { 7, 6, 5, 4, 3, 2, 1, 1, 1, 1, 1, 1 };
        public static byte[] GroupOf_2 = new byte[] { 1, 1, 1, 1, 1, 1 };
        public static byte[] GroupOf_3 = new byte[] { 4, 3, 2, 1 };
        public static byte[] GroupOf_4 = new byte[] { 3, 2, 1 };
        public static byte[] GroupOf_6 = new byte[] { 2, 1 };




        public static void ChangeKoeficients(List<byte> koef_of_position,
                                             List<byte> koef_group_of_two,
                                             List<byte> koef_group_of_three,
                                             List<byte> koef_group_of_four,
                                             List<byte> koef_group_of_six)
        {
            for (int i = 0; i < koef_of_position.Count; ++i)
            {
                Position[i] = koef_of_position[i];
            }
            for (int i = 0; i < koef_group_of_two.Count; ++i)
            {
                GroupOf_2[i] = koef_group_of_two[i];
            }
            for (int i = 0; i < koef_group_of_three.Count; ++i)
            {
                GroupOf_3[i] = koef_group_of_three[i];
            }
            for (int i = 0; i < koef_group_of_four.Count; ++i)
            {
                GroupOf_4[i] = koef_group_of_four[i];
            }
            for (int i = 0; i < koef_group_of_six.Count; ++i)
            {
                GroupOf_6[i] = koef_group_of_six[i];
            }
        }

    }
}
