using System;

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
	
	internal sealed class BatchModeRecognizer_12 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer_12(BatchModeRecognizer batchModeRecognizer)
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			185,
			101,
			173,
			191,
			1,
			2,
			97,
			191,
			16
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length != 2)
			{
				commandInterpreter.putResponse("Usage: batchReset batchfile");
			}
			else
			{
				IOException ex2;
				try
				{
					this.this_0.setBatchFile(array[1]);
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_2F;
				}
				goto IL_64;
				IL_2F:
				IOException ex3 = ex2;
				commandInterpreter.putResponse(new StringBuilder().append("Can't load ").append(array[1]).append(' ').append(ex3).toString());
			}
			IL_64:
			return "";
		}

		public string getHelp()
		{
			return "reset the batch to the beginning";
		}

		
		internal BatchModeRecognizer this_0 = batchModeRecognizer;
	}
}
