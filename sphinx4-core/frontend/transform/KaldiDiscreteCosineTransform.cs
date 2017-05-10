using System;

using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.transform
{
	public class KaldiDiscreteCosineTransform : DiscreteCosineTransform
	{
		[LineNumberTable(new byte[]
		{
			159,
			166,
			104
		})]
		
		public KaldiDiscreteCosineTransform(int numberMelFilters, int cepstrumSize) : base(numberMelFilters, cepstrumSize)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			169,
			102
		})]
		
		public KaldiDiscreteCosineTransform()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			174,
			127,
			20,
			159,
			0,
			152,
			110,
			151,
			109,
			63,
			4,
			232,
			61,
			233,
			70
		})]
		
		protected internal override void computeMelCosine()
		{
			int cepstrumSize = this.cepstrumSize;
			int numberMelFilters = this.numberMelFilters;
			int[] array = new int[2];
			int num = numberMelFilters;
			array[1] = num;
			num = cepstrumSize;
			array[0] = num;
			this.melcosine = (double[][])ByteCodeHelper.multianewarray(typeof(double[][]).TypeHandle, array);
			Arrays.fill(this.melcosine[0], java.lang.Math.sqrt((double)1f / (double)this.numberMelFilters));
			double num2 = java.lang.Math.sqrt(2.0 / (double)this.numberMelFilters);
			for (int i = 1; i < this.cepstrumSize; i++)
			{
				double num3 = 3.1415926535897931 * (double)i / (double)this.numberMelFilters;
				for (int j = 0; j < this.numberMelFilters; j++)
				{
					this.melcosine[i][j] = num2 * java.lang.Math.cos(num3 * ((double)j + 0.5));
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			189,
			140,
			103,
			107,
			61,
			38,
			230,
			69
		})]
		protected internal override double[] applyMelCosine(double[] melspectrum)
		{
			double[] array = new double[this.cepstrumSize];
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < this.numberMelFilters; j++)
				{
					double[] array2 = array;
					int num = i;
					double[] array3 = array2;
					array3[num] += melspectrum[j] * this.melcosine[i][j];
				}
			}
			return array;
		}
	}
}
