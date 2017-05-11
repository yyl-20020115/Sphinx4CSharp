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
	
	internal sealed class CommandInterpreter_2 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter_2(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			81,
			112
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			CommandInterpreter.access_100(this.this_0).dump();
			return "";
		}

		public string getHelp()
		{
			return "shows command history";
		}

		
		internal CommandInterpreter this_0 = commandInterpreter;
	}
}
