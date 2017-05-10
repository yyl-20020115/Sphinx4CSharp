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
	
	internal sealed class CommandInterpreter$4 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter$4(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			105,
			136,
			103,
			106,
			9,
			198,
			113
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			StringBuilder stringBuilder = new StringBuilder(80);
			for (int i = 1; i < array.Length; i++)
			{
				stringBuilder.append(array[i]);
				stringBuilder.append(' ');
			}
			this.this$0.putResponse(stringBuilder.toString());
			return "";
		}

		public string getHelp()
		{
			return "display a line of text";
		}

		
		internal CommandInterpreter this$0 = commandInterpreter;
	}
}
