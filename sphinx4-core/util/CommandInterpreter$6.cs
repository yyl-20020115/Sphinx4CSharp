using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	[EnclosingMethod("edu.cmu.sphinx.util.CommandInterpreter", "addStandardCommands", "()V")]
	[SourceFile("CommandInterpreter.java")]
	
	internal sealed class CommandInterpreter$6 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter$6(CommandInterpreter commandInterpreter)
		{
		}

		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			return "";
		}

		public string getHelp()
		{
			return "command executed upon exit";
		}

		
		internal CommandInterpreter this$0 = commandInterpreter;
	}
}
