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
	
	internal sealed class BatchModeRecognizer_2 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer_2(BatchModeRecognizer batchModeRecognizer)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			186,
			101,
			146,
			103,
			100,
			17,
			230,
			69
		})]
		
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

		
		internal BatchModeRecognizer this_0 = batchModeRecognizer;
	}
}
