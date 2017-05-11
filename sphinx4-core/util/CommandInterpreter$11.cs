using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	
	.
	
	internal sealed class CommandInterpreter_11 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter_11(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			147,
			136,
			105,
			111,
			102,
			55,
			239,
			69,
			2,
			97,
			112,
			130,
			144
		})]
		
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
				catch (NumberFormatException ex)
				{
					goto IL_49;
				}
				goto IL_71;
				IL_49:
				this.this_0.putResponse("Usage: repeat count command args");
			}
			else
			{
				this.this_0.putResponse("Usage: repeat count command args");
			}
			IL_71:
			return "";
		}

		public string getHelp()
		{
			return "repeatedly execute a command";
		}

		
		internal CommandInterpreter this_0 = commandInterpreter;
	}
}
