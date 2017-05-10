using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	[EnclosingMethod("edu.cmu.sphinx.util.SocketCommandClient", "main", "([Ljava.lang.java.lang.String;)V")]
	[SourceFile("SocketCommandClient.java")]
	internal sealed class SocketCommandClient_1 : java.lang.Object, CommandInterface
	{
		
		
		internal SocketCommandClient_1(SocketCommandClient socketCommandClient)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			129,
			102,
			103,
			49,
			166,
			114
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 1; i < array.Length; i++)
			{
				stringBuilder.append(array[i]).append(' ');
			}
			this.val_sci.sendCommand(stringBuilder.toString());
			return "";
		}

		public string getHelp()
		{
			return "send a command";
		}

		
		internal SocketCommandClient val_sci = socketCommandClient;
	}
}
