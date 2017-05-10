using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.denoise
{
	public class Denoise : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			27,
			104,
			105,
			105,
			105,
			106,
			106,
			106,
			104
		})]
		
		public Denoise(double lambdaPower, double lambdaA, double lambdaB, double lambdaT, double muT, double maxGain, int smoothWindow)
		{
			this.lambdaPower = lambdaPower;
			this.lambdaA = lambdaA;
			this.lambdaB = lambdaB;
			this.lambdaT = lambdaT;
			this.muT = muT;
			this.maxGain = maxGain;
			this.smoothWindow = smoothWindow;
		}

		[LineNumberTable(new byte[]
		{
			160,
			98,
			109,
			109,
			108,
			108,
			102,
			51,
			166
		})]
		
		private void initStatistics(double[] array, int num)
		{
			this.power = Arrays.copyOf(array, num);
			this.noise = Arrays.copyOf(array, num);
			this.floor = new double[num];
			this.peak = new double[num];
			for (int i = 0; i < num; i++)
			{
				this.floor[i] = array[i] / this.maxGain;
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			82,
			103,
			63,
			10,
			166
		})]
		private void updatePower(double[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				this.power[i] = this.lambdaPower * this.power[i] + ((double)1f - this.lambdaPower) * array[i];
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			88,
			103,
			104,
			159,
			2,
			255,
			0,
			60,
			230,
			70
		})]
		private void estimateEnvelope(double[] array, double[] array2)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] > array2[i])
				{
					array2[i] = this.lambdaA * array2[i] + ((double)1f - this.lambdaA) * array[i];
				}
				else
				{
					array2[i] = this.lambdaB * array2[i] + ((double)1f - this.lambdaB) * array[i];
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			69,
			106,
			132,
			119,
			116,
			147,
			107,
			233,
			56,
			233,
			74
		})]
		private void tempMasking(double[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				double num = array[i];
				double[] array2 = this.peak;
				int num2 = i;
				double[] array3 = array2;
				array3[num2] *= this.lambdaT;
				if (array[i] < this.lambdaT * this.peak[i])
				{
					array[i] = this.peak[i] * this.muT;
				}
				if (num > this.peak[i])
				{
					this.peak[i] = num;
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			126,
			103,
			109,
			11,
			198
		})]
		private void powerBoosting(double[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] < this.floor[i])
				{
					array[i] = this.floor[i];
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			112,
			104,
			106,
			111,
			114,
			103,
			104,
			42,
			168,
			235,
			57,
			233,
			73
		})]
		
		private double[] smooth(double[] array)
		{
			double[] array2 = new double[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				int num = java.lang.Math.max(i - this.smoothWindow, 0);
				int num2 = java.lang.Math.min(i + this.smoothWindow + 1, array.Length);
				double num3 = (double)0f;
				for (int j = num; j < num2; j++)
				{
					num3 += array[j];
				}
				array2[i] = num3 / (double)(num2 - num);
			}
			return array2;
		}

		[LineNumberTable(new byte[]
		{
			37,
			102
		})]
		
		public Denoise()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			49,
			103,
			114,
			114,
			114,
			114,
			114,
			114,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.lambdaPower = ps.getDouble("lambdaPower");
			this.lambdaA = ps.getDouble("lambdaA");
			this.lambdaB = ps.getDouble("lambdaB");
			this.lambdaT = ps.getDouble("lambdaT");
			this.muT = ps.getDouble("muT");
			this.maxGain = ps.getDouble("maxGain");
			this.smoothWindow = ps.getInt("smoothWindow");
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			61,
			172,
			104,
			103,
			103,
			103,
			103,
			130,
			104,
			162,
			103,
			103,
			131,
			104,
			136,
			135,
			146,
			104,
			104,
			63,
			4,
			200,
			142,
			136,
			136,
			104,
			104,
			127,
			0,
			31,
			8,
			200,
			138,
			104,
			56,
			200
		})]
		
		public override Data getData()
		{
			Data data = this.getPredecessor().getData();
			if (data is DataStartSignal)
			{
				this.power = null;
				this.noise = null;
				this.floor = null;
				this.peak = null;
				return data;
			}
			if (!(data is DoubleData))
			{
				return data;
			}
			DoubleData doubleData = (DoubleData)data;
			double[] values = doubleData.getValues();
			int num = values.Length;
			if (this.power == null)
			{
				this.initStatistics(values, num);
			}
			this.updatePower(values);
			this.estimateEnvelope(this.power, this.noise);
			double[] array = new double[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = java.lang.Math.max(this.power[i] - this.noise[i], (double)0f);
			}
			this.estimateEnvelope(array, this.floor);
			this.tempMasking(array);
			this.powerBoosting(array);
			double[] array2 = new double[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = array[i] / (this.power[i] + 1E-10);
				array2[i] = java.lang.Math.min(java.lang.Math.max(array2[i], (double)1f / this.maxGain), this.maxGain);
			}
			double[] array3 = this.smooth(array2);
			for (int i = 0; i < num; i++)
			{
				double[] array4 = values;
				int num2 = i;
				double[] array5 = array4;
				array5[num2] *= array3[i];
			}
			return data;
		}

		internal double[] power;

		internal double[] noise;

		internal double[] floor;

		internal double[] peak;

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.7
		})]
		public const string LAMBDA_POWER = "lambdaPower";

		internal double lambdaPower;

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.995
		})]
		public const string LAMBDA_A = "lambdaA";

		internal double lambdaA;

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.5
		})]
		public const string LAMBDA_B = "lambdaB";

		internal double lambdaB;

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.85
		})]
		public const string LAMBDA_T = "lambdaT";

		internal double lambdaT;

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.2
		})]
		public const string MU_T = "muT";

		internal double muT;

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			20.0
		})]
		public const string MAX_GAIN = "maxGain";

		internal double maxGain;

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			4
		})]
		public const string SMOOTH_WINDOW = "smoothWindow";

		internal int smoothWindow;

		internal const double EPS = 1E-10;
	}
}
