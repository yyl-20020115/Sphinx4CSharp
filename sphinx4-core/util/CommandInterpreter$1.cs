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
	
	internal sealed class CommandInterpreter$1 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter$1(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			69,
			107
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			CommandInterpreter.access_000(this.this$0);
			return "";
		}

		public string getHelp()
		{
			return "lists available commands";
		}

		
		internal CommandInterpreter this$0 = commandInterpreter;
	}
}
