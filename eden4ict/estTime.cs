using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eden4ict
{
    public enum ict3070Series
    {
        series3,
        series6
    }


    static public class timeLookupTable
    {
        static private string[] componentList = { "jumper", "resistor", "capacitor[fr128]", "capacitor[fr1024]","capacitor[fr8192]", "inductor", "diode" };
        static private string[] optionList = { "Normal", "EN","ED","ED&EN" };
        static private double[,] series3Table = {
                                                    { 5.9, 3.57, 0, 5.7, 5.4, 0, 12 },
                                                    { 17, 4.3, 0, 7.1, 7.3, 0, 12 },
                                                    { 23, 21.3, 35.8, 24.3, 24, 0, 12 },
                                                    { 155, 21.5, 53.5, 44.3, 44.3, 19.81, 12 }

                                                 };
        static private double[,] series6Table = {
                                                    { 3.8, 3.8, 0, 5.6, 4, 0, 12 },
                                                    { 3.8, 3.8, 0, 5.6, 4, 0, 12 },
                                                    { 20.5, 20.5, 20.6, 12.9, 12, 0, 12 },
                                                    { 20.5, 20.5, 20.6, 12.9, 12, 12.68, 12 }

                                                 };

        public static double getTestTime(ict3070Series series, string component, string option)
        {
            var targetTable  = (series == ict3070Series.series3) ? series3Table : series6Table;
            
            int componentIndex = Array.IndexOf(componentList, component);
            int optionIndex = Array.IndexOf(optionList, option);
            //Console.WriteLine(componentIndex.ToString() +", "+ optionIndex.ToString());
            return targetTable[optionIndex, componentIndex];
        }
    }
}
