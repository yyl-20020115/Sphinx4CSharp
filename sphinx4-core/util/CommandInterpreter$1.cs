namespace edu.cmu.sphinx.util
{
	internal sealed class CommandInterpreter_1 : java.lang.Object, CommandInterface
	{
		internal CommandInterpreter_1(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}
	
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			CommandInterpreter.access_000(this.this_0);
			return "";
		}

		public string getHelp()
		{
			return "lists available commands";
		}

		internal CommandInterpreter this_0;
	}
}
