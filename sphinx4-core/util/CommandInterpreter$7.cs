using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class CommandInterpreter_7 : Object, CommandInterface
	{
		internal CommandInterpreter_7(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			this.this_0.putResponse("Command Interpreter - Version 1.1 ");
			return "";
		}

		public string getHelp()
		{
			return "displays version information";
		}
		
		internal CommandInterpreter this_0;
	}
}
