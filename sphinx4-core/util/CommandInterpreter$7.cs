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
	
	internal sealed class CommandInterpreter_7 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter_7(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			82,
			112
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			this.this_0.putResponse("Command Interpreter - Version 1.1 ");
			return "";
		}

		public string getHelp()
		{
			return "displays version information";
		}

		
		internal CommandInterpreter this_0 = commandInterpreter;
	}
}
