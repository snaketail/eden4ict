using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// This file contains functions to help count the components
/// </summary>

namespace eden4ict
{
    internal partial class Program
    {
        /// <summary>
        /// To create the list of counters based on component list
        /// </summary>
        /// <param name="componentList"></param>
        /// <param name="counters"></param>
        static void createCounters(List<string> componentList, List<counter> counters)
        {
            foreach (var component in componentList)
            {
                string priStr;
                string secStr;
                if (component.Contains('.'))
                {
                    string[] subs = component.Split('.');
                    priStr = subs[0];
                    secStr = subs[1];
                }
                else
                {
                    priStr = component;
                    secStr = "*";
                }
                var currentCounter = new counter() { PriType = priStr, SecType = secStr};
                counters.Add(currentCounter);
            }
        }


        /// <summary>
        /// Get analog folder path, if in project folder, run directly, else propmt dialog to choose
        /// </summary>
        /// <param name="currentDir"></param>
        /// <returns></returns>
        static string getDirPath()
        {
            string currentDir = Environment.CurrentDirectory;

            bool inProjectfolder = false;
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
            return dirPath;
        }


        /// <summary>
        /// Generate the start string list (without secondary option)
        /// </summary>
        /// <param name="counters"></param>
        /// <returns>Generated Start String list</returns>
        static List<string> generateStartStrings(List<counter> counters)
        {
            List<string> strings = new List<string>();
            foreach (var counter in counters)
                if (!strings.Contains(counter.PriType))
                    strings.Add(counter.PriType);
            return strings;
        }

        static string ProcessFile(ref int index, string file,List<string>startStr, List<counter> Counters)
        {
            string result = null;
            string name = Path.GetFileName(file);
            if (!excludeStr.Any(name.Contains))
            {
                foreach (string line in File.ReadLines(file))
                {
                    if (startStr.Any(line.StartsWith))
                    {
                        //Console.WriteLine("processing " + line);

                        result = checkEdEn(ref index, name, line, Counters);
                        break;
                    }
                }
            }
            return result;
        }

        static string checkEdEn(ref int index, string name, string line, List<counter> Counters)
        {
            string type=null;
            string edStr = " ", enStr = " ", edenStr = " ", normalStr = " ";
            string[] words = line.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
            bool ed = words.Contains("ed");
            bool en = words.Contains("en");
            bool eden = ed && en;

            //define the correct counter in Counters
            counter thiscounter = matchCounter(words, Counters, ref type);

            if (thiscounter == null)
                return null;

            if (eden)
            {
                edenStr = "yes";
                thiscounter.eden++;
            }
            else if (ed)
            {
                edStr = "Yes";
                thiscounter.ed++;
            }
            else if (en)
            {
                enStr = "Yes";
                thiscounter.en++;
            }
            else
            {
                normalStr = "Yes";
                thiscounter.normal++;
            }

            index++;
            return string.Format("{0},{1},{2}, {3}, {4}, {5}, {6}", index, name, type, normalStr, enStr, edStr, edenStr);
        }

        /// <summary>
        /// Match the suitable counter based on keywords
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="Counters"></param>
        /// <returns></returns>
        static counter matchCounter(string[] keywords, List<counter> Counters, ref string itemType)
        {
            foreach (var counter in Counters)
            {
                if (counter.SecType == "*")
                {
                    if (keywords.Contains(counter.PriType))
                    {
                        itemType = counter.Name;
                        //Console.WriteLine(itemType);
                        return counter;

                    }
                }
                else
                {
                    if(keywords.Contains(counter.PriType) & keywords.Contains(counter.SecType))
                    {
                        itemType = counter.Name;
                        //Console.WriteLine(itemType);

                        return counter;
                    }
                }
            }
            itemType = null;
            return null;
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
