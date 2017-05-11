using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	internal sealed class BatchModeRecognizer_9 : java.lang.Object, CommandInterface
	{
		internal BatchModeRecognizer_9(BatchModeRecognizer batchModeRecognizer)
		{
			this_0 = batchModeRecognizer;
		}
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			Result result = null;
			if (array.Length != 1 && array.Length != 2)
			{
				commandInterpreter.putResponse("Usage: batchNext [norec]");
			}
			else
			{
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
					commandInterpreter.putResponse(new StringBuilder().append("I/O error during decoding: ").append(Throwable.instancehelper_getMessage(ex)).toString());
				}
			}
			return (result == null) ? "" : result.getBestResultNoFiller();
		}

		public string getHelp()
		{
			return "advance the batch and perform recognition";
		}

		internal BatchModeRecognizer this_0;
	}
}
