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
	
	internal sealed class CommandInterpreter$12 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter$12(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			171,
			101,
			112,
			191,
			10,
			144
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length == 2)
			{
				if (!this.this$0.load(array[1]))
				{
					this.this$0.putResponse(new StringBuilder().append("load: trouble loading ").append(array[1]).toString());
				}
			}
			else
			{
				this.this$0.putResponse("Usage: load filename");
			}
			return "";
		}

		public string getHelp()
		{
			return "load and execute commands from a file";
		}

		
		internal CommandInterpreter this$0 = commandInterpreter;
	}
}
