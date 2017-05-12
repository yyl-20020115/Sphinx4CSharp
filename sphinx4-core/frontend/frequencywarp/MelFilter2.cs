using java.util;
using java.lang;

namespace edu.cmu.sphinx.frontend.frequencywarp
{
	public class MelFilter2 : Object
	{		
		public MelFilter2(double center, double delta, double[] melPoints)
		{
			int num = 0;
			int num2 = melPoints.Length;
			double num3 = center - delta;
			double num4 = center + delta;
			double[] array = new double[melPoints.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (num3 < melPoints[i] && melPoints[i] <= center)
				{
					array[i] = (melPoints[i] - num3) / (center - num3);
					num2 = Math.min(i, num2);
					num = i;
				}
				if (center < melPoints[i] && melPoints[i] < num4)
				{
					array[i] = (num4 - melPoints[i]) / (num4 - center);
					num = i;
				}
			}
			this.offset = num2;
			this.weights = Arrays.copyOfRange(array, num2, num + 1);
		}
		public virtual double apply(double[] powerSpectrum)
		{
			double num = (double)0f;
			for (int i = 0; i < this.weights.Length; i++)
			{
				num += this.weights[i] * powerSpectrum[this.offset + i];
			}
			return num;
		}
		
		private int offset;
		
		private double[] weights;
	}
}
