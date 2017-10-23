using StateEvaluationDLL.DataStructures.ReferenceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluationDLL.DataStructures
{
    public class Preference
    {
        private double blue;
        public double Blue { get { return blue; } }

        private double yellow;
        public double Yellow { get { return yellow; } }

        private double red;
        public double Red { get { return red; } }

        private TypePreference? type;
        public TypePreference? Type { get { return type; } }



        public Preference(List<byte> lessSequence, List<byte> sequence)
        {
            List<double> tempPreference = CalculateColorWeight(sequence);
            for (int i = 0; i < 3; ++i)
            {
                blue = tempPreference[0];
                yellow = tempPreference[1];
                red = tempPreference[2];
            }

            this.Normalization();
            this.CalculateTypePreference(lessSequence);
        }

        void Clear()
        {
            blue = 0;
            yellow = 0;
            red = 0;
            type = null;
        }

        int CalculateCoefficientInSeriesChoise(int countInSeries, bool firstGroup)
        {
            int coefficientCountFirstGroup_3_or_4 = 1;
            int coefficientGroup_Count_4 = 1;

            if (firstGroup && (3 <= countInSeries))
            {
                coefficientCountFirstGroup_3_or_4 = Coefficients.CountFirstGroup_3_or_4;
            }

            if (countInSeries == 4)
            {
                coefficientGroup_Count_4 = Coefficients.CountInSeries_4;
            }

            return (Coefficients.InSeries * countInSeries * coefficientCountFirstGroup_3_or_4 * coefficientGroup_Count_4);
        }

        int CalculateValueSingleChoise(int index, int countInSeries, bool firstGroup)
        {
            return (CalculateCoefficientInSeriesChoise(countInSeries, firstGroup) *
                Coefficients.Position[index] *
                Coefficients.GroupOf_2[(int)(index / 2)] *
                Coefficients.GroupOf_3[(int)(index / 3)] *
                Coefficients.GroupOf_4[(int)(index / 4)] *
                Coefficients.GroupOf_6[(int)(index / 6)]);
        }

        private List<double> CalculateColorWeight(List<byte> sequence)
        {

            bool firstGroup = true;
            int countInSeries = 0;
            ColorZones currentZone;
            ColorZones currentGroupZone = ColorZone.DetectColorZone(sequence[0]);

            List<double> tempPreference = new List<double>();
            tempPreference.Add(0);
            tempPreference.Add(0);
            tempPreference.Add(0);

            for (int i = 0; i < sequence.Count; ++i)
            {
                currentZone = ColorZone.DetectColorZone(sequence[i]);

                if (currentZone == currentGroupZone)
                {
                    ++countInSeries;
                }
                else
                {
                    firstGroup = false;
                    currentGroupZone = ColorZone.DetectColorZone(sequence[i]);
                    countInSeries = 1;
                }

                switch (currentZone)
                {
                    case ColorZones.Blue:
                        tempPreference[0] += CalculateValueSingleChoise(i, countInSeries, firstGroup);
                        break;
                    case ColorZones.Yellow:
                        tempPreference[1] += CalculateValueSingleChoise(i, countInSeries, firstGroup);
                        break;
                    case ColorZones.Red:
                        tempPreference[2] += CalculateValueSingleChoise(i, countInSeries, firstGroup);
                        break;
                }
            }
            return tempPreference;
        }

        private void Normalization()
        {
            List<double> tempPreference = CalculateColorWeight(new List<byte> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            double maxCount = tempPreference.Max();

            blue /= maxCount;
            blue = Math.Round(blue, 2);

            yellow /= maxCount;
            yellow = Math.Round(yellow, 2);

            red /= maxCount;
            red = Math.Round(red, 2);

        }

        private void CalculateTypePreference(List<byte> lessSequence)
        {
            type = TypePreference.Смешанная;
            if ((Coefficients.SIGNIFICANT_WEIGHT <= blue) ||
                ((Coefficients.LESS_SIGNIFICANT_WEIGHT <= blue) && (ColorZone.DetectColorZone(lessSequence[0]) == ColorZones.Blue)))
            {
                type = TypePreference.Синяя;
            }

            if ((Coefficients.SIGNIFICANT_WEIGHT <= yellow) ||
                ((Coefficients.LESS_SIGNIFICANT_WEIGHT <= yellow) && (ColorZone.DetectColorZone(lessSequence[0]) == ColorZones.Yellow)))
            {
                type = TypePreference.Желтая;
            }

            if ((Coefficients.SIGNIFICANT_WEIGHT <= red) ||
               ((Coefficients.LESS_SIGNIFICANT_WEIGHT <= red) && (ColorZone.DetectColorZone(lessSequence[0]) == ColorZones.Red)))
            {
                type = TypePreference.Красная;
            }
        }

        public ColorZones? MaxRequiredColor()
        {
            ColorZones? rezultZone = null;
            if ((red < blue) && (yellow < blue)) { rezultZone = ColorZones.Blue; }
            else if ((blue < yellow) && (red < yellow)) { rezultZone = ColorZones.Yellow; }
            else if ((blue < red) && (yellow < red)) { rezultZone = ColorZones.Red; }
            return rezultZone;
        }

        public override string ToString()
        {
            // return ($"Blue = {Blue}\nYellow = {Yellow}\nRed = {Red}");
            return ($"Преференция = {type}");
        }
    }
}
