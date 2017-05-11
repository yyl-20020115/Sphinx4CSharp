using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	
	.
	
	internal sealed class CommandInterpreter_12 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter_12(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			171,
			101,
			112,
			191,
			10,
			144
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length == 2)
			{
				if (!this.this_0.load(array[1]))
				{
					this.this_0.putResponse(new StringBuilder().append("load: trouble loading ").append(array[1]).toString());
				}
			}
			else
			{
				this.this_0.putResponse("Usage: load filename");
			}
			return "";
		}

		public string getHelp()
		{
			return "load and execute commands from a file";
		}

		
		internal CommandInterpreter this_0 = commandInterpreter;
	}
}
