using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.frontend.frequencywarp
{
	public class PLPCepstrumProducer : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			39,
			159,
			22,
			147,
			107,
			145,
			109,
			63,
			2,
			232,
			61,
			233,
			71
		})]
		
		private void computeCosine()
		{
			int num = this.LPCOrder + 1;
			int num2 = this.numberPLPFilters;
			int[] array = new int[2];
			int num3 = num2;
			array[1] = num3;
			num3 = num;
			array[0] = num3;
			this.cosine = (double[][])ByteCodeHelper.multianewarray(typeof(double[][]).TypeHandle, array);
			double num4 = 2.0 * (double)this.numberPLPFilters;
			for (int i = 0; i <= this.LPCOrder; i++)
			{
				double num5 = 6.2831853071795862 * (double)i / num4;
				for (int j = 0; j < this.numberPLPFilters; j++)
				{
					this.cosine[i][j] = java.lang.Math.cos(num5 * ((double)j + 0.5));
				}
			}
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			102,
			135,
			106,
			255,
			28,
			71,
			168,
			136,
			140,
			136,
			142,
			99,
			102,
			140
		})]
		
		private Data process(DoubleData doubleData)
		{
			double[] values = doubleData.getValues();
			if (values.Length != this.numberPLPFilters)
			{
				string text = new StringBuilder().append("PLPSpectrum size is incorrect: plpspectrum.length == ").append(values.Length).append(", numberPLPFilters == ").append(this.numberPLPFilters).toString();
				
				throw new IllegalArgumentException(text);
			}
			double[] array = this.powerLawCompress(values);
			double[] autocor = this.applyCosine(array);
			LinearPredictor linearPredictor = new LinearPredictor(this.LPCOrder);
			linearPredictor.getARFilter(autocor);
			double[] data = linearPredictor.getData(this.cepstrumSize);
			return new DoubleData(data, doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
		}

		[LineNumberTable(new byte[]
		{
			61,
			136,
			103,
			53,
			166
		})]
		
		private double[] powerLawCompress(double[] array)
		{
			double[] array2 = new double[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = java.lang.Math.pow(array[i], 0.33333333333333331);
			}
			return array2;
		}

		[LineNumberTable(new byte[]
		{
			160,
			75,
			110,
			104,
			170,
			142,
			108,
			106,
			99,
			158,
			109,
			60,
			168,
			243,
			54,
			233,
			78
		})]
		private double[] applyCosine(double[] array)
		{
			double[] array2 = new double[this.LPCOrder + 1];
			double num = (double)this.numberPLPFilters;
			double num2 = 0.5;
			for (int i = 0; i <= this.LPCOrder; i++)
			{
				if (this.numberPLPFilters > 0)
				{
					double[] array3 = this.cosine[i];
					int j = 0;
					double[] array4 = array2;
					int num3 = i;
					double[] array5 = array4;
					array5[num3] += num2 * array[j] * array3[j];
					for (j = 1; j < this.numberPLPFilters; j++)
					{
						double[] array6 = array2;
						num3 = i;
						array5 = array6;
						array5[num3] += array[j] * array3[j];
					}
					double[] array7 = array2;
					num3 = i;
					array5 = array7;
					array5[num3] /= num;
				}
			}
			return array2;
		}

		[LineNumberTable(new byte[]
		{
			5,
			104,
			102,
			103,
			103,
			103
		})]
		
		public PLPCepstrumProducer(int numberPLPFilters, int cepstrumSize, int LPCOrder)
		{
			this.initLogger();
			this.numberPLPFilters = numberPLPFilters;
			this.cepstrumSize = cepstrumSize;
			this.LPCOrder = LPCOrder;
		}

		[LineNumberTable(new byte[]
		{
			12,
			102
		})]
		
		public PLPCepstrumProducer()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			22,
			103,
			113,
			113,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.numberPLPFilters = ps.getInt("numberFilters");
			this.cepstrumSize = ps.getInt("cepstrumLength");
			this.LPCOrder = ps.getInt("lpcOrder");
		}

		[LineNumberTable(new byte[]
		{
			32,
			102,
			102
		})]
		
		public override void initialize()
		{
			base.initialize();
			this.computeCosine();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			80,
			108,
			130,
			99,
			104,
			205
		})]
		
		public override Data getData()
		{
			Data data = this.getPredecessor().getData();
			Data result = data;
			if (data != null && data is DoubleData)
			{
				result = this.process((DoubleData)data);
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

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			13
		})]
		public const string PROP_CEPSTRUM_LENGTH = "cepstrumLength";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			14
		})]
		public const string PROP_LPC_ORDER = "lpcOrder";

		private int cepstrumSize;

		private int LPCOrder;

		private int numberPLPFilters;

		private double[][] cosine;
	}
}
