using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StateEvaluationDLL.DataStructures;

//using Microsoft.Office.Interop.Excel;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < 180; i++)
            {
                list.Add(Guid.NewGuid().ToString());
            }
            string filePath = System.IO.Directory.GetCurrentDirectory() + "\\" + "20.xlsx";


            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook wb = ExcelApp.Workbooks.Open(filePath, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            Microsoft.Office.Interop.Excel.Worksheet sh = (Microsoft.Office.Interop.Excel.Worksheet)wb.Sheets["Кольорова преференція початок"];

            Range excelRange = sh.UsedRange;
            int i0 = 3, j0 = 7;

            Range xlRng = sh.UsedRange;

            Object arr = xlRng.Value;


            List<List<Byte>> lessSequences = new List<List<byte>>();
            List<List<Byte>> Sequences = new List<List<byte>>();
            List<string> pref = new List<string>();
            List<string> names = new List<string>();
            List<string> dates = new List<string>();
            for (int i = i0; i < xlRng.Rows.Count; i += 3)
            {
                names.Add(xlRng[i, 2].Value2);
                //dates.Add(((DateTime)xlRng[i, 3].Date).ToShortDateString());
                lessSequences.Add(new List<byte>() { (byte)xlRng[i,7].Value2, (byte)xlRng[i, 8].Value2, (byte)xlRng[i, 9].Value2 });
                Sequences.Add(new List<byte>() { (byte)xlRng[i + 1, 7].Value2, (byte)xlRng[i + 1, 8].Value2, (byte)xlRng[i+1, 9].Value2, (byte)xlRng[i+1, 10].Value2,
                                                 (byte)xlRng[i + 1, 11].Value2, (byte)xlRng[i + 1, 12].Value2, (byte)xlRng[i+1, 13].Value2, (byte)xlRng[i+1, 14].Value2,
                                                 (byte)xlRng[i + 1, 15].Value2, (byte)xlRng[i + 1, 16].Value2, (byte)xlRng[i+1, 17].Value2, (byte)xlRng[i+1, 18].Value2});
            }
            List<Preference> prefRes = new List<Preference>();
            for (var i = 0; i < Sequences.Count; i++)
            {
                prefRes.Add(new Preference(lessSequences[i], Sequences[i]));
            }

            foreach (var preference in prefRes)
            {
                pref.Add(preference.Type.ToString());
            }
            foreach (object s in (Array)excelRange)
            {
                Console.WriteLine(s);
            }

            for (int i = 2; i <= excelRange.Count + 1; i++)
            {
                string values = sh.Cells[i, 2].ToString();
            }

            Console.ReadLine();
        }
    }
}
