using java.lang;

namespace edu.cmu.sphinx.util
{

	internal sealed class CommandInterpreter_4 : Object, CommandInterface
	{
		internal CommandInterpreter_4(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			StringBuilder stringBuilder = new StringBuilder(80);
			for (int i = 1; i < array.Length; i++)
			{
				stringBuilder.append(array[i]);
				stringBuilder.append(' ');
			}
			this.this_0.putResponse(stringBuilder.toString());
			return "";
		}

		public string getHelp()
		{
			return "display a line of text";
		}

		internal CommandInterpreter this_0;
	}
}
