using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using IKVM.Runtime;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	internal sealed class BatchModeRecognizer_10 : java.lang.Object, CommandInterface
	{
		internal BatchModeRecognizer_10(BatchModeRecognizer batchModeRecognizer)
		{
			this_0 = batchModeRecognizer;
		}
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			Result result = null;
			if (array.Length != 1)
			{
				commandInterpreter.putResponse("Usage: batchAll");
			}
			else
			{
				string result2;
				IOException ex2;
				try
				{
					if (this.this_0.curBatchItem == null)
					{
						this.this_0.batchManager.start();
					}
					for (;;)
					{
						this.this_0.curBatchItem = this.this_0.batchManager.getNextItem();
						if (this.this_0.curBatchItem == null)
						{
							break;
						}
						string filename = this.this_0.curBatchItem.getFilename();
						string transcript = this.this_0.curBatchItem.getTranscript();
						this.this_0.setInputStream(filename);
						result = this.this_0.recognizer.recognize(transcript);
					}
					result2 = "";
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_B5;
				}
				return result2;
				IL_B5:
				IOException ex3 = ex2;
				commandInterpreter.putResponse(new StringBuilder().append("I/O error during decoding: ").append(Throwable.instancehelper_getMessage(ex3)).toString());
			}
			return (result == null) ? "" : result.getBestResultNoFiller();
		}

		public string getHelp()
		{
			return "recognize all of the remaining batch items";
		}
		
		internal BatchModeRecognizer this_0;
	}
}
