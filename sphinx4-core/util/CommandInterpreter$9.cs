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
	
	internal sealed class CommandInterpreter_9 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter_9(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			106,
			107,
			139,
			159,
			27,
			159,
			27
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			long num = Runtime.getRuntime().totalMemory();
			long num2 = Runtime.getRuntime().freeMemory();
			this.this_0.putResponse(new StringBuilder().append("Free Memory  : ").append((double)num2 / 1048576.0).append(" mbytes").toString());
			this.this_0.putResponse(new StringBuilder().append("Total Memory : ").append((double)num / 1048576.0).append(" mbytes").toString());
			return "";
		}

		public string getHelp()
		{
			return "shows memory statistics";
		}

		
		internal CommandInterpreter this_0 = commandInterpreter;
	}
}
