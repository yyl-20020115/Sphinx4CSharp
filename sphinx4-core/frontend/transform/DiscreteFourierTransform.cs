using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using java.lang;

namespace edu.cmu.sphinx.frontend.transform
{
	public class DiscreteFourierTransform : BaseDataProcessor
	{		
		private void initializeFFT()
		{
			this.computeLogBase2(this.numberFftPoints);
			this.createWeightFft(this.numberFftPoints, this.invert);
			this.initComplexArrays();
			this.weightFftTimesFrom2 = new Complex();
			this.tempComplex = new Complex();
		}
		
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
		
		public DiscreteFourierTransform(int numberFftPoints, bool invert)
		{
			this.initLogger();
			this.numberFftPoints = numberFftPoints;
			this.isNumberFftPointsSet = (numberFftPoints != -1);
			this.invert = invert;
		}
		
		public DiscreteFourierTransform()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.logger = ps.getLogger();
			this.numberFftPoints = ps.getInt("numberFftPoints");
			this.isNumberFftPointsSet = (this.numberFftPoints != -1);
			this.invert = ps.getBoolean("invert").booleanValue();
		}
		
		public override void initialize()
		{
			base.initialize();
			if (this.isNumberFftPointsSet)
			{
				this.initializeFFT();
			}
		}
		
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
