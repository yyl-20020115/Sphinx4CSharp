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
	
	internal sealed class CommandInterpreter$2 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter$2(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			81,
			112
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			CommandInterpreter.access$100(this.this$0).dump();
			return "";
		}

		public string getHelp()
		{
			return "shows command history";
		}

		
		internal CommandInterpreter this$0 = commandInterpreter;
	}
}
