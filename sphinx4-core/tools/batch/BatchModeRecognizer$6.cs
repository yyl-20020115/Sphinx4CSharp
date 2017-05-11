using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	internal sealed class BatchModeRecognizer_6 : java.lang.Object, CommandInterface
	{
		internal BatchModeRecognizer_6(BatchModeRecognizer batchModeRecognizer)
		{
			this_0 = batchModeRecognizer;
		}

		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			Result result = null;
			if (array.Length < 2)
			{
				commandInterpreter.putResponse("Usage: recognize audio [transcript]");
			}
			else
			{
				string inputStream = array[1];
				string referenceText = null;
				if (array.Length > 2)
				{
					referenceText = array[2];
				}
				try
				{
					this.this_0.setInputStream(inputStream);
					result = this.this_0.recognizer.recognize(referenceText);
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
			return "perform recognition on the given audio";
		}

		internal BatchModeRecognizer this_0;
	}
}
