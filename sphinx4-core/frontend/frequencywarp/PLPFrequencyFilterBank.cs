﻿using edu.cmu.sphinx.util.props;
using java.lang;

namespace edu.cmu.sphinx.frontend.frequencywarp
{
	public class PLPFrequencyFilterBank : BaseDataProcessor
	{
		private double loudnessScalingFunction(double num)
		{
			double num2 = num * num;
			double num3 = num2 / (num2 + 160000.0);
			return num3 * num3 * ((num2 + 1440000.0) / (num2 + 9610000.0));
		}
		
		private void buildCriticalBandFilterbank()
		{
			int num = (this.numberFftPoints >> 1) + 1;
			FrequencyWarper frequencyWarper = new FrequencyWarper();
			this.criticalBandFilter = new PLPFilter[this.numberFilters];
			if (this.numberFftPoints == 0)
			{
				string text = "Number of FFT points is zero";
				
				throw new IllegalArgumentException(text);
			}
			if (this.numberFilters < 1)
			{
				string text2 = new StringBuilder().append("Number of filters illegal: ").append(this.numberFilters).toString();
				
				throw new IllegalArgumentException(text2);
			}
			double[] array = new double[num];
			double num2 = (double)(this.sampleRate / 2);
			for (int i = 0; i < num; i++)
			{
				array[i] = (double)i * num2 / (double)(num - 1);
			}
			double num3 = frequencyWarper.hertzToBark(this.minFreq);
			double num4 = frequencyWarper.hertzToBark(this.maxFreq);
			if (this.numberFilters < 1)
			{
				string text3 = new StringBuilder().append("Number of filters illegal: ").append(this.numberFilters).toString();
				
				throw new IllegalArgumentException(text3);
			}
			double num5 = (num4 - num3) / (double)(this.numberFilters + 1);
			for (int i = 0; i < this.numberFilters; i++)
			{
				double centerFreqInHz = frequencyWarper.barkToHertz(num3 + (double)i * num5);
				this.criticalBandFilter[i] = new PLPFilter(array, centerFreqInHz);
			}
		}
		
		private void buildEqualLoudnessScalingFactors()
		{
			this.equalLoudnessScaling = new double[this.numberFilters];
			for (int i = 0; i < this.numberFilters; i++)
			{
				double _centerFreqInHz = this.criticalBandFilter[i].__centerFreqInHz;
				this.equalLoudnessScaling[i] = this.loudnessScalingFunction(_centerFreqInHz);
			}
		}
		
		private DoubleData process(DoubleData doubleData)
		{
			double[] values = doubleData.getValues();
			if (this.criticalBandFilter == null || this.sampleRate != doubleData.getSampleRate())
			{
				this.numberFftPoints = values.Length - 1 << 1;
				this.sampleRate = doubleData.getSampleRate();
				this.buildCriticalBandFilterbank();
				this.buildEqualLoudnessScalingFactors();
			}
			else if (values.Length != (this.numberFftPoints >> 1) + 1)
			{
				string text = new StringBuilder().append("Window size is incorrect: in.length == ").append(values.Length).append(", numberFftPoints == ").append((this.numberFftPoints >> 1) + 1).toString();
				
				throw new IllegalArgumentException(text);
			}
			double[] array = new double[this.numberFilters];
			for (int i = 0; i < this.numberFilters; i++)
			{
				array[i] = this.criticalBandFilter[i].filterOutput(values);
				double[] array2 = array;
				int num = i;
				double[] array3 = array2;
				array3[num] *= this.equalLoudnessScaling[i];
			}
			return new DoubleData(array, doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
		}
		
		public PLPFrequencyFilterBank(double minFreq, double maxFreq, int numberFilters)
		{
			this.initLogger();
			this.minFreq = minFreq;
			this.maxFreq = maxFreq;
			this.numberFilters = numberFilters;
		}
		
		public PLPFrequencyFilterBank()
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
			32
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
			3600.0
		})]
		public const string PROP_MAX_FREQ = "maximumFrequency";

		private int sampleRate;

		private int numberFftPoints;

		private int numberFilters;

		private double minFreq;

		private double maxFreq;

		private PLPFilter[] criticalBandFilter;

		private double[] equalLoudnessScaling;
	}
}
