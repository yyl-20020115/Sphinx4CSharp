using edu.cmu.sphinx.util;

namespace edu.cmu.sphinx.tools.batch
{

	internal sealed class BatchModeRecognizer_7 : java.lang.Object, CommandInterface
	{
		internal BatchModeRecognizer_7(BatchModeRecognizer batchModeRecognizer)
		{
			this_0 = batchModeRecognizer;
		}

		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length != 1)
			{
				commandInterpreter.putResponse("Usage: statsReset");
			}
			else
			{
				this.this_0.recognizer.resetMonitors();
			}
			return "";
		}

		public string getHelp()
		{
			return "resets gathered statistics";
		}
		
		internal BatchModeRecognizer this_0;
	}
}
