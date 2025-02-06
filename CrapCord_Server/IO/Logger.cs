namespace CrapCord_Server.IO;

public class Logger {
	public void Log(object msg, LogSeverity severity = LogSeverity.Info) {
		var color = severity switch {
			LogSeverity.Info => ConsoleColor.Black,
			LogSeverity.Warning => ConsoleColor.Yellow,
			LogSeverity.Error => ConsoleColor.Red,
			LogSeverity.Critical => ConsoleColor.DarkRed,
			_ => ConsoleColor.Black
		};
			
		Console.ForegroundColor = color;
			
		Console.WriteLine(
			$"{DateTime.Now:HH:mm:ss}  {severity.ToString()}{"".PadRight(LogSeverity.Critical.ToString().Length - severity.ToString().Length)}  {msg}"
		);
			
		Console.ForegroundColor = ConsoleColor.Black;
		Console.Write("");
	}

	public enum LogSeverity {
		Info,
		Warning,
		Error,
		Critical
	}
}