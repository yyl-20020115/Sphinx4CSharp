using System;

using IKVM.Attributes;
using java.io;
using java.lang;
using java.util;
using javax.sound.sampled;

namespace edu.cmu.sphinx.tools.audio
{
	public class RawReader : java.lang.Object
	{
		
		
		public RawReader()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			183,
			130,
			103,
			105,
			104,
			100,
			104,
			132,
			127,
			5,
			134,
			231,
			70,
			104,
			103,
			98,
			98,
			99,
			103,
			111,
			101,
			101,
			99,
			130,
			176,
			169,
			100,
			98,
			99,
			102,
			99,
			138,
			104,
			103,
			9,
			234,
			69,
			104,
			99,
			138,
			106,
			103,
			9,
			232,
			73,
			99,
			143,
			112,
			229,
			69,
			110,
			106,
			56,
			168
		})]
		
		public static short[] readAudioData(InputStream audioStream, AudioFormat audioFormat)
		{
			int num = 1;
			AudioFormat.Encoding encoding = audioFormat.getEncoding();
			int num2 = audioFormat.getSampleSizeInBits() / 8;
			if (encoding == AudioFormat.Encoding.PCM_SIGNED)
			{
				num = 1;
			}
			else if (encoding == AudioFormat.Encoding.PCM_UNSIGNED)
			{
				num = 0;
			}
			else
			{
				java.lang.System.err.println(new StringBuilder().append("Unsupported audio encoding: ").append(encoding).toString());
				java.lang.System.exit(-1);
			}
			int num3 = audioFormat.isBigEndian() ? 1 : 0;
			byte[] array = new byte[num2];
			ArrayList arrayList = new ArrayList();
			int num4 = 0;
			while (num4 == 0)
			{
				int i;
				int num5 = i = audioStream.read(array, 0, num2);
				while (i < num2)
				{
					if (num5 == -1)
					{
						num4 = 1;
						break;
					}
					num5 = audioStream.read(array, i, num2 - i);
					i += num5;
				}
				if (num4 == 0)
				{
					int num6;
					if (num3 != 0)
					{
						num6 = (int)array[0];
						if (num == 0)
						{
							num6 &= 255;
						}
						for (int j = 1; j < num2; j++)
						{
							int num7 = (int)array[j];
							num6 = (num6 << 8) + num7;
						}
					}
					else
					{
						num6 = (int)array[num2 - 1];
						if (num == 0)
						{
							num6 &= 255;
						}
						for (int j = num2 - 2; j >= 0; j += -1)
						{
							int num7 = (int)array[j];
							num6 = (num6 << 8) + num7;
						}
					}
					if (num == 0)
					{
						num6 -= 1 << num2 * 8 - 1;
					}
					arrayList.add(Short.valueOf((short)num6));
				}
			}
			short[] array2 = new short[arrayList.size()];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = ((Short)arrayList.get(j)).shortValue();
			}
			return array2;
		}
	}
}
