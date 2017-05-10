using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend.frequencywarp
{
	public class PLPFilter : java.lang.Object
	{
		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			13,
			136,
			134,
			104,
			105,
			143,
			149,
			208,
			145,
			174,
			114,
			108,
			114,
			108,
			127,
			8,
			108,
			111,
			108,
			159,
			15,
			237,
			51,
			233,
			79
		})]
		
		public PLPFilter(double[] DFTFrequenciesInHz, double centerFreqInHz)
		{
			FrequencyWarper frequencyWarper = new FrequencyWarper();
			this.numDFTPoints = DFTFrequenciesInHz.Length;
			this.__centerFreqInHz = centerFreqInHz;
			this.__centerFreqInBark = frequencyWarper.hertzToBark(centerFreqInHz);
			if (centerFreqInHz < DFTFrequenciesInHz[0] || centerFreqInHz > DFTFrequenciesInHz[this.numDFTPoints - 1])
			{
				string text = "Center frequency for PLP filter out of range";
				
				throw new IllegalArgumentException(text);
			}
			this.filterCoefficients = new double[this.numDFTPoints];
			for (int i = 0; i < this.numDFTPoints; i++)
			{
				double num = frequencyWarper.hertzToBark(DFTFrequenciesInHz[i]) - this.__centerFreqInBark;
				if (num < -2.5)
				{
					this.filterCoefficients[i] = (double)0f;
				}
				else if (num <= -0.5)
				{
					this.filterCoefficients[i] = java.lang.Math.pow(10.0, num + 0.5);
				}
				else if (num <= 0.5)
				{
					this.filterCoefficients[i] = (double)1f;
				}
				else if (num <= 1.3)
				{
					this.filterCoefficients[i] = java.lang.Math.pow(10.0, -2.5 * (num - 0.5));
				}
				else
				{
					this.filterCoefficients[i] = (double)0f;
				}
			}
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			58,
			106,
			255,
			28,
			69,
			102,
			107,
			48,
			166
		})]
		
		public virtual double filterOutput(double[] spectrum)
		{
			if (spectrum.Length != this.numDFTPoints)
			{
				string text = new StringBuilder().append("Mismatch in no. of DFT points ").append(spectrum.Length).append(" in spectrum and in filter ").append(this.numDFTPoints).toString();
				
				throw new IllegalArgumentException(text);
			}
			double num = (double)0f;
			for (int i = 0; i < this.numDFTPoints; i++)
			{
				num += spectrum[i] * this.filterCoefficients[i];
			}
			return num;
		}

		
		public double centerFreqInHz
		{
			
			get
			{
				return this.__centerFreqInHz;
			}
			
			private set
			{
				this.__centerFreqInHz = value;
			}
		}

		
		public double centerFreqInBark
		{
			
			get
			{
				return this.__centerFreqInBark;
			}
			
			private set
			{
				this.__centerFreqInBark = value;
			}
		}

		private double[] filterCoefficients;

		
		private int numDFTPoints;

		internal double __centerFreqInHz;

		internal double __centerFreqInBark;
	}
}
