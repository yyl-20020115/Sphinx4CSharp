namespace edu.cmu.sphinx.frontend.feature
{
	public class ConcatFeatureExtractor : AbstractFeatureExtractor
	{		
		public ConcatFeatureExtractor(int window) : base(window)
		{
		}
		
		public ConcatFeatureExtractor()
		{
		}
		
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
