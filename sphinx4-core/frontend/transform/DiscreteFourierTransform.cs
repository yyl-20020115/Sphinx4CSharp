using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend.transform
{
	public class DiscreteFourierTransform : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			74,
			108,
			114,
			102,
			107,
			107
		})]
		
		private void initializeFFT()
		{
			this.computeLogBase2(this.numberFftPoints);
			this.createWeightFft(this.numberFftPoints, this.invert);
			this.initComplexArrays();
			this.weightFftTimesFrom2 = new Complex();
			this.tempComplex = new Complex();
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			104,
			103,
			134,
			114,
			31,
			6,
			244,
			70
		})]
		
		private void computeLogBase2(int num)
		{
			this.logBase2NumberFftPoints = 0;
			int i = num;
			while (i > 1)
			{
				bool flag = i != 0;
				int num2 = 2;
				if ((num2 != -1 && (flag ? 1 : 0) % num2 != 0) || num < 0)
				{
					string text = new StringBuilder().append("Not a power of 2: ").append(num).toString();
					
					throw new IllegalArgumentException(text);
				}
				i >>= 1;
				this.logBase2NumberFftPoints++;
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			82,
			130,
			238,
			70,
			110,
			99,
			163,
			104,
			63,
			0,
			166
		})]
		
		private void createWeightFft(int num, bool flag)
		{
			this.weightFft = new Complex[num >> 1];
			double num2 = -6.2831853071795862 / (double)num;
			if (flag)
			{
				num2 = -num2;
			}
			for (int i = 0; i < num >> 1; i++)
			{
				this.weightFft[i] = new Complex(java.lang.Math.cos(num2 * (double)i), java.lang.Math.sin(num2 * (double)i));
			}
		}

		[LineNumberTable(new byte[]
		{
			85,
			113,
			113,
			145,
			107,
			109,
			109,
			237,
			61,
			230,
			69
		})]
		
		private void initComplexArrays()
		{
			this.inputFrame = new Complex[this.numberFftPoints];
			this.from = new Complex[this.numberFftPoints];
			this.to = new Complex[this.numberFftPoints];
			for (int i = 0; i < this.numberFftPoints; i++)
			{
				this.inputFrame[i] = new Complex();
				this.from[i] = new Complex();
				this.to[i] = new Complex();
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			53,
			99,
			99,
			136,
			227,
			70,
			102,
			109,
			17,
			230,
			73,
			246,
			70,
			106,
			104,
			49,
			200,
			104,
			49,
			198
		})]
		
		private void recurseFft(Complex[] array, double[] array2, int num, bool flag)
		{
			double b;
			if (!flag)
			{
				b = (double)1f;
			}
			else
			{
				b = (double)num;
			}
			for (int i = 0; i < num; i++)
			{
				this.to[i].reset();
				this.from[i].scaleComplex(array[i], b);
			}
			this.butterflyStage(this.from, this.to, num, num >> 1);
			if ((this.logBase2NumberFftPoints & 1) == 0)
			{
				for (int i = 0; i <= num >> 1; i++)
				{
					array2[i] = this.from[i].squaredMagnitudeComplex();
				}
			}
			else
			{
				for (int i = 0; i <= num >> 1; i++)
				{
					array2[i] = this.to[i].squaredMagnitudeComplex();
				}
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			204,
			130,
			100,
			100,
			100,
			191,
			6
		})]
		
		private static int getNumberFftPoints(int num)
		{
			int i = 1;
			while (i < num)
			{
				i <<= 1;
				if (i < 1)
				{
					string text = new StringBuilder().append("Invalid # of FFT points: ").append(i).toString();
					
					throw new Error(text);
				}
			}
			return i;
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			115,
			135,
			109,
			98,
			105,
			53,
			166,
			101,
			115,
			127,
			23,
			229,
			61,
			230,
			69,
			98,
			98,
			101,
			53,
			166,
			105,
			45,
			230,
			72,
			240,
			69,
			249,
			69,
			98,
			102,
			139
		})]
		
		private DoubleData process(DoubleData doubleData)
		{
			double[] values = doubleData.getValues();
			if (this.numberFftPoints < values.Length)
			{
				int i;
				for (i = 0; i < this.numberFftPoints; i++)
				{
					this.inputFrame[i].set(values[i], (double)0f);
				}
				while (i < values.Length)
				{
					this.tempComplex.set(values[i], (double)0f);
					Complex[] array = this.inputFrame;
					int num = i;
					int num2 = this.numberFftPoints;
					Complex complex = array[(num2 != -1) ? (num % num2) : 0];
					Complex[] array2 = this.inputFrame;
					int num3 = i;
					int num4 = this.numberFftPoints;
					complex.addComplex(array2[(num4 != -1) ? (num3 % num4) : 0], this.tempComplex);
					i++;
				}
			}
			else
			{
				int i;
				for (i = 0; i < values.Length; i++)
				{
					this.inputFrame[i].set(values[i], (double)0f);
				}
				while (i < this.numberFftPoints)
				{
					this.inputFrame[i].reset();
					i++;
				}
			}
			double[] array3 = new double[(this.numberFftPoints >> 1) + 1];
			this.recurseFft(this.inputFrame, array3, this.numberFftPoints, this.invert);
			return new DoubleData(array3, doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
		}

		[LineNumberTable(new byte[]
		{
			161,
			46,
			136,
			133,
			106,
			98,
			101,
			99,
			103,
			99,
			234,
			69,
			114,
			229,
			69,
			109,
			229,
			69,
			109,
			101,
			100,
			100,
			103,
			103,
			236,
			35,
			233,
			105,
			141
		})]
		
		private void butterflyStage(Complex[] array, Complex[] array2, int num, int num2)
		{
			if (num2 > 0)
			{
				int num3 = 2 * num2;
				for (int i = 0; i < num2; i++)
				{
					int num4 = i;
					int num5 = i + num2;
					int num6 = i;
					int num7 = i + (num >> 1);
					for (int j = 0; j < num >> 1; j += num2)
					{
						this.weightFftTimesFrom2.multiplyComplex(this.weightFft[j], array[num5]);
						array2[num6].addComplex(array[num4], this.weightFftTimesFrom2);
						array2[num7].subtractComplex(array[num4], this.weightFftTimesFrom2);
						num4 += num3;
						num5 += num3;
						num6 += num2;
						num7 += num2;
					}
				}
				this.butterflyStage(array2, array, num, num2 >> 1);
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			123,
			98,
			104,
			102,
			103,
			109,
			103
		})]
		
		public DiscreteFourierTransform(int numberFftPoints, bool invert)
		{
			this.initLogger();
			this.numberFftPoints = numberFftPoints;
			this.isNumberFftPointsSet = (numberFftPoints != -1);
			this.invert = invert;
		}

		[LineNumberTable(new byte[]
		{
			34,
			102
		})]
		
		public DiscreteFourierTransform()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			44,
			103,
			108,
			113,
			114,
			118
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.logger = ps.getLogger();
			this.numberFftPoints = ps.getInt("numberFftPoints");
			this.isNumberFftPointsSet = (this.numberFftPoints != -1);
			this.invert = ps.getBoolean("invert").booleanValue();
		}

		[LineNumberTable(new byte[]
		{
			57,
			102,
			104,
			134
		})]
		
		public override void initialize()
		{
			base.initialize();
			if (this.isNumberFftPointsSet)
			{
				this.initializeFFT();
			}
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			155,
			140,
			123,
			108,
			232,
			70,
			114,
			109,
			113,
			235,
			70,
			111,
			109,
			108,
			105,
			255,
			34,
			70,
			232,
			72
		})]
		
		public override Data getData()
		{
			object obj = this.getPredecessor().getData();
			if ((Data)obj != null && ((Data)obj) is DoubleData)
			{
				DoubleData doubleData = (DoubleData)((Data)obj);
				if (!this.isNumberFftPointsSet)
				{
					if (this.numberDataPoints != doubleData.getValues().Length)
					{
						this.numberDataPoints = doubleData.getValues().Length;
						this.numberFftPoints = DiscreteFourierTransform.getNumberFftPoints(this.numberDataPoints);
						this.initializeFFT();
					}
				}
				else if (this.numberDataPoints != doubleData.getValues().Length)
				{
					this.numberDataPoints = doubleData.getValues().Length;
					int num = DiscreteFourierTransform.getNumberFftPoints(this.numberDataPoints);
					if (num != this.numberFftPoints)
					{
						this.logger.warning(new StringBuilder().append("User set numberFftPoints (").append(this.numberFftPoints).append(") is not ideal (").append(num).append(')').toString());
					}
				}
				obj = this.process(doubleData);
			}
			object obj2 = obj;
			Data result;
			if (obj2 != null)
			{
				if ((result = (obj2 as Data)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			-1
		})]
		public const string PROP_NUMBER_FFT_POINTS = "numberFftPoints";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_INVERT = "invert";

		private bool isNumberFftPointsSet;

		private int numberFftPoints;

		private int logBase2NumberFftPoints;

		private int numberDataPoints;

		private bool invert;

		private Complex[] weightFft;

		private Complex[] inputFrame;

		private Complex[] from;

		private Complex[] to;

		private Complex weightFftTimesFrom2;

		private Complex tempComplex;
	}
}
