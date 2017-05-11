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
	
	internal sealed class CommandInterpreter_3 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter_3(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			93,
			127,
			16
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			this.this_0.putResponse(new StringBuilder().append("Total number of commands: ").append(CommandInterpreter.access_200(this.this_0)).toString());
			return "";
		}

		public string getHelp()
		{
			return "shows command status";
		}

		
		internal CommandInterpreter this_0 = commandInterpreter;
	}
}
