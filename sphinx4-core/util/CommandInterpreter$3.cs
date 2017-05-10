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
	
	internal sealed class CommandInterpreter$3 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter$3(CommandInterpreter commandInterpreter)
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
			this.this$0.putResponse(new StringBuilder().append("Total number of commands: ").append(CommandInterpreter.access$200(this.this$0)).toString());
			return "";
		}

		public string getHelp()
		{
			return "shows command status";
		}

		
		internal CommandInterpreter this$0 = commandInterpreter;
	}
}
