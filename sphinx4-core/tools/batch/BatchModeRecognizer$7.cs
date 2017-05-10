using System;

using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	[EnclosingMethod("edu.cmu.sphinx.tools.batch.BatchModeRecognizer", "addCommands", "(Ledu.cmu.sphinx.util.CommandInterpreter;)V")]
	[SourceFile("BatchModeRecognizer.java")]
	
	internal sealed class BatchModeRecognizer$7 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer$7(BatchModeRecognizer batchModeRecognizer)
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			32,
			101,
			141,
			144
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length != 1)
			{
				commandInterpreter.putResponse("Usage: statsReset");
			}
			else
			{
				this.this$0.recognizer.resetMonitors();
			}
			return "";
		}

		public string getHelp()
		{
			return "resets gathered statistics";
		}

		
		internal BatchModeRecognizer this$0 = batchModeRecognizer;
	}
}
