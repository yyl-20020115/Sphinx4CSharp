using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.tools.batch
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	
	.
	
	internal sealed class BatchModeRecognizer_1 : java.lang.Object, CommandInterface
	{
		
		
		internal BatchModeRecognizer_1(BatchModeRecognizer batchModeRecognizer)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			170,
			101,
			141,
			127,
			16,
			137
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length != 1)
			{
				commandInterpreter.putResponse("Usage: ls");
			}
			else
			{
				Iterator iterator = this.this_0.cm.getInstanceNames(ClassLiteral<Configurable>.Value).iterator();
				while (iterator.hasNext())
				{
					string response = (string)iterator.next();
					commandInterpreter.putResponse(response);
				}
			}
			return "";
		}

		public string getHelp()
		{
			return "list active components";
		}

		
		internal BatchModeRecognizer this_0 = batchModeRecognizer;
	}
}
