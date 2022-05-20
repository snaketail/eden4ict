using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace eden4ict
{
    internal class Program
    {
        static List<string> excludeStr = new List<string> { ".o", "~", "discharge", "." };
        static List<string> startStr = new List<string> { "resistor", "capacitor.fr1024", "capacitor.fr128", "capacitor.fr8192", "capacitor", "diode", "jumper", "inductor" };
        static string[] delimiterChars = { " ", ",", ", " };

        [STAThread]
        static void Main(string[] args)
        {
            counter counter = new counter();

            bool inProjectfolder = false;
            string currentDir = Environment.CurrentDirectory;
            string dirPath;

            foreach (var dir in Directory.GetDirectories(currentDir))
                if (Path.GetFileName(dir) == "analog")
                    inProjectfolder = true;

            if (inProjectfolder)
                 dirPath = Environment.CurrentDirectory + @"\analog";
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    dirPath = fbd.SelectedPath;
                else
                {
                    dirPath = null;
                    Environment.Exit(0);
                }
            }

            string resultPath = dirPath + @"\__result.csv";
            string summaryPath = dirPath + @"\__summary.csv";
            string[] fileEntries = Directory.GetFiles(dirPath);

            var result = new StringBuilder();
            var summaryStr = new StringBuilder();

            result.AppendLine("index, Name, Type, normal, EN, ED, ED&EN, ");

            int i = 1;
            foreach (string fileEntry in fileEntries)
            {
                Console.WriteLine(fileEntry);
                string newLine = ProcessFile(ref i, fileEntry, counter);
                if (newLine != null)
                    result.AppendLine(newLine);
            }
            Console.WriteLine("Files Processed: {0}", fileEntries.Length);


            summaryStr.AppendLine(string.Format("{0},{1},{2}, {3}, {4}, {5}", "Component", "Normal", "EN", "ED", "ED&EN", "Total"));
            summaryStr.AppendLine(string.Format("{0},{1},{2}, {3}, {4}, {5}", "Resitor", counter.res.normal, counter.res.en, counter.res.ed, counter.res.eden, counter.res.total));
            summaryStr.AppendLine(string.Format("{0},{1},{2}, {3}, {4}, {5}", "capacitor", counter.cap.normal, counter.cap.en, counter.cap.ed, counter.cap.eden, counter.cap.total));
            summaryStr.AppendLine(string.Format("{0},{1},{2}, {3}, {4}, {5}", "inductor", counter.ind.normal, counter.ind.en, counter.ind.ed, counter.ind.eden, counter.ind.total));
            summaryStr.AppendLine(string.Format("{0},{1},{2}, {3}, {4}, {5}", "jumper", counter.jumper.normal, counter.jumper.en, counter.jumper.ed, counter.jumper.eden, counter.jumper.total));
            summaryStr.AppendLine(string.Format("{0},{1},{2}, {3}, {4}, {5}", "diode", counter.diode.normal, counter.diode.en, counter.diode.ed, counter.diode.eden, counter.diode.total));

            File.WriteAllText(resultPath, result.ToString());
            File.WriteAllText(summaryPath, summaryStr.ToString());

            Console.WriteLine("File processing completed, open summary file? Type 'no' to exit without Opening.");
            string openfile = Console.ReadLine();
            if (openfile != "no" && openfile != "NO" && openfile != "No")
                OpenWithDefaultProgram(summaryPath);
        }

        static string ProcessFile(ref int index, string file, counter _counter)
        {
            string result = null;
            string name = Path.GetFileName(file);
            if (!excludeStr.Any(name.Contains))
            {
                foreach (string line in File.ReadLines(file))
                {
                    if (startStr.Any(line.StartsWith))
                    {
                        result = checkEdEn(ref index, name, line, _counter);
                        break;
                    }
                }
            }
            return result;
        }

        static string checkEdEn(ref int index, string name, string line, counter _counter)
        {
            string result;
            string type;
            string edStr=" ", enStr=" ", edenStr=" ", normalStr=" ";
            string[] words = line.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
            bool ed = words.Contains("ed");
            bool en = words.Contains("en");
            bool eden = ed && en;

            if (eden)
            {
                edenStr = "yes";
                switch (words[0])
                {
                    case "resistor":
                        type = "resistor";
                        _counter.res.eden++;
                        break;
                    case "capacitor":
                        type = "capacitor";
                        _counter.cap.eden++;
                        break;
                    case "diode":
                        type = "diode";
                        _counter.diode.eden++;
                        break;
                    case "jumper":
                        type = "jumper";
                        _counter.jumper.eden++;
                        break;
                    case "inductor":
                        type = "inductor";
                        _counter.ind.eden++;
                        break;
                    default:
                        type = "Null";
                        break;
                }
            }
            else if (ed)
            {
                edStr = "Yes";
                switch (words[0])
                {
                    case "resistor":
                        type = "resistor";
                        _counter.res.ed++;
                        break;
                    case "capacitor":
                        type = "capacitor";
                        _counter.cap.ed++;
                        break;
                    case "diode":
                        type = "diode";
                        _counter.diode.ed++;
                        break;
                    case "jumper":
                        type = "jumper";
                        _counter.jumper.ed++;
                        break;
                    case "inductor":
                        type = "inductor";
                        _counter.ind.ed++;
                        break;
                    default:
                        type = "Null";
                        break;
                }
            }
            else if (en)
            {
                enStr = "Yes";
                switch (words[0])
                {
                    case "resistor":
                        type = "resistor";
                        _counter.res.en++;
                        break;
                    case "capacitor":
                        type = "capacitor";
                        _counter.cap.en++;
                        break;
                    case "diode":
                        type = "diode";
                        _counter.diode.en++;
                        break;
                    case "jumper":
                        type = "jumper";
                        _counter.jumper.en++;
                        break;
                    case "inductor":
                        type = "inductor";
                        _counter.ind.en++;
                        break;
                    default:
                        type = "Null";
                        break;
                }
            }
            else
            {
                normalStr = "Yes";
                switch (words[0])
                {
                    case "resistor":
                        type = "resistor";
                        _counter.res.normal++;
                        break;
                    case "capacitor":
                        type = "capacitor";
                        _counter.cap.normal++;
                        break;
                    case "diode":
                        type = "diode";
                        _counter.diode.normal++;
                        break;
                    case "jumper":
                        type = "jumper";
                        _counter.jumper.normal++;
                        break;
                    case "inductor":
                        type = "inductor";
                        _counter.ind.normal++;
                        break;
                    default:
                        type = "Null";
                        break;
                }
            }
            if (type != "null")
            {
                result = string.Format("{0},{1},{2}, {3}, {4}, {5}, {6}", index, name, type, normalStr, enStr, edStr, edenStr);
                index++;
            }

            else
                result = null;
            return result;
        }


        public static void OpenWithDefaultProgram(string path)
        {
            Process fileopener = new Process();

            fileopener.StartInfo.FileName = "explorer";
            fileopener.StartInfo.Arguments = "\"" + path + "\"";
            fileopener.Start();
        }

    }
}
