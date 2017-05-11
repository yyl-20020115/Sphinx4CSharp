namespace edu.cmu.sphinx.util
{
	internal sealed class CommandInterpreter_5 : java.lang.Object, CommandInterface
	{
		internal CommandInterpreter_5(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			CommandInterpreter.access_302(this.this_0, true);
			return "";
		}

		public string getHelp()
		{
			return "exit the shell";
		}

		internal CommandInterpreter this_0;
	}
}
