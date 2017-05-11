﻿using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.tools.batch
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	
	.
	
	internal sealed class BatchModeRecognizer_3 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer_3(BatchModeRecognizer batchModeRecognizer)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			204,
			101,
			173,
			191,
			11,
			2,
			97,
			223,
			1
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length != 2)
			{
				commandInterpreter.putResponse("Usage: edit component");
			}
			else
			{
				Exception ex3;
				try
				{
					ConfigurationManagerUtils.editConfig(this.this_0.cm, array[1]);
				}
				catch (Exception ex)
				{
					Exception ex2 = ByteCodeHelper.MapException<Exception>(ex, 0);
					if (ex2 == null)
					{
						throw;
					}
					ex3 = ex2;
					goto IL_39;
				}
				goto IL_5F;
				IL_39:
				Exception ex4 = ex3;
				commandInterpreter.putResponse(new StringBuilder().append("Failed to save ").append(ex4).toString());
			}
			IL_5F:
			return "";
		}

		public string getHelp()
		{
			return "edit a  component's configuration";
		}

		
		internal BatchModeRecognizer this_0 = batchModeRecognizer;
	}
}
