﻿using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	
	.
	
	internal sealed class CommandInterpreter_8 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter_8(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			94,
			106
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			Runtime.getRuntime().gc();
			return "";
		}

		public string getHelp()
		{
			return "performs garbage collection";
		}

		
		internal CommandInterpreter this_0 = commandInterpreter;
	}
}
