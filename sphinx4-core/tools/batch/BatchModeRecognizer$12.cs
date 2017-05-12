using edu.cmu.sphinx.util;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	internal sealed class BatchModeRecognizer_12 : Object, CommandInterface
	{
		internal BatchModeRecognizer_12(BatchModeRecognizer batchModeRecognizer)
		{
			this_0 = batchModeRecognizer;
		}

		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length != 2)
			{
				commandInterpreter.putResponse("Usage: batchReset batchfile");
			}
			else
			{
				try
				{
					this.this_0.setBatchFile(array[1]);
				}
				catch (IOException ex)
				{
					commandInterpreter.putResponse(new StringBuilder().append("Can't load ").append(array[1]).append(' ').append(ex).toString());
				}
			}
			return "";
		}

		public string getHelp()
		{
			return "reset the batch to the beginning";
		}

		
		internal BatchModeRecognizer this_0;
	}
}
