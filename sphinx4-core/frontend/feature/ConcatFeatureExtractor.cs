using System;

using IKVM.Attributes;

namespace edu.cmu.sphinx.frontend.feature
{
	public class ConcatFeatureExtractor : AbstractFeatureExtractor
	{
		[LineNumberTable(new byte[]
		{
			159,
			169,
			103
		})]
		
		public ConcatFeatureExtractor(int window) : base(window)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			172,
			102
		})]
		
		public ConcatFeatureExtractor()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			182,
			110,
			120,
			98,
			116,
			127,
			2,
			112,
			121,
			42,
			232,
			61,
			233,
			71,
			158,
			98,
			102,
			10
		})]
		
		protected internal override Data computeNextFeature()
		{
			DoubleData doubleData = this.cepstraBuffer[this.currentPosition];
			float[] array = new float[(this.window * 2 + 1) * doubleData.getValues().Length];
			int num = 0;
			for (int i = -this.window; i <= this.window; i++)
			{
				int num2 = this.currentPosition + i + this.cepstraBufferSize;
				int cepstraBufferSize = this.cepstraBufferSize;
				int num3 = (cepstraBufferSize != -1) ? (num2 % cepstraBufferSize) : 0;
				double[] values = this.cepstraBuffer[num3].getValues();
				double[] array2 = values;
				int num4 = array2.Length;
				for (int j = 0; j < num4; j++)
				{
					double num5 = array2[j];
					float[] array3 = array;
					int num6 = num;
					num++;
					array3[num6] = (float)num5;
				}
			}
			int num7 = this.currentPosition + 1;
			int cepstraBufferSize2 = this.cepstraBufferSize;
			this.currentPosition = ((cepstraBufferSize2 != -1) ? (num7 % cepstraBufferSize2) : 0);
			return new FloatData(array, doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
		}
	}
}
