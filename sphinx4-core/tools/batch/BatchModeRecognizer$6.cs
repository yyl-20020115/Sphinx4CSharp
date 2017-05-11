using System;

using edu.cmu.sphinx.result;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	
	.
	
	internal sealed class BatchModeRecognizer_6 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer_6(BatchModeRecognizer batchModeRecognizer)
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			0,
			130,
			101,
			144,
			100,
			98,
			101,
			196,
			108,
			223,
			5,
			226,
			61,
			98,
			114,
			47,
			229,
			69
		})]
		
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
				IOException ex2;
				try
				{
					this.this_0.setInputStream(inputStream);
					result = this.this_0.recognizer.recognize(referenceText);
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_53;
				}
				goto IL_80;
				IL_53:
				IOException ex3 = ex2;
				commandInterpreter.putResponse(new StringBuilder().append("I/O error during decoding: ").append(Throwable.instancehelper_getMessage(ex3)).toString());
			}
			IL_80:
			return (result == null) ? "" : result.getBestResultNoFiller();
		}

		public string getHelp()
		{
			return "perform recognition on the given audio";
		}

		
		internal BatchModeRecognizer this_0 = batchModeRecognizer;
	}
}
