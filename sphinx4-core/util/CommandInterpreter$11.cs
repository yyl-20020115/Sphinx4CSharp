using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	internal sealed class CommandInterpreter_11 : Object, CommandInterface
	{
		internal CommandInterpreter_11(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}

		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length >= 3)
			{
				try
				{
					int num = Integer.parseInt(array[1]);
					string[] args = (string[])Arrays.copyOfRange(array, 2, array.Length);
					for (int i = 0; i < num; i++)
					{
						this.this_0.putResponse(this.this_0.execute(args));
					}
				}
				catch (NumberFormatException)
				{
					this.this_0.putResponse("Usage: repeat count command args");
				}
			}
			else
			{
				this.this_0.putResponse("Usage: repeat count command args");
			}
			return "";
		}

		public string getHelp()
		{
			return "repeatedly execute a command";
		}

		internal CommandInterpreter this_0;
	}
}
