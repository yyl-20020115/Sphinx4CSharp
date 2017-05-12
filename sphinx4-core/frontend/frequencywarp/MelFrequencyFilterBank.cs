using edu.cmu.sphinx.util.props;
using java.lang;

namespace edu.cmu.sphinx.frontend.frequencywarp
{
	public class MelFrequencyFilterBank : BaseDataProcessor
	{		
		public MelFrequencyFilterBank(double minFreq, double maxFreq, int numberFilters)
		{
			this.initLogger();
			this.minFreq = minFreq;
			this.maxFreq = maxFreq;
			this.numberFilters = numberFilters;
		}
		
		private double linToMelFreq(double num)
		{
			return 2595.0 * (Math.log((double)1f + num / 700.0) / Math.log(10.0));
		}
		
		private double setToNearestFrequencyBin(double num, double num2)
		{
			if (num2 == (double)0f)
			{
				string text = "stepFreq is zero";
				
				throw new IllegalArgumentException(text);
			}
			return num2 * (double)Math.round(num / num2);
		}
		
		private double melToLinFreq(double num)
		{
			return 700.0 * (Math.pow(10.0, num / 2595.0) - (double)1f);
		}
		
		private void buildFilterbank(int num, int num2, double num3, double num4)
		{
			double[] array = new double[num2];
			double[] array2 = new double[num2];
			double[] array3 = new double[num2];
			this.filter = new MelFilter[num2];
			if (num == 0)
			{
				string text = "Number of FFT points is zero";
				
				throw new IllegalArgumentException(text);
			}
			double num5 = (double)this.sampleRate / (double)num;
			if (num2 < 1)
			{
				string text2 = new StringBuilder().append("Number of filters illegal: ").append(num2).toString();
				
				throw new IllegalArgumentException(text2);
			}
			double num6 = this.linToMelFreq(num3);
			double num7 = this.linToMelFreq(num4);
			double num8 = (num7 - num6) / (double)(num2 + 1);
			array[0] = this.setToNearestFrequencyBin(num3, num5);
			double num9 = num6;
			double num10;
			for (int i = 0; i < num2; i++)
			{
				num9 += num8;
				num10 = this.melToLinFreq(num9);
				array2[i] = this.setToNearestFrequencyBin(num10, num5);
				if (i > 0)
				{
					array3[i - 1] = array2[i];
				}
				if (i < num2 - 1)
				{
					array[i + 1] = array2[i];
				}
			}
			num9 += num8;
			num10 = this.melToLinFreq(num9);
			array3[num2 - 1] = this.setToNearestFrequencyBin(num10, num5);
			for (int i = 0; i < num2; i++)
			{
				double num11 = this.setToNearestFrequencyBin(array[i], num5);
				if (num11 < array[i])
				{
					num11 += num5;
				}
				this.filter[i] = new MelFilter(array[i], array2[i], array3[i], num11, num5);
			}
		}
		
		private DoubleData process(DoubleData doubleData)
		{
			double[] values = doubleData.getValues();
			if (this.filter == null || this.sampleRate != doubleData.getSampleRate())
			{
				this.numberFftPoints = values.Length - 1 << 1;
				this.sampleRate = doubleData.getSampleRate();
				this.buildFilterbank(this.numberFftPoints, this.numberFilters, this.minFreq, this.maxFreq);
			}
			else if (values.Length != (this.numberFftPoints >> 1) + 1)
			{
				string text = new StringBuilder().append("Window size is incorrect: in.length == ").append(values.Length).append(", numberFftPoints == ").append((this.numberFftPoints >> 1) + 1).toString();
				
				throw new IllegalArgumentException(text);
			}
			double[] array = new double[this.numberFilters];
			for (int i = 0; i < this.numberFilters; i++)
			{
				array[i] = this.filter[i].filterOutput(values);
			}
			return new DoubleData(array, this.sampleRate, doubleData.getFirstSampleNumber());
		}
		
		public MelFrequencyFilterBank()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.minFreq = ps.getDouble("minimumFrequency");
			this.maxFreq = ps.getDouble("maximumFrequency");
			this.numberFilters = ps.getInt("numberFilters");
		}
		
		public override void initialize()
		{
			base.initialize();
		}
		
		public override Data getData()
		{
			object obj = this.getPredecessor().getData();
			if ((Data)obj != null && ((Data)obj) is DoubleData)
			{
				obj = this.process((DoubleData)((Data)obj));
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
			40
		})]
		public const string PROP_NUMBER_FILTERS = "numberFilters";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			130.0
		})]
		public const string PROP_MIN_FREQ = "minimumFrequency";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			6800.0
		})]
		public const string PROP_MAX_FREQ = "maximumFrequency";

		private int sampleRate;

		private int numberFftPoints;

		private int numberFilters;

		private double minFreq;

		private double maxFreq;

		private MelFilter[] filter;
	}
}
