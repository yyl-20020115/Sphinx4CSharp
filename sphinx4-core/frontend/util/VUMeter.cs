using java.lang;

namespace edu.cmu.sphinx.frontend.util
{
	public class VUMeter : Object
	{
		private void calculateVULevels(double[] array)
		{
			double num = (double)0f;
			this.average = (double)0f;
			double num2 = (double)0f;
			double num3 = (double)0f;
			for (int i = 0; i < array.Length; i++)
			{
				double num4 = array[i];
				double num5 = (double)0f;
				double num6 = (double)0f;
				if (i > 0)
				{
					num5 = array[i - 1];
				}
				if (i > 1)
				{
					num6 = array[i - 2];
				}
				double num7 = 0.9779999852180481 * num4 + -1.9560999870300293 * num5 + 0.9779999852180481 * num6 - -1.9556000232696533 * num2 - 0.95649999380111694 * num3;
				num3 = num2;
				num2 = num7;
				double num8 = Math.abs(num7);
				long num9 = java.lang.System.currentTimeMillis();
				num += num8 * num8;
				this.average += num8;
				if (num8 > this.peak)
				{
					this.peak = num8;
				}
				else if (num9 - this.then > (long)((ulong)1000))
				{
					this.peak = num8;
					this.then = num9;
				}
			}
			this.rms = num / (double)array.Length;
			this.rms = Math.sqrt(this.rms);
			this.average /= (double)array.Length;
		}
		
		private void calculateVULevels(short[] array)
		{
			double num = (double)0f;
			this.average = (double)0f;
			double num2 = (double)0f;
			double num3 = (double)0f;
			for (int i = 0; i < array.Length; i++)
			{
				int num4 = (int)array[i];
				double num5 = (double)0f;
				double num6 = (double)0f;
				if (i > 0)
				{
					num5 = (double)array[i - 1];
				}
				if (i > 1)
				{
					num6 = (double)array[i - 2];
				}
				double num7 = (double)(0.978f * (float)num4) + -1.9560999870300293 * num5 + 0.9779999852180481 * num6 - -1.9556000232696533 * num2 - 0.95649999380111694 * num3;
				num3 = num2;
				num2 = num7;
				double num8 = Math.abs(num7);
				long num9 = java.lang.System.currentTimeMillis();
				num += num8 * num8;
				this.average += num8;
				if (num8 > this.peak)
				{
					this.peak = num8;
				}
				else if (num9 - this.then > (long)((ulong)1000))
				{
					this.peak = num8;
					this.then = num9;
				}
			}
			this.rms = num / (double)array.Length;
			this.rms = Math.sqrt(this.rms);
			this.average /= (double)array.Length;
		}
		
		public VUMeter()
		{
			this.peakHoldTime = 1000;
			this.then = java.lang.System.currentTimeMillis();
			this.a2 = -1.9556f;
			this.a3 = 0.9565f;
			this.b1 = 0.978f;
			this.b2 = -1.9561f;
			this.b3 = 0.978f;
		}
		
		public double getRmsDB()
		{
			return Math.max((double)0f, 20.0 * Math.log(this.rms) / VUMeter.log10);
		}
		
		public double getAverageDB()
		{
			return Math.max((double)0f, 20.0 * Math.log(this.average) / VUMeter.log10);
		}
		
		public double getPeakDB()
		{
			return Math.max((double)0f, 20.0 * Math.log(this.peak) / VUMeter.log10);
		}
		public bool getIsClipping()
		{
			return 32767.0 < 2.0 * this.peak;
		}
		
		public double getMaxDB()
		{
			return VUMeter.maxDB;
		}
		
		public virtual void calculateVULevels(Data data)
		{
			if (data is DoubleData)
			{
				double[] values = ((DoubleData)data).getValues();
				this.calculateVULevels(values);
			}
		}
		
		public virtual void calculateVULevels(byte[] data, int offset, int cnt)
		{
			short[] array = new short[cnt / 2];
			for (int i = 0; i < cnt / 2; i++)
			{
				int num = offset + 2 * i;
				array[i] = (short)((int)data[num] << 8 | (int)(byte.MaxValue & data[num + 1]));
			}
			this.calculateVULevels(array);
		}

		private double rms;

		private double average;

		private double peak;
	
		private static double log10 = Math.log(10.0);
		
		private static double maxDB = Math.max((double)0f, 20.0 * Math.log(32767.0) / VUMeter.log10);
		
		private int peakHoldTime;

		private long then;
		
		private float a2;
		
		private float a3;
		
		private float b1;

		private float b2;

		private float b3;
	}
}
