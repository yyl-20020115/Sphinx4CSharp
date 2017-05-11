using IKVM.Runtime;
using java.io;
using java.lang;
using java.text;
using java.util;

namespace edu.cmu.sphinx.util
{
	public class Utilities : java.lang.Object
	{
		public static string join(List tokens)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Iterator iterator = tokens.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				stringBuilder.append(text);
				stringBuilder.append(' ');
			}
			return java.lang.String.instancehelper_trim(stringBuilder.toString());
		}

		public static string pathJoin(string path1, string path2)
		{
			if (java.lang.String.instancehelper_length(path1) > 0 && java.lang.String.instancehelper_charAt(path1, java.lang.String.instancehelper_length(path1) - 1) == '/')
			{
				path1 = java.lang.String.instancehelper_substring(path1, 0, java.lang.String.instancehelper_length(path1) - 1);
			}
			if (java.lang.String.instancehelper_length(path2) > 0 && java.lang.String.instancehelper_charAt(path2, 0) == '/')
			{
				path2 = java.lang.String.instancehelper_substring(path2, 1);
			}
			return new StringBuilder().append(path1).append("/").append(path2).toString();
		}

		public static string doubleToScientificString(double number, int fractionDigits)
		{
			DecimalFormat decimalFormat = new DecimalFormat();
			StringBuilder stringBuilder = new StringBuilder(5 + fractionDigits).append("0.");
			for (int i = 0; i < fractionDigits; i++)
			{
				stringBuilder.append('0');
			}
			stringBuilder.append("E00");
			decimalFormat.applyPattern(stringBuilder.toString());
			string text = decimalFormat.format(number);
			int num = java.lang.String.instancehelper_indexOf(text, 69);
			if (java.lang.String.instancehelper_charAt(text, num + 1) != '-')
			{
				return new StringBuilder().append(java.lang.String.instancehelper_substring(text, 0, num + 1)).append('+').append(java.lang.String.instancehelper_substring(text, num + 1)).toString();
			}
			return text;
		}

		public static int readLittleEndianInt(DataInputStream dataStream)
		{
			return dataStream.readUnsignedByte() | dataStream.readUnsignedByte() << 8 | dataStream.readUnsignedByte() << 16 | dataStream.readUnsignedByte() << 24;
		}

		public static float readLittleEndianFloat(DataInputStream dataStream)
		{
			FloatConverter floatConverter = new FloatConverter();
			return FloatConverter.ToFloat(Utilities.readLittleEndianInt(dataStream), ref floatConverter);
		}
		public static void floorData(float[] data, float floor)
		{
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] < floor)
				{
					data[i] = floor;
				}
			}
		}

		public static void objectTracker(string name, int count)
		{
		}

		public static int swapInteger(int integer)
		{
			return (255 & integer) << 24 | (65280 & integer) << 8 | (16711680 & integer) >> 8 | (-16777216 & integer) >> 24;
		}
		public static void normalize(float[] data)
		{
			float num = 0f;
			int num2 = data.Length;
			for (int i = 0; i < num2; i++)
			{
				float num3 = data[i];
				num += num3;
			}
			if (num != 0f)
			{
				for (int j = 0; j < data.Length; j++)
				{
					data[j] /= num;
				}
			}
		}
		public static void nonZeroFloor(float[] data, float floor)
		{
			for (int i = 0; i < data.Length; i++)
			{
				if ((double)data[i] != (double)0f && data[i] < floor)
				{
					data[i] = floor;
				}
			}
		}

		public static float swapFloat(float floatValue)
		{
			FloatConverter floatConverter = new FloatConverter();
			return FloatConverter.ToFloat(Utilities.swapInteger(FloatConverter.ToInt(floatValue, ref floatConverter)), ref floatConverter);
		}

		public static string pad(string @string, int minLength)
		{
			string result = @string;
			int num = minLength - java.lang.String.instancehelper_length(@string);
			if (num > 0)
			{
				result = new StringBuilder().append(@string).append(Utilities.pad(minLength - java.lang.String.instancehelper_length(@string))).toString();
			}
			else if (num < 0)
			{
				result = java.lang.String.instancehelper_substring(@string, 0, minLength);
			}
			return result;
		}

		public static string pad(int padding)
		{
			if (padding > 0)
			{
				StringBuilder stringBuilder = new StringBuilder(padding);
				for (int i = 0; i < padding; i++)
				{
					stringBuilder.append(' ');
				}
				return stringBuilder.toString();
			}
			return "";
		}

		public static bool isCepstraFileBigEndian(string filename)
		{
			File file = new File(filename);
			int num = (int)file.length();
			DataInputStream dataInputStream = new DataInputStream(new FileInputStream(filename));
			int num2 = dataInputStream.readInt() * 4 + 4;
			dataInputStream.close();
			return num == num2;
		}

		public static void dumpMemoryInfo(string msg)
		{
			Runtime runtime = Runtime.getRuntime();
			long num = runtime.freeMemory();
			runtime.gc();
			long num2 = (runtime.freeMemory() - num) / (long)((ulong)1048576);
			long num3 = runtime.freeMemory() / (long)((ulong)1048576);
			long num4 = runtime.totalMemory() / (long)((ulong)1048576);
			long num5 = runtime.totalMemory() - runtime.freeMemory();
			if (num5 > Utilities.maxUsed)
			{
				Utilities.maxUsed = num5;
			}
			java.lang.System.@out.println(new StringBuilder().append("Memory (mb)  total: ").append(num4).append(" reclaimed: ").append(num2).append(" free: ").append(num3).append(" Max Used: ").append(Utilities.maxUsed / (long)((ulong)1048576)).append(" -- ").append(msg).toString());
		}
		private Utilities()
		{
		}

		public static string pad(int val, int minLength)
		{
			return Utilities.pad(java.lang.String.valueOf(val), minLength);
		}

		public static string pad(double val, int minLength)
		{
			return Utilities.pad(java.lang.String.valueOf(val), minLength);
		}

		public static void dump(PrintWriter pw, int padding, string @string)
		{
			pw.print(Utilities.pad(padding));
			pw.println(@string);
		}

		public static List asList(int[] align)
		{
			ArrayList arrayList = new ArrayList(align.Length);
			int num = align.Length;
			for (int i = 0; i < num; i++)
			{
				int num2 = align[i];
				arrayList.add(Integer.valueOf(num2));
			}
			return arrayList;
		}

		private const bool TRACKING_OBJECTS = false;

		internal static long maxUsed;
	}
}
