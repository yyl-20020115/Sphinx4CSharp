using edu.cmu.sphinx.util;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer
{

	internal class Buffer : Object
	{
		internal Buffer(int num, bool flag, int num2)
		{
			this.id = num2;
			this.isLog = flag;
			this._wasUsed = false;
			this.numerator = new double[num];
			if (flag)
			{
				this.denominator = -3.4028234663852886E+38;
				for (int i = 0; i < num; i++)
				{
					this.numerator[i] = -3.4028234663852886E+38;
				}
			}
		}
		
		internal void accumulate(float num, int num2)
		{
			if (!Buffer.assertionsDisabled)
			{
				
				throw new AssertionError();
			}
			if (!Buffer.assertionsDisabled && this.numerator == null)
			{
				
				throw new AssertionError();
			}
			if (!Buffer.assertionsDisabled && this.isLog)
			{
				
				throw new AssertionError();
			}
			double[] array = this.numerator;
			array[num2] += (double)num;
			this.denominator += (double)num;
			this._wasUsed = true;
		}
		
		internal void logAccumulate(float num, int num2, LogMath logMath)
		{
			if (!Buffer.assertionsDisabled && this.numerator == null)
			{
				
				throw new AssertionError();
			}
			if (!Buffer.assertionsDisabled && !this.isLog)
			{
				
				throw new AssertionError();
			}
			this.numerator[num2] = (double)logMath.addAsLinear((float)this.numerator[num2], num);
			this.denominator = (double)logMath.addAsLinear((float)this.denominator, num);
			this._wasUsed = true;
		}
	
		internal void accumulate(double[] array, double num)
		{
			if (!Buffer.assertionsDisabled && this.numerator == null)
			{
				
				throw new AssertionError();
			}
			if (!Buffer.assertionsDisabled && array == null)
			{
				
				throw new AssertionError();
			}
			if (!Buffer.assertionsDisabled && this.numerator.Length != array.Length)
			{
				
				throw new AssertionError();
			}
			if (!Buffer.assertionsDisabled && this.isLog)
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < this.numerator.Length; i++)
			{
				double[] array2 = this.numerator;
				int num2 = i;
				double[] array3 = array2;
				array3[num2] += array[i];
			}
			this.denominator += num;
			this._wasUsed = true;
		}
		
		internal void logAccumulate(float[] array, float num, LogMath logMath)
		{
			if (!Buffer.assertionsDisabled && this.numerator == null)
			{
				
				throw new AssertionError();
			}
			if (!Buffer.assertionsDisabled && array == null)
			{
				
				throw new AssertionError();
			}
			if (!Buffer.assertionsDisabled && this.numerator.Length != array.Length)
			{
				
				throw new AssertionError();
			}
			if (!Buffer.assertionsDisabled && !this.isLog)
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < this.numerator.Length; i++)
			{
				this.numerator[i] = (double)logMath.addAsLinear((float)this.numerator[i], array[i]);
			}
			this.denominator = (double)logMath.addAsLinear((float)this.denominator, num);
			this._wasUsed = true;
		}
		
		internal void normalize()
		{
			if (!Buffer.assertionsDisabled && this.isLog)
			{
				
				throw new AssertionError();
			}
			if (this.denominator == (double)0f)
			{
				java.lang.System.@out.println(new StringBuilder().append("Empty denominator: ").append(this.id).toString());
				this._wasUsed = false;
				return;
			}
			double num = (double)1f / this.denominator;
			for (int i = 0; i < this.numerator.Length; i++)
			{
				double[] array = this.numerator;
				int num2 = i;
				double[] array2 = array;
				array2[num2] *= num;
			}
			this.denominator = (double)1f;
		}

		internal void logNormalize()
		{
			if (!Buffer.assertionsDisabled && !this.isLog)
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < this.numerator.Length; i++)
			{
				double[] array = this.numerator;
				int num = i;
				double[] array2 = array;
				array2[num] -= this.denominator;
			}
			this.denominator = (double)0f;
		}

		internal void logNormalizeNonZero(float[] array)
		{
			if (!Buffer.assertionsDisabled && !this.isLog)
			{
				
				throw new AssertionError();
			}
			if (!Buffer.assertionsDisabled && array.Length != this.numerator.Length)
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < this.numerator.Length; i++)
			{
				if (array[i] != -3.40282347E+38f)
				{
					double[] array2 = this.numerator;
					int num = i;
					double[] array3 = array2;
					array3[num] -= this.denominator;
				}
			}
			this.denominator = (double)0f;
		}
	
		internal void normalizeToSum()
		{
			if (!Buffer.assertionsDisabled && this.isLog)
			{
				
				throw new AssertionError();
			}
			float num = 0f;
			double[] array = this.numerator;
			int i = array.Length;
			for (int j = 0; j < i; j++)
			{
				double num2 = array[j];
				num = (float)((double)num + num2);
			}
			float num3 = (float)((double)1f / (double)num);
			for (i = 0; i < this.numerator.Length; i++)
			{
				double[] array2 = this.numerator;
				int num4 = i;
				double[] array3 = array2;
				array3[num4] *= (double)num3;
			}
			this.denominator = (double)1f;
		}
	
		internal void logNormalizeToSum(LogMath logMath)
		{
			if (!Buffer.assertionsDisabled && !this.isLog)
			{
				
				throw new AssertionError();
			}
			float minValue = float.MinValue;
			float num = minValue;
			double[] array = this.numerator;
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				double num3 = array[i];
				if (num3 != (double)minValue)
				{
					num = logMath.addAsLinear(num, (float)num3);
				}
			}
			for (int j = 0; j < this.numerator.Length; j++)
			{
				if (this.numerator[j] != (double)minValue)
				{
					double[] array2 = this.numerator;
					int num4 = j;
					double[] array3 = array2;
					array3[num4] -= (double)num;
				}
			}
			this.denominator = (double)0f;
		}
	
		protected internal bool floor(float num)
		{
			if (!Buffer.assertionsDisabled && this.isLog)
			{
				
				throw new AssertionError();
			}
			int result = 0;
			for (int i = 0; i < this.numerator.Length; i++)
			{
				if (this.numerator[i] < (double)num)
				{
					result = 1;
					this.numerator[i] = (double)num;
				}
			}
			return result != 0;
		}
	
		protected internal bool logFloor(float num)
		{
			if (!Buffer.assertionsDisabled && !this.isLog)
			{
				
				throw new AssertionError();
			}
			int result = 0;
			for (int i = 0; i < this.numerator.Length; i++)
			{
				if (this.numerator[i] < (double)num)
				{
					result = 1;
					this.numerator[i] = (double)num;
				}
			}
			return result != 0;
		}
		
		protected internal float getValue(int num)
		{
			return (float)this.numerator[num];
		}

		protected internal void setValue(int num, float num2)
		{
			this.numerator[num] = (double)num2;
		}

		protected internal float[] getValues()
		{
			float[] array = new float[this.numerator.Length];
			for (int i = 0; i < this.numerator.Length; i++)
			{
				array[i] = (float)this.numerator[i];
			}
			return array;
		}

		protected internal bool wasUsed()
		{
			return this._wasUsed;
		}
	
		public void dump()
		{
			java.lang.System.@out.println(new StringBuilder().append("Denominator= ").append(this.denominator).toString());
			java.lang.System.@out.println("Numerators= ");
			for (int i = 0; i < this.numerator.Length; i++)
			{
				java.lang.System.@out.println(new StringBuilder().append("[").append(i).append("]= ").append(this.numerator[i]).toString());
			}
		}

		
		static Buffer()
		{
		}

		private double[] numerator;

		private double denominator;

		private bool _wasUsed;

		private bool isLog;

		private int id;

		internal static bool assertionsDisabled = !ClassLiteral<Buffer>.Value.desiredAssertionStatus();
	}
}
