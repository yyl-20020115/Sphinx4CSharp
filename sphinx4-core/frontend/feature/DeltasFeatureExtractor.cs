namespace edu.cmu.sphinx.frontend.feature
{
	public class DeltasFeatureExtractor : AbstractFeatureExtractor
	{		
		public DeltasFeatureExtractor(int window) : base(window)
		{
		}
		
		public DeltasFeatureExtractor()
		{
		}
		
		protected internal override Data computeNextFeature()
		{
			int num = this.currentPosition - 1 + this.cepstraBufferSize;
			int cepstraBufferSize = this.cepstraBufferSize;
			int num2 = (cepstraBufferSize != -1) ? (num % cepstraBufferSize) : 0;
			int num3 = this.currentPosition - 2 + this.cepstraBufferSize;
			int cepstraBufferSize2 = this.cepstraBufferSize;
			int num4 = (cepstraBufferSize2 != -1) ? (num3 % cepstraBufferSize2) : 0;
			int num5 = this.currentPosition - 3 + this.cepstraBufferSize;
			int cepstraBufferSize3 = this.cepstraBufferSize;
			int num6 = (cepstraBufferSize3 != -1) ? (num5 % cepstraBufferSize3) : 0;
			int num7 = this.currentPosition + 1;
			int cepstraBufferSize4 = this.cepstraBufferSize;
			int num8 = (cepstraBufferSize4 != -1) ? (num7 % cepstraBufferSize4) : 0;
			int num9 = this.currentPosition + 2;
			int cepstraBufferSize5 = this.cepstraBufferSize;
			int num10 = (cepstraBufferSize5 != -1) ? (num9 % cepstraBufferSize5) : 0;
			int num11 = this.currentPosition + 3;
			int cepstraBufferSize6 = this.cepstraBufferSize;
			int num12 = (cepstraBufferSize6 != -1) ? (num11 % cepstraBufferSize6) : 0;
			DoubleData doubleData = this.cepstraBuffer[this.currentPosition];
			double[] values = this.cepstraBuffer[num12].getValues();
			double[] values2 = this.cepstraBuffer[num10].getValues();
			double[] values3 = this.cepstraBuffer[num8].getValues();
			double[] values4 = doubleData.getValues();
			double[] values5 = this.cepstraBuffer[num2].getValues();
			double[] values6 = this.cepstraBuffer[num4].getValues();
			double[] values7 = this.cepstraBuffer[num6].getValues();
			float[] array = new float[values4.Length * 3];
			int num13 = this.currentPosition + 1;
			int cepstraBufferSize7 = this.cepstraBufferSize;
			this.currentPosition = ((cepstraBufferSize7 != -1) ? (num13 % cepstraBufferSize7) : 0);
			int num14 = 0;
			double[] array2 = values4;
			int num15 = array2.Length;
			for (int i = 0; i < num15; i++)
			{
				double num16 = array2[i];
				float[] array3 = array;
				int num17 = num14;
				num14++;
				array3[num17] = (float)num16;
			}
			for (int j = 0; j < values2.Length; j++)
			{
				float[] array4 = array;
				int num18 = num14;
				num14++;
				array4[num18] = (float)(values2[j] - values6[j]);
			}
			for (int j = 0; j < values.Length; j++)
			{
				float[] array5 = array;
				int num19 = num14;
				num14++;
				array5[num19] = (float)(values[j] - values5[j] - (values3[j] - values7[j]));
			}
			return new FloatData(array, doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
		}
	}
}
