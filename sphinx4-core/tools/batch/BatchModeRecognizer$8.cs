using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	internal sealed class BatchModeRecognizer_8 : Object, CommandInterface
	{
		internal BatchModeRecognizer_8(BatchModeRecognizer batchModeRecognizer)
		{
			this_0 = batchModeRecognizer;
		}
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			Result result = null;
			if (array.Length != 1)
			{
				commandInterpreter.putResponse("Usage: batchRecognize");
			}
			else
			{
				try
				{
					if (this.this_0.curBatchItem == null)
					{
						this.this_0.batchManager.start();
						this.this_0.curBatchItem = this.this_0.batchManager.getNextItem();
					}
					string filename = this.this_0.curBatchItem.getFilename();
					string transcript = this.this_0.curBatchItem.getTranscript();
					this.this_0.setInputStream(filename);
					result = this.this_0.recognizer.recognize(transcript);
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
			return "perform recognition on the current batch item";
		}
		
		internal BatchModeRecognizer this_0;
	}
}
