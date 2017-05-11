using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.CommandInterface"
	})]
	
	.
	
	internal sealed class CommandInterpreter_13 : java.lang.Object, CommandInterface
	{
		
		
		internal CommandInterpreter_13(CommandInterpreter commandInterpreter)
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			189,
			104,
			106,
			103,
			98,
			103,
			111,
			100,
			110,
			164,
			234,
			57,
			230,
			75,
			100,
			174,
			127,
			0,
			120,
			98,
			98,
			144
		})]
		
		public string execute(CommandInterpreter commandInterpreter, string[] array)
		{
			if (array.Length > 1)
			{
				string[] array2 = new string[array.Length - 1];
				ArrayList arrayList = new ArrayList(5);
				int num = 0;
				for (int i = 1; i < array.Length; i++)
				{
					if (java.lang.String.instancehelper_equals(array[i], ";"))
					{
						if (num > 0)
						{
							arrayList.add(Arrays.copyOf(array2, num));
							num = 0;
						}
					}
					else
					{
						string[] array3 = array2;
						int num2 = num;
						num++;
						array3[num2] = array[i];
					}
				}
				if (num > 0)
				{
					arrayList.add(Arrays.copyOf(array2, num));
				}
				Iterator iterator = arrayList.iterator();
				while (iterator.hasNext())
				{
					string[] args = (string[])iterator.next();
					this.this_0.putResponse(this.this_0.execute(args));
				}
			}
			else
			{
				this.this_0.putResponse("Usage: chain cmd1 ; cmd2 ; cmd3 ");
			}
			return "";
		}

		public string getHelp()
		{
			return "execute multiple commands on a single line";
		}

		
		internal CommandInterpreter this_0 = commandInterpreter;
	}
}
