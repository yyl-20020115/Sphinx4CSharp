using java.lang;
using java.text;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.util
{
	public class SphinxLogFormatter : java.util.logging.Formatter
	{
		public SphinxLogFormatter()
		{
			this.DATE_FORMATTER = new SimpleDateFormat("HH:mm:ss.SSS");
		}

		public virtual void setTerse(bool terse)
		{
			this.terse = terse;
		}

		public virtual bool getTerse()
		{
			return this.terse;
		}

		public override string format(LogRecord record)
		{
			if (this.terse)
			{
				return new StringBuilder().append(record.getMessage()).append('\n').toString();
			}
			DateFormat date_FORMATTER = this.DATE_FORMATTER;
			string text = date_FORMATTER.format(new Date(record.getMillis()));
			StringBuilder stringBuilder = new StringBuilder().append(text).append(' ');
			string loggerName = record.getLoggerName();
			string text2;
			if (loggerName != null)
			{
				string[] array = String.instancehelper_split(loggerName, "[.]");
				text2 = array[array.Length - 1];
			}
			else
			{
				text2 = loggerName;
			}
			stringBuilder.append(Utilities.pad(new StringBuilder().append(record.getLevel().getName()).append(' ').append(text2).toString(), 24));
			stringBuilder.append("  ").append(record.getMessage()).append('\n');
			return stringBuilder.toString();
		}
		
		private DateFormat DATE_FORMATTER;

		private bool terse;
	}
}
