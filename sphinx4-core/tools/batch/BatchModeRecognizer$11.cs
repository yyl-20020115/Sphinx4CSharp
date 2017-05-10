using System;

using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	[EnclosingMethod("edu.cmu.sphinx.tools.batch.BatchModeRecognizer", "addCommands", "(Ledu.cmu.sphinx.util.CommandInterpreter;)V")]
	[SourceFile("BatchModeRecognizer.java")]
	
	internal sealed class BatchModeRecognizer_11 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer_11(BatchModeRecognizer batchModeRecognizer)
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			166,
			101,
			173,
			187,
			2,
			97,
			171
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length != 1)
			{
				commandInterpreter.putResponse("Usage: batchReset");
			}
			else
			{
				try
				{
					this.this_0.batchManager.start();
				}
				catch (IOException ex)
				{
					goto IL_2A;
				}
				goto IL_3B;
				IL_2A:
				commandInterpreter.putResponse("trouble reseting batch");
			}
			IL_3B:
			return "";
		}

		public string getHelp()
		{
			return "reset the batch to the beginning";
		}

		
		internal BatchModeRecognizer this_0 = batchModeRecognizer;
	}
}
