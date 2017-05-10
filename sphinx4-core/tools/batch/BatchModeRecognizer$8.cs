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
	
	internal sealed class BatchModeRecognizer$8 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer$8(BatchModeRecognizer batchModeRecognizer)
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			48,
			130,
			101,
			208,
			109,
			112,
			155,
			113,
			113,
			108,
			223,
			5,
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
			if (array.Length != 1)
			{
				commandInterpreter.putResponse("Usage: batchRecognize");
			}
			else
			{
				IOException ex2;
				try
				{
					if (this.this$0.curBatchItem == null)
					{
						this.this$0.batchManager.start();
						this.this$0.curBatchItem = this.this$0.batchManager.getNextItem();
					}
					string filename = this.this$0.curBatchItem.getFilename();
					string transcript = this.this$0.curBatchItem.getTranscript();
					this.this$0.setInputStream(filename);
					result = this.this$0.recognizer.recognize(transcript);
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_9E;
				}
				goto IL_CB;
				IL_9E:
				IOException ex3 = ex2;
				commandInterpreter.putResponse(new StringBuilder().append("I/O error during decoding: ").append(Throwable.instancehelper_getMessage(ex3)).toString());
			}
			IL_CB:
			return (result == null) ? "" : result.getBestResultNoFiller();
		}

		public string getHelp()
		{
			return "perform recognition on the current batch item";
		}

		
		internal BatchModeRecognizer this$0 = batchModeRecognizer;
	}
}
