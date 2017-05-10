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
	
	internal sealed class CommandInterpreter$5 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter$5(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			123,
			109
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			CommandInterpreter.access$302(this.this$0, true);
			return "";
		}

		public string getHelp()
		{
			return "exit the shell";
		}

		
		internal CommandInterpreter this$0 = commandInterpreter;
	}
}
