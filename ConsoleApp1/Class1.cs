//using Microsoft.Office.Interop.Excel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using StateEvaluationDLL.DataStructures;

////using Microsoft.Office.Interop.Excel;

//namespace ConsoleApp1
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            string filePath = System.IO.Directory.GetCurrentDirectory() + "\\" + "pref.xlsx";


//            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
//            ExcelApp.Visible = true;
//            Microsoft.Office.Interop.Excel.Workbook wb = ExcelApp.Workbooks.Open(filePath, Missing.Value, false, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

//            Microsoft.Office.Interop.Excel.Worksheet sh = (Microsoft.Office.Interop.Excel.Worksheet)wb.Sheets["Кольорова преференція початок"];

//            Range excelRange = sh.UsedRange;


//            Range xlRng = sh.UsedRange;

//            Object arr = xlRng.Value;


//            List<List<Byte>> lessSequences = new List<List<byte>>();
//            List<List<Byte>> Sequences = new List<List<byte>>();
//            List<string> pref = new List<string>();
//            List<string> names = new List<string>();
//            List<string> dates = new List<string>();
//            int i0 = 7, j0 = 7;
//            for (int i = i0; i <= xlRng.Rows.Count; i += 1)
//            {
//                if (i == 110)
//                {
//                    i = 112;
//                    continue;
//                }
//                if (xlRng[i, 2].Value2 == "ПІБ")
//                {
//                    continue;
//                }
//                names.Add(xlRng[i, 2].Value2);
//                //dates.Add(((DateTime)xlRng[i, 3].Date).ToShortDateString());
//                lessSequences.Add(new List<byte>() { (byte)xlRng[i, 12].Value2, (byte)xlRng[i, 13].Value2, (byte)xlRng[i, 14].Value2 });
//                Sequences.Add(new List<byte>() { (byte)xlRng[i, 16].Value2, (byte)xlRng[i, 17].Value2, (byte)xlRng[i, 18].Value2, (byte)xlRng[i, 19].Value2,
//                                                 (byte)xlRng[i, 20].Value2, (byte)xlRng[i, 21].Value2, (byte)xlRng[i, 22].Value2, (byte)xlRng[i, 23].Value2,
//                                                 (byte)xlRng[i, 24].Value2, (byte)xlRng[i, 25].Value2, (byte)xlRng[i, 26].Value2, (byte)xlRng[i, 27].Value2});
//            }
//            List<Preference> prefRes = new List<Preference>();
//            for (var i = 0; i < Sequences.Count; i++)
//            {
//                //if(i%12 == 0) prefRes.Add(null);
//                prefRes.Add(new Preference(lessSequences[i], Sequences[i]));
//            }
//            int i1 = 0;
//            foreach (var preference in prefRes)
//            {
//                if (i1 % 12 == 0) pref.Add("");
//                pref.Add(preference.Type.ToString());
//                ++i1;
//            }
//            foreach (object s in (Array)excelRange)
//            {
//                Console.WriteLine(s);
//            }

//            for (int i = 2; i <= excelRange.Count + 1; i++)
//            {
//                string values = sh.Cells[i, 2].ToString();
//            }

//            Console.ReadLine();
//        }
//    }
//}
