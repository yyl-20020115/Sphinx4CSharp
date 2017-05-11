using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	
	.
	
	internal sealed class BatchModeRecognizer_4 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer_4(BatchModeRecognizer batchModeRecognizer)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			224,
			101,
			141,
			189
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length != 2)
			{
				commandInterpreter.putResponse("Usage: save filename.xml");
			}
			else
			{
				ConfigurationManager cm = this.this_0.cm;
				File.__<clinit>();
				ConfigurationManagerUtils.save(cm, new File(array[1]));
			}
			return "";
		}

		public string getHelp()
		{
			return "save configuration to a file";
		}

		
		internal BatchModeRecognizer this_0 = batchModeRecognizer;
	}
}
