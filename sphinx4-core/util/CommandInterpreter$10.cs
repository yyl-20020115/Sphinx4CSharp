using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class CommandInterpreter_10 : Object, CommandInterface
	{
		internal CommandInterpreter_10(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length == 2)
			{
				try
				{
					try
					{
						float num = Float.parseFloat(array[1]);
						Thread.sleep(ByteCodeHelper.f2l(num * 1000f));
					}
					catch (NumberFormatException)
					{
						this.this_0.putResponse("Usage: delay time-in-seconds");
					}
				}
				catch (InterruptedException)
				{
				}
			}
			else
			{
				this.this_0.putResponse("Usage: delay time-in-seconds");
			}
			return "";
		}

		public string getHelp()
		{
			return "pauses for a given number of seconds";
		}

		internal CommandInterpreter this_0;
	}
}
