using Serilog;
using Serilog.Core;

namespace WT.Libraries.Logging
{
	/// <summary>
	/// This class contains a logger that will sent all the logs to the console using Serilog.
	/// ⚠️ This is ONLY to be used in startup process before the real logging mechanism is available.
	/// </summary>
	public static class StartupLogger
	{
		/// <summary>
		/// Logger that will sent all the logs to the console using Serilog.
		/// ⚠️ This is ONLY to be used in startup process before the real logging mechanism is available.
		/// </summary>
		public static readonly Logger Logger = new LoggerConfiguration()
			.WriteTo.Console()
			.CreateLogger();
	}
}
