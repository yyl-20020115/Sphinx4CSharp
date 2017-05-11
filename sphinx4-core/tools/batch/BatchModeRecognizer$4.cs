using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.io;

namespace edu.cmu.sphinx.tools.batch
{
	internal sealed class BatchModeRecognizer_4 : java.lang.Object, CommandInterface
	{
		internal BatchModeRecognizer_4(BatchModeRecognizer batchModeRecognizer)
		{
			this_0 = batchModeRecognizer;
		}
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length != 2)
			{
				commandInterpreter.putResponse("Usage: save filename.xml");
			}
			else
			{
				ConfigurationManager cm = this.this_0.cm;
				ConfigurationManagerUtils.save(cm, new File(array[1]));
			}
			return "";
		}

		public string getHelp()
		{
			return "save configuration to a file";
		}

		internal BatchModeRecognizer this_0;
	}
}
