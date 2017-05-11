using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	
	.
	
	internal sealed class BatchModeRecognizer_5 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer_5(BatchModeRecognizer batchModeRecognizer)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			240,
			101,
			173,
			153
		})]
		
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

		
		internal BatchModeRecognizer this_0 = batchModeRecognizer;
	}
}
