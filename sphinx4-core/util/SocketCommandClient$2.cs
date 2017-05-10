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
	internal sealed class SocketCommandClient$2 : java.lang.Object, CommandInterface
	{
		
		
		internal SocketCommandClient$2(SocketCommandClient socketCommandClient)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			145,
			109,
			147
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			while (this.val$sci.isResponse())
			{
				commandInterpreter.putResponse(this.val$sci.getResponse());
			}
			return "";
		}

		public string getHelp()
		{
			return "receive a response";
		}

		
		internal SocketCommandClient val$sci = socketCommandClient;
	}
}
