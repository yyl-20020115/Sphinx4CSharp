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
	[EnclosingMethod("edu.cmu.sphinx.tools.batch.BatchModeRecognizer", "addCommands", "(Ledu.cmu.sphinx.util.CommandInterpreter;)V")]
	[SourceFile("BatchModeRecognizer.java")]
	
	internal sealed class BatchModeRecognizer$2 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer$2(BatchModeRecognizer batchModeRecognizer)
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
				ConfigurationManagerUtils.showConfig(this.this$0.cm);
			}
			else
			{
				for (int i = 1; i < array.Length; i++)
				{
					string name = array[i];
					ConfigurationManagerUtils.showConfig(this.this$0.cm, name);
				}
			}
			return "";
		}

		public string getHelp()
		{
			return "show component configuration";
		}

		
		internal BatchModeRecognizer this$0 = batchModeRecognizer;
	}
}
