using java.util;

namespace edu.cmu.sphinx.tools.audio
{
	public class Downsampler : java.lang.Object
	{		
		public static short[] downsample(short[] inSamples, int srIn, int srOut)
		{
			short[] array = new short[inSamples.Length];
			int num = -1;
			int num2 = 0;
			int num3 = srOut;
			int num4 = 0;
			while (num4 == 0)
			{
				int num5 = 0;
				for (int i = 0; i < srIn; i++)
				{
					if (num3 == srOut)
					{
						num++;
						if (num >= inSamples.Length)
						{
							num4 = 1;
							break;
						}
						num3 = 0;
					}
					num5 += (int)inSamples[num];
					num3++;
				}
				short[] array2 = array;
				int num6 = num2;
				num2++;
				int num7 = num5;
				array2[num6] = (short)((srIn != -1) ? (num7 / srIn) : (-(short)num7));
			}
			return Arrays.copyOf(array, num2);
		}
		
		public Downsampler()
		{
		}
	}
}
