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
	internal sealed class SocketCommandClient$3 : java.lang.Object, CommandInterface
	{
		
		
		internal SocketCommandClient$3(SocketCommandClient socketCommandClient)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			159,
			102,
			103,
			49,
			166,
			119,
			109,
			147
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 1; i < array.Length; i++)
			{
				stringBuilder.append(array[i]).append(' ');
			}
			commandInterpreter.putResponse(this.val$sci.sendCommandGetResponse(stringBuilder.toString()));
			while (this.val$sci.isResponse())
			{
				commandInterpreter.putResponse(this.val$sci.getResponse());
			}
			return "";
		}

		public string getHelp()
		{
			return "send a command, receive a response";
		}

		
		internal SocketCommandClient val$sci = socketCommandClient;
	}
}
