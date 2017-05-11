using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class CommandInterpreter_8 : java.lang.Object, CommandInterface
	{	
		internal CommandInterpreter_8(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}
	
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			Runtime.getRuntime().gc();
			return "";
		}

		public string getHelp()
		{
			return "performs garbage collection";
		}
		
		internal CommandInterpreter this_0;
	}
}
