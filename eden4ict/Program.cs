using System;
using System.Collections.Generic;
using System.IO;
using System.Text;



namespace eden4ict
{
    internal partial class  Program
    {
        static List<string> excludeStr = new List<string> { ".o", "~", "discharge", "." };
        static string[] delimiterChars = { " ", ",", ", " };

        static List<string> componentTypeStrList = new List<string> 
        {
            "resistor", 
            "capacitor.fr1024", 
            "capacitor.fr128", 
            "capacitor.fr8192", 
            "diode", 
            "jumper", 
            "inductor" 
        };

        [STAThread]
        static void Main(string[] args)
        {

            //Sort the file paths
            string dirPath = getDirPath();
            string resultPath = dirPath + @"\__result.csv";
            string summaryPath = dirPath + @"\__summary.csv";

            //Create collection of counters to storage components number
            List<counter> componentCounters = new List<counter>();
            List<string> startStrs = new List<string>();

            string[] fileEntries = Directory.GetFiles(dirPath);

            createCounters(componentTypeStrList, componentCounters);
            startStrs = generateStartStrings(componentCounters);

            var result = new StringBuilder();
            var summaryStr = new StringBuilder();


            #region Generate Result.csv
            result.AppendLine("index, Name, Type, normal, EN, ED, ED&EN, ");

            int i = 1;
            foreach (string fileEntry in fileEntries)
            {
                Console.WriteLine(fileEntry);
                string newLine = ProcessFile(ref i, fileEntry, startStrs, componentCounters);
                if (newLine != null)
                    result.AppendLine(newLine);
            }
            Console.WriteLine("Files Processed: {0}", fileEntries.Length);

            File.WriteAllText(resultPath, result.ToString());
            #endregion

            #region Generate Summary.csv
            summaryStr.AppendLine(string.Format("{0},{1},{2}, {3}, {4}, {5}", "Component", "Normal", "EN", "ED", "ED&EN", "Total"));
            foreach (var  counter in componentCounters)
            {
                summaryStr.AppendLine(string.Format("{0},{1},{2}, {3}, {4}, {5}", counter.Name, counter.normal, counter.en, counter.ed, counter.eden, counter.total));
            }

            File.WriteAllText(summaryPath, summaryStr.ToString());

            Console.WriteLine("File processing completed, open summary file? Type 'no' to exit without Opening.");
            string openfile = Console.ReadLine();
            if (openfile != "no" && openfile != "NO" && openfile != "No")
                OpenWithDefaultProgram(summaryPath);

            #endregion


        }


    }
}
