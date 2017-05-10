using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.util
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	[EnclosingMethod("edu.cmu.sphinx.util.CommandInterpreter", "addStandardCommands", "()V")]
	[SourceFile("CommandInterpreter.java")]
	
	internal sealed class CommandInterpreter$10 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter$10(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			125,
			133,
			105,
			223,
			3,
			226,
			61,
			97,
			144,
			34,
			97,
			130,
			144
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length == 2)
			{
				try
				{
					try
					{
						float num = Float.parseFloat(array[1]);
						Thread.sleep(ByteCodeHelper.f2l(num * 1000f));
					}
					catch (NumberFormatException ex)
					{
						goto IL_2A;
					}
				}
				catch (InterruptedException ex2)
				{
				}
				goto IL_58;
				IL_2A:
				this.this$0.putResponse("Usage: delay time-in-seconds");
			}
			else
			{
				this.this$0.putResponse("Usage: delay time-in-seconds");
			}
			IL_58:
			return "";
		}

		public string getHelp()
		{
			return "pauses for a given number of seconds";
		}

		
		internal CommandInterpreter this$0 = commandInterpreter;
	}
}
