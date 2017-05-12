using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{

	internal sealed class CommandInterpreter_14 : Object, CommandInterface
	{		
		internal CommandInterpreter_14(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length > 1)
			{
				string[] args = (string[])Arrays.copyOfRange(array, 1, array.Length);
				long num = java.lang.System.currentTimeMillis();
				this.this_0.putResponse(this.this_0.execute(args));
				long num2 = java.lang.System.currentTimeMillis();
				this.this_0.putResponse(new StringBuilder().append("Time: ").append((double)(num2 - num) / 1000.0).append(" seconds").toString());
			}
			else
			{
				this.this_0.putResponse("Usage: time cmd [args]");
			}
			return "";
		}

		public string getHelp()
		{
			return "report the time it takes to run a command";
		}

		internal CommandInterpreter this_0;
	}
}
