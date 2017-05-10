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
	[EnclosingMethod("edu.cmu.sphinx.util.CommandInterpreter", "addStandardCommands", "()V")]
	[SourceFile("CommandInterpreter.java")]
	
	internal sealed class CommandInterpreter_14 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter_14(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			225,
			104,
			111,
			166,
			119,
			134,
			191,
			29,
			98,
			144
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length > 1)
			{
				string[] args = (string[])Arrays.copyOfRange(array, 1, array.Length);
				long num = java.lang.System.currentTimeMillis();
				this.this_0.putResponse(this.this_0.execute(args));
				long num2 = java.lang.System.currentTimeMillis();
				this.this_0.putResponse(new StringBuilder().append("Time: ").append((double)(num2 - num) / 1000.0).append(" seconds").toString());
			}
			else
			{
				this.this_0.putResponse("Usage: time cmd [args]");
			}
			return "";
		}

		public string getHelp()
		{
			return "report the time it takes to run a command";
		}

		
		internal CommandInterpreter this_0 = commandInterpreter;
	}
}
