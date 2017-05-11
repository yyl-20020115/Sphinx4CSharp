using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.utils
{
	public class Utils : java.lang.Object
	{		
		public static string[] toStringArray(HashMap syms)
		{
			string[] array = new string[syms.size()];
			Iterator iterator = syms.keySet().iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				array[((Integer)syms.get(text)).intValue()] = text;
			}
			return array;
		}

		public Utils()
		{
		}

		public static int search(ArrayList src, ArrayList pattern, int start)
		{
			int result = -1;
			int num = 0;
			if (start > src.size() - pattern.size())
			{
				return -1;
			}
			int num2;
			for (;;)
			{
				num2 = src.subList(num + start, src.size() - pattern.size() + 1).indexOf(pattern.get(0));
				if (num2 == -1)
				{
					break;
				}
				int num3 = 1;
				for (int i = 1; i < pattern.size(); i++)
				{
					if (!java.lang.String.instancehelper_equals((string)src.get(num + start + num2 + i), pattern.get(i)))
					{
						result = -1;
						num3 = 0;
						break;
					}
				}
				if (num3 != 0)
				{
					goto Block_5;
				}
				num += num2 + 1;
				if (num + start >= src.size())
				{
					return result;
				}
			}
			return num2;
			Block_5:
			result = num + num2;
			return result;
		}
		
		public static int getIndex(string[] strings, string @string)
		{
			for (int i = 0; i < strings.Length; i++)
			{
				if (java.lang.String.instancehelper_equals(java.lang.String.instancehelper_toLowerCase(@string), java.lang.String.instancehelper_toLowerCase(strings[i])))
				{
					return i;
				}
			}
			return -1;
		}
	}
}
