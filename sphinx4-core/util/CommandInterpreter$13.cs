using java.util;

namespace edu.cmu.sphinx.util
{
	internal sealed class CommandInterpreter_13 : java.lang.Object, CommandInterface
	{
		internal CommandInterpreter_13(CommandInterpreter commandInterpreter)
		{
			this_0 = commandInterpreter;
		}
	
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

		internal CommandInterpreter this_0;
	}
}
