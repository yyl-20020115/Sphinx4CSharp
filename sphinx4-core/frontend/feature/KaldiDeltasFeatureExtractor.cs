namespace edu.cmu.sphinx.frontend.feature
{
	public class KaldiDeltasFeatureExtractor : AbstractFeatureExtractor
	{	
		public KaldiDeltasFeatureExtractor(int window) : base(window)
		{
		}
	
		public KaldiDeltasFeatureExtractor()
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
			int num7 = this.currentPosition - 4 + this.cepstraBufferSize;
			int cepstraBufferSize4 = this.cepstraBufferSize;
			int num8 = (cepstraBufferSize4 != -1) ? (num7 % cepstraBufferSize4) : 0;
			int num9 = this.currentPosition + 1;
			int cepstraBufferSize5 = this.cepstraBufferSize;
			int num10 = (cepstraBufferSize5 != -1) ? (num9 % cepstraBufferSize5) : 0;
			int num11 = this.currentPosition + 2;
			int cepstraBufferSize6 = this.cepstraBufferSize;
			int num12 = (cepstraBufferSize6 != -1) ? (num11 % cepstraBufferSize6) : 0;
			int num13 = this.currentPosition + 3;
			int cepstraBufferSize7 = this.cepstraBufferSize;
			int num14 = (cepstraBufferSize7 != -1) ? (num13 % cepstraBufferSize7) : 0;
			int num15 = this.currentPosition + 4;
			int cepstraBufferSize8 = this.cepstraBufferSize;
			int num16 = (cepstraBufferSize8 != -1) ? (num15 % cepstraBufferSize8) : 0;
			DoubleData doubleData = this.cepstraBuffer[this.currentPosition];
			double[] values = this.cepstraBuffer[num16].getValues();
			double[] values2 = this.cepstraBuffer[num14].getValues();
			double[] values3 = this.cepstraBuffer[num12].getValues();
			double[] values4 = this.cepstraBuffer[num10].getValues();
			double[] values5 = doubleData.getValues();
			double[] values6 = this.cepstraBuffer[num2].getValues();
			double[] values7 = this.cepstraBuffer[num4].getValues();
			double[] values8 = this.cepstraBuffer[num6].getValues();
			double[] values9 = this.cepstraBuffer[num8].getValues();
			float[] array = new float[values5.Length * 3];
			int num17 = this.currentPosition + 1;
			int cepstraBufferSize9 = this.cepstraBufferSize;
			this.currentPosition = ((cepstraBufferSize9 != -1) ? (num17 % cepstraBufferSize9) : 0);
			int num18 = 0;
			double[] array2 = values5;
			int num19 = array2.Length;
			for (int i = 0; i < num19; i++)
			{
				double num20 = array2[i];
				float[] array3 = array;
				int num21 = num18;
				num18++;
				array3[num21] = (float)num20;
			}
			for (int j = 0; j < values3.Length; j++)
			{
				float[] array4 = array;
				int num22 = num18;
				num18++;
				array4[num22] = (float)(2.0 * values3[j] + values4[j] - values6[j] - 2.0 * values7[j]) / 10f;
			}
			for (int j = 0; j < values2.Length; j++)
			{
				float[] array5 = array;
				int num23 = num18;
				num18++;
				array5[num23] = (float)(4.0 * values[j] + 4.0 * values2[j] + values3[j] - 4.0 * values4[j] - 10.0 * values5[j] + (4.0 * values9[j] + 4.0 * values8[j] + values7[j] - 4.0 * values6[j])) / 100f;
			}
			return new FloatData(array, doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
		}
	}
}
