using System;

using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using IKVM.Runtime;
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
	
	internal sealed class BatchModeRecognizer_9 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer_9(BatchModeRecognizer batchModeRecognizer)
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			80,
			130,
			106,
			240,
			71,
			109,
			144,
			251,
			69,
			109,
			112,
			187,
			113,
			113,
			101,
			159,
			3,
			108,
			255,
			5,
			69,
			226,
			61,
			98,
			114,
			47,
			197
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			Result result = null;
			if (array.Length != 1 && array.Length != 2)
			{
				commandInterpreter.putResponse("Usage: batchNext [norec]");
			}
			else
			{
				IOException ex2;
				try
				{
					if (this.this_0.curBatchItem == null)
					{
						this.this_0.batchManager.start();
					}
					this.this_0.curBatchItem = this.this_0.batchManager.getNextItem();
					if (this.this_0.curBatchItem == null)
					{
						this.this_0.batchManager.start();
						this.this_0.curBatchItem = this.this_0.batchManager.getNextItem();
					}
					string filename = this.this_0.curBatchItem.getFilename();
					string transcript = this.this_0.curBatchItem.getTranscript();
					if (array.Length == 2)
					{
						commandInterpreter.putResponse(new StringBuilder().append("Skipping: ").append(transcript).toString());
					}
					else
					{
						this.this_0.setInputStream(filename);
						result = this.this_0.recognizer.recognize(transcript);
					}
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_102;
				}
				goto IL_12F;
				IL_102:
				IOException ex3 = ex2;
				commandInterpreter.putResponse(new StringBuilder().append("I/O error during decoding: ").append(Throwable.instancehelper_getMessage(ex3)).toString());
			}
			IL_12F:
			return (result == null) ? "" : result.getBestResultNoFiller();
		}

		public string getHelp()
		{
			return "advance the batch and perform recognition";
		}

		
		internal BatchModeRecognizer this_0 = batchModeRecognizer;
	}
}
