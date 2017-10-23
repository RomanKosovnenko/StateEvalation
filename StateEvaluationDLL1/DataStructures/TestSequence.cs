using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluationDLL.DataStructures
{
    public class TestSequence
    {
        private List<byte> lessSequence;
        public List<byte> LessSequence { get { return lessSequence; } }

        private List<byte> sequence;
        public List<byte> Sequence { get { return sequence; } }

        private Preference preference;
        public Preference Preference { get { return preference; } }

        private DirectionSelection? blueDirection;
        public DirectionSelection? BlueDirection { get { return blueDirection; } }

        private DirectionSelection? yellowDirection;
        public DirectionSelection? YellowDirection { get { return yellowDirection; } }

        private DirectionSelection? redDirection;
        public DirectionSelection? RedDirection { get { return redDirection; } }

        private DirectionSelection? sequenceDirection;
        public DirectionSelection? SequenceDirection { get { return sequenceDirection; } }


        public TestSequence(string lessSequence, string sequence) : this(ColorSequence.SequenceFromString(lessSequence), ColorSequence.SequenceFromString(sequence)) { }
        public TestSequence(List<byte> lessSequence, List<byte> sequence)
        {
            this.lessSequence = lessSequence;
            this.sequence = sequence;
            this.preference = new Preference(this.lessSequence, this.sequence);
            SetAllColorDirection();
            //  SetSequenceDirection();
        }

        void Clear()
        {
            blueDirection = null;
            yellowDirection = null;
            redDirection = null;
            sequenceDirection = null;
        }

        private void SetAllColorDirection()
        {
            blueDirection = ColorGroupDirection(Sequence.FindAll(i => (ColorZone.BLUE_ZONE_START_INDEX <= i && i <= ColorZone.BLUE_ZONE_END_INDEX)));
            yellowDirection = ColorGroupDirection(Sequence.FindAll(i => (ColorZone.YELLOW_ZONE_START_INDEX <= i && i <= ColorZone.YELLOW_ZONE_END_INDEX)));
            redDirection = ColorGroupDirection(Sequence.FindAll(i => (ColorZone.RED_ZONE_START_INDEX <= i && i <= ColorZone.RED_ZONE_END_INDEX)));
            sequenceDirection = ColorAllDirection(sequence);
        }

        private DirectionSelection ColorGroupDirection(List<byte> colorGroup)
        {
            int countClockwise = 0;
            int countAntiClockwise = 0;

            for (int i = 1; i < colorGroup.Count; ++i)
            {
                if (colorGroup[i - 1] < colorGroup[i]) { ++countClockwise; }
                else { ++countAntiClockwise; }
            }

            if (countClockwise == colorGroup.Count - 1) { return DirectionSelection.Clockwise; }
            else if (countAntiClockwise == colorGroup.Count - 1) { return DirectionSelection.AntiClockwise; }
            else { return DirectionSelection.Mixed; }
        }


        private DirectionSelection ColorAllDirection(List<byte> colorGroup)
        {
            int countClockwise = 0; // количество преференций по часовой стрелке
            int countAntiClockwise = 0; // количество преференций против часовой стрелки
            int difference = 0; // разница между текущей и следующей преференцией
            int undefined = 0; // количество неопределенных преференций
            int countColors = colorGroup.Count; // количество преференций 
            for (int i = 0; i < countColors - 1; ++i)
            {
                difference = colorGroup[i + 1] - colorGroup[i];
                if (((difference > 0) && (difference < countColors / 2)) ||
                    ((difference < 0) && (difference > countColors / 2)))
                    countClockwise++;
                else if (difference == countColors / 2)
                    undefined++;
                else
                    countAntiClockwise++;
            }

            int groupClockwise = 0; // количество групповых преференций по часовой стрелке
            int groupAnticlockwise = 0; // количество групповых преференций против часовой стрелки
            if (blueDirection == DirectionSelection.Clockwise) groupClockwise += 1;
            else if (blueDirection == DirectionSelection.AntiClockwise) groupAnticlockwise -= 1;

            if (redDirection == DirectionSelection.Clockwise) groupClockwise += 1;
            else if (redDirection == DirectionSelection.AntiClockwise) groupAnticlockwise -= 1;

            if (yellowDirection == DirectionSelection.Clockwise) groupClockwise += 1;
            else if (yellowDirection == DirectionSelection.AntiClockwise) groupAnticlockwise -= 1;

            if (groupClockwise >= 2 || (countClockwise - countAntiClockwise) >= (countColors * 2 / 3)) return DirectionSelection.Clockwise;
            if (groupAnticlockwise <= -2 || (countAntiClockwise - countClockwise) >= (countColors * 2 / 3)) return DirectionSelection.AntiClockwise;
            return DirectionSelection.Mixed;
        }
        //private void SetSequenceDirection()
        //{
        //    int countClockwise = 0;
        //    int countAntiClockwise = 0;
        //    bool currentDirection = ClockDirection(sequence[0], sequence[1]);

        //    for (int i = 2; i < Sequence.Count; ++i)
        //    {
        //        if(currentDirection) { ++countClockwise; }
        //        else { ++countAntiClockwise; }
        //        if (currentDirection != ClockDirection(sequence[i - 1], sequence[i])  { currentDirection = !currentDirection; }
        //    }

        //}

        //private bool ClockDirection(byte color1, byte color2)
        //{
        //    const byte BOTTOM_COLOR = 1;
        //    const byte TOP_COLOR = 7;

        //    List<byte> CircleFirstPart = new List<byte>();
        //    List<byte> CircleSecondPart = new List<byte>();

        //    byte currentColor = TOP_COLOR;
        //    for (byte i = 0; i < (Coefficients.COLOR_COUNT / 2); ++i)
        //    {
        //        CircleFirstPart.Add(currentColor);
        //        currentColor = ColorOperations.NextColorNumber(currentColor, Coefficients.COLOR_COUNT);
        //    }

        //    currentColor = BOTTOM_COLOR;
        //    for (byte i = 0; i < (Coefficients.COLOR_COUNT / 2); ++i)
        //    {
        //        CircleFirstPart.Add(currentColor);
        //        currentColor = ColorOperations.NextColorNumber(currentColor,Coefficients.COLOR_COUNT);
        //    }


        //    if (((color1 < color2) &&
        //            (CircleFirstPart.Exists(i => i == color1) && CircleFirstPart.Exists(i => i == color2)) ||
        //            (CircleSecondPart.Exists(i => i == color1) && CircleSecondPart.Exists(i => i == color2)))||
        //       ((color1 > color2) &&
        //       ()
        //       ))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private int ClockIndex(byte element, byte bottom, byte top, int maxColorCount)
        //{


        //}

        public override string ToString()
        {
            string str = "";
            foreach (var i in Sequence)
            { str += i + ","; }
            return str.Remove(str.Length - 1, 1);
        }

        //public static void WriteJSONToFile(string path, List<byte> Sequence)
        //{
        //    FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
        //    StreamWriter sw = new StreamWriter(fs);
        //    String fileContent = SerializeToJson(Sequence);
        //    sw.Write(fileContent);
        //    sw.Close();
        //}

        //public static String SerializeToJson(List<byte> Sequence)
        //{
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string strJson = serializer.Serialize(Sequence);
        //    return strJson;
        //}
    }
}
