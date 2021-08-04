using WT.Libraries.Logging.seq;

namespace WT.Libraries.Logging
{
    public class LoggerOptions
    {
        public SeqLoggerOptions Seq { get; set; } = new SeqLoggerOptions();

        public string Division { get; set; } = "EcommerceDiv";
        public string Environment { get; set; } = "Development";
    }
}
