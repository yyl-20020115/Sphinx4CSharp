using System;

using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;

namespace edu.cmu.sphinx.frontend.filter
{
	public class Preemphasizer : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			51,
			103,
			101,
			135,
			178,
			100,
			115,
			103,
			100,
			110,
			226,
			61,
			230,
			70,
			104
		})]
		private void applyPreemphasis(double[] array)
		{
			double num = this.prior;
			if (array.Length > 0)
			{
				num = array[array.Length - 1];
			}
			if (array.Length > 1 && this.preemphasisFactor != (double)0f)
			{
				double num2 = array[0];
				array[0] = num2 - this.preemphasisFactor * this.prior;
				for (int i = 1; i < array.Length; i++)
				{
					double num3 = array[i];
					array[i] = num3 - this.preemphasisFactor * num2;
					num2 = num3;
				}
			}
			this.prior = num;
		}

		[LineNumberTable(new byte[]
		{
			0,
			104,
			102,
			105
		})]
		
		public Preemphasizer(double preemphasisFactor)
		{
			this.initLogger();
			this.preemphasisFactor = preemphasisFactor;
		}

		[LineNumberTable(new byte[]
		{
			5,
			134
		})]
		
		public Preemphasizer()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			17,
			103,
			114
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.preemphasisFactor = ps.getDouble("factor");
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			32,
			108,
			99,
			104,
			115,
			112,
			172
		})]
		
		public override Data getData()
		{
			Data data = this.getPredecessor().getData();
			if (data != null)
			{
				if (data is DoubleData)
				{
					this.applyPreemphasis(((DoubleData)data).getValues());
				}
				else if (data is DataEndSignal || data is SpeechEndSignal)
				{
					this.prior = (double)0f;
				}
			}
			return data;
		}

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			0.97
		})]
		public const string PROP_PREEMPHASIS_FACTOR = "factor";

		private double preemphasisFactor;

		private double prior;
	}
}
