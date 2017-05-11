namespace edu.cmu.sphinx.util
{
	internal sealed class CommandInterpreter_6 : java.lang.Object, CommandInterface
	{
		internal CommandInterpreter_6(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}

		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			return "";
		}

		public string getHelp()
		{
			return "command executed upon exit";
		}

		internal CommandInterpreter this_0;
	}
}
