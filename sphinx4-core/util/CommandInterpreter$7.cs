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
	
	internal sealed class CommandInterpreter$7 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter$7(CommandInterpreter commandInterpreter)
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
			this.this$0.putResponse("Command Interpreter - Version 1.1 ");
			return "";
		}

		public string getHelp()
		{
			return "displays version information";
		}

		
		internal CommandInterpreter this$0 = commandInterpreter;
	}
}
