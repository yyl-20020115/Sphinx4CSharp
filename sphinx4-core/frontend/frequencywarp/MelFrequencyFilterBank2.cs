using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.frontend.frequencywarp
{
	public class MelFrequencyFilterBank2 : BaseDataProcessor
	{	
		public MelFrequencyFilterBank2(double minFreq, double maxFreq, int numberFilters)
		{
			this.initLogger();
			this.minFreq = minFreq;
			this.maxFreq = maxFreq;
			this.numberFilters = numberFilters;
		}
		
		private double linearToMel(double num)
		{
			return 1127.0 * java.lang.Math.log1p(num / 700.0);
		}
		
		private void buildFilterbank(int num, int num2, double num3, double num4)
		{
			if (!MelFrequencyFilterBank2.assertionsDisabled && num <= 0)
			{
				
				throw new AssertionError();
			}
			if (!MelFrequencyFilterBank2.assertionsDisabled && num2 <= 0)
			{
				
				throw new AssertionError();
			}
			double num5 = this.linearToMel(num3);
			double num6 = this.linearToMel(num4);
			double num7 = (num6 - num5) / (double)(num2 + 1);
			double num8 = (double)this.sampleRate / (double)num;
			double[] array = new double[num / 2];
			this.filters = new MelFilter2[num2];
			for (int i = 0; i < num / 2; i++)
			{
				array[i] = this.linearToMel((double)i * num8);
			}
			for (int i = 0; i < num2; i++)
			{
				double center = num5 + (double)(i + 1) * num7;
				this.filters[i] = new MelFilter2(center, num7, array);
			}
		}
		
		private DoubleData process(DoubleData doubleData)
		{
			double[] values = doubleData.getValues();
			int num = values.Length - 1 << 1;
			if (this.filters == null || this.sampleRate != doubleData.getSampleRate())
			{
				this.sampleRate = doubleData.getSampleRate();
				this.buildFilterbank(num, this.numberFilters, this.minFreq, this.maxFreq);
			}
			else if (values.Length != (num >> 1) + 1)
			{
				string text = new StringBuilder().append("Window size is incorrect: in.length == ").append(values.Length).append(", numberFftPoints == ").append((num >> 1) + 1).toString();
				
				throw new IllegalArgumentException(text);
			}
			double[] array = new double[this.numberFilters];
			for (int i = 0; i < this.numberFilters; i++)
			{
				array[i] = this.filters[i].apply(values);
			}
			return new DoubleData(array, this.sampleRate, doubleData.getFirstSampleNumber());
		}
		
		public MelFrequencyFilterBank2()
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
		
		static MelFrequencyFilterBank2()
		{
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

		private int numberFilters;

		private double minFreq;

		private double maxFreq;

		private MelFilter2[] filters;

		internal static bool assertionsDisabled = !ClassLiteral<MelFrequencyFilterBank2>.Value.desiredAssertionStatus();
	}
}
