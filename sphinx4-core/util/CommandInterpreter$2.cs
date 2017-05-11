namespace edu.cmu.sphinx.util
{
	internal sealed class CommandInterpreter_2 : java.lang.Object, CommandInterface
	{
		internal CommandInterpreter_2(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}
	
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			CommandInterpreter.access_100(this.this_0).dump();
			return "";
		}

		public string getHelp()
		{
			return "shows command history";
		}

		
		internal CommandInterpreter this_0;
	}
}
