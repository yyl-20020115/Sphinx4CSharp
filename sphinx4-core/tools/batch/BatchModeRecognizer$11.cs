using edu.cmu.sphinx.util;
using java.io;

namespace edu.cmu.sphinx.tools.batch
{
	internal sealed class BatchModeRecognizer_11 : java.lang.Object, CommandInterface
	{
		internal BatchModeRecognizer_11(BatchModeRecognizer batchModeRecognizer)
		{
			this_0 = batchModeRecognizer;
		}

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
				catch (IOException)
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

		
		internal BatchModeRecognizer this_0;
	}
}
