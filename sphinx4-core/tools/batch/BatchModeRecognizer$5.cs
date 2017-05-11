using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.tools.batch
{
	internal sealed class BatchModeRecognizer_5 : java.lang.Object, CommandInterface
	{
		internal BatchModeRecognizer_5(BatchModeRecognizer batchModeRecognizer)
		{
			this_0 = batchModeRecognizer;
		}
	
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length != 4)
			{
				commandInterpreter.putResponse("Usage: set component property value");
			}
			else
			{
				ConfigurationManagerUtils.setProperty(this.this_0.cm, array[1], array[3], array[2]);
			}
			return "";
		}

		public string getHelp()
		{
			return "set component property to a given value";
		}

		internal BatchModeRecognizer this_0;
	}
}
