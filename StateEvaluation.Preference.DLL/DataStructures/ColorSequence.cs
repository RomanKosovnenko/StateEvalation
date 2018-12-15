using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluationDLL.DataStructures
{
    public static class ColorSequence
    {
        public static List<byte> SequenceFromString(string str)
        {
            List<byte> tempSequence = new List<byte>();
            string[] seq = str.Split(',');
            foreach (var i in seq)
            {
                tempSequence.Add(Byte.Parse(i));
            }
            return tempSequence;
        }
        public static string SequenceFromListToString(List<byte> sequence)
        {
            string SequenceString = string.Empty;
            for (int i = 0; i < sequence.Count; ++i)
            {
                SequenceString += sequence[i] + ",";
            }
            SequenceString = SequenceString.Remove(SequenceString.Length - 1, 1);
            return SequenceString;
        }

        public static List<byte> SubsetReverse(List<byte> sequence, int index, int count)
        {
            List<byte> seq = sequence;
            seq.RemoveRange(index, count);
            seq.Reverse();
            return seq;
        }
    }
}
