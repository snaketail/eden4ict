/// <summary>
/// The counter class pride the storage of the counted option numbers
/// </summary>

namespace eden4ict
{
    public class counter
    {
        public string PriType { get; set; } = null;
        public string SecType { get; set; } = null;
        public uint normal { get; set; } = 0;
        public uint ed { get; set; } = 0;
        public uint en { get; set; } = 0;
        public uint eden { get; set; } = 0;
        public uint total { get { return normal + ed + en + eden; } }

        public string Name
        {
            get
            {
                if (SecType == "*")
                    return PriType;
                else
                    return PriType + '[' + SecType + ']';
            }
        }

        public double  s3Time 
        { 
            get
            {
                return (timeLookupTable.getTestTime(ict3070Series.series3, Name, "ED") * ed +
                    timeLookupTable.getTestTime(ict3070Series.series3, Name, "EN") * en+
                    timeLookupTable.getTestTime(ict3070Series.series3, Name, "ED&EN") * eden+
                    timeLookupTable.getTestTime(ict3070Series.series3, Name, "Normal") * normal)/1000;
            }
        }

        public double s6Time
        {
            get
            {
                return (timeLookupTable.getTestTime(ict3070Series.series6, Name, "ED") * ed+
                    timeLookupTable.getTestTime(ict3070Series.series6, Name, "EN") * en +
                    timeLookupTable.getTestTime(ict3070Series.series6, Name, "ED&EN") * eden +
                    timeLookupTable.getTestTime(ict3070Series.series6, Name, "Normal") * normal)/1000;
            }
        }

        public double deltaTime 
        { 
            get  { return s3Time - s6Time; }
        }
    }
}
