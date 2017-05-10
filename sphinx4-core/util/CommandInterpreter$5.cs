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
	
	internal sealed class CommandInterpreter_5 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter_5(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			123,
			109
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			CommandInterpreter.access_302(this.this_0, true);
			return "";
		}

		public string getHelp()
		{
			return "exit the shell";
		}

		
		internal CommandInterpreter this_0 = commandInterpreter;
	}
}
