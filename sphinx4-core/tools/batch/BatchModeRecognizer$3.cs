using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	internal sealed class BatchModeRecognizer_3 : Object, CommandInterface
	{	
		internal BatchModeRecognizer_3(BatchModeRecognizer batchModeRecognizer)
		{
			this_0 = batchModeRecognizer;
		}
	
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length != 2)
			{
				commandInterpreter.putResponse("Usage: edit component");
			}
			else
			{
				try
				{
					ConfigurationManagerUtils.editConfig(this.this_0.cm, array[1]);
				}
				catch (System.Exception ex)
				{
					commandInterpreter.putResponse(new StringBuilder().append("Failed to save ").append(ex).toString());
				}
			}
			return "";
		}

		public string getHelp()
		{
			return "edit a  component's configuration";
		}

		internal BatchModeRecognizer this_0;
	}
}
