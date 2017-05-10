using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend.frequencywarp
{
	public class MelFilter : java.lang.Object
	{
		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			13,
			232,
			72,
			106,
			240,
			70,
			115,
			110,
			105,
			240,
			71,
			111,
			199,
			99,
			176,
			236,
			69,
			241,
			69,
			105,
			234,
			71,
			104,
			230,
			75,
			102,
			179,
			244,
			50,
			241,
			85,
			104,
			107
		})]
		
		public MelFilter(double leftEdge, double centerFreq, double rightEdge, double initialFreq, double deltaFreq)
		{
			if (deltaFreq == (double)0f)
			{
				string text = "deltaFreq has zero value";
				
				throw new IllegalArgumentException(text);
			}
			if (java.lang.Math.round(rightEdge - leftEdge) == 0L || java.lang.Math.round(centerFreq - leftEdge) == 0L || java.lang.Math.round(rightEdge - centerFreq) == 0L)
			{
				string text2 = "Filter boundaries too close";
				
				throw new IllegalArgumentException(text2);
			}
			int num = (int)java.lang.Math.round((rightEdge - leftEdge) / deltaFreq + (double)1f);
			if (num == 0)
			{
				string text3 = "Number of elements in mel is zero.";
				
				throw new IllegalArgumentException(text3);
			}
			this.weight = new double[num];
			double num2 = 2.0 / (rightEdge - leftEdge);
			double num3 = num2 / (centerFreq - leftEdge);
			double num4 = num2 / (centerFreq - rightEdge);
			double num5 = initialFreq;
			int num6 = 0;
			while (num5 <= rightEdge)
			{
				if (num5 < centerFreq)
				{
					this.weight[num6] = num3 * (num5 - leftEdge);
				}
				else
				{
					this.weight[num6] = num2 + num4 * (num5 - centerFreq);
				}
				num5 += deltaFreq;
				num6++;
			}
			this.initialFreqIndex = (int)java.lang.Math.round(initialFreq / deltaFreq);
		}

		[LineNumberTable(new byte[]
		{
			100,
			166,
			108,
			105,
			101,
			240,
			61,
			230,
			70
		})]
		public virtual double filterOutput(double[] spectrum)
		{
			double num = (double)0f;
			for (int i = 0; i < this.weight.Length; i++)
			{
				int num2 = this.initialFreqIndex + i;
				if (num2 < spectrum.Length)
				{
					num += spectrum[num2] * this.weight[i];
				}
			}
			return num;
		}

		private double[] weight;

		private int initialFreqIndex;
	}
}
