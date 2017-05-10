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
	
	internal sealed class BatchModeRecognizer$9 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer$9(BatchModeRecognizer batchModeRecognizer)
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
					if (this.this$0.curBatchItem == null)
					{
						this.this$0.batchManager.start();
					}
					this.this$0.curBatchItem = this.this$0.batchManager.getNextItem();
					if (this.this$0.curBatchItem == null)
					{
						this.this$0.batchManager.start();
						this.this$0.curBatchItem = this.this$0.batchManager.getNextItem();
					}
					string filename = this.this$0.curBatchItem.getFilename();
					string transcript = this.this$0.curBatchItem.getTranscript();
					if (array.Length == 2)
					{
						commandInterpreter.putResponse(new StringBuilder().append("Skipping: ").append(transcript).toString());
					}
					else
					{
						this.this$0.setInputStream(filename);
						result = this.this$0.recognizer.recognize(transcript);
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

		
		internal BatchModeRecognizer this$0 = batchModeRecognizer;
	}
}
