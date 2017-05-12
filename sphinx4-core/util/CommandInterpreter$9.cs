using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class CommandInterpreter_9 : Object, CommandInterface
	{
		internal CommandInterpreter_9(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}

		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			long num = Runtime.getRuntime().totalMemory();
			long num2 = Runtime.getRuntime().freeMemory();
			this.this_0.putResponse(new StringBuilder().append("Free Memory  : ").append((double)num2 / 1048576.0).append(" mbytes").toString());
			this.this_0.putResponse(new StringBuilder().append("Total Memory : ").append((double)num / 1048576.0).append(" mbytes").toString());
			return "";
		}

		public string getHelp()
		{
			return "shows memory statistics";
		}
		
		internal CommandInterpreter this_0;
	}
}
