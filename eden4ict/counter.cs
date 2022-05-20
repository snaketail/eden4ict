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
        public uint MyProperty { get { return normal + ed + en + eden; } }
    }
}
