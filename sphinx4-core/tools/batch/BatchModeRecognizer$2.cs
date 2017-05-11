using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.tools.batch
{
	internal sealed class BatchModeRecognizer_2 : java.lang.Object, CommandInterface
	{
		internal BatchModeRecognizer_2(BatchModeRecognizer batchModeRecognizer)
		{
			this_0 = batchModeRecognizer;
		}

		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length < 2)
			{
				ConfigurationManagerUtils.showConfig(this.this_0.cm);
			}
			else
			{
				for (int i = 1; i < array.Length; i++)
				{
					string name = array[i];
					ConfigurationManagerUtils.showConfig(this.this_0.cm, name);
				}
			}
			return "";
		}

		public string getHelp()
		{
			return "show component configuration";
		}

		internal BatchModeRecognizer this_0;
	}
}
