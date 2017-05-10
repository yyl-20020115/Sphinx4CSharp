using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend.transform
{
	public class Lifter : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			159,
			178,
			104,
			102,
			103
		})]
		
		public Lifter(int lifterValue)
		{
			this.initLogger();
			this.lifterValue = lifterValue;
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			31,
			135,
			104,
			104,
			104,
			106,
			255,
			28,
			70,
			103,
			48,
			166
		})]
		
		private void liftCepstrum(DoubleData doubleData)
		{
			double[] values = doubleData.getValues();
			if (this.lifterWeights == null)
			{
				this.cepstrumSize = values.Length;
				this.computeLifterWeights();
			}
			else if (values.Length != this.cepstrumSize)
			{
				string text = new StringBuilder().append("MelCepstrum size is incorrect: melcepstrum.length == ").append(values.Length).append(", cepstrumSize == ").append(this.cepstrumSize).toString();
				
				throw new IllegalArgumentException(text);
			}
			for (int i = 0; i < values.Length; i++)
			{
				values[i] *= this.lifterWeights[i];
			}
		}

		[LineNumberTable(new byte[]
		{
			52,
			113,
			107,
			127,
			10,
			9,
			198
		})]
		
		private void computeLifterWeights()
		{
			this.lifterWeights = new double[this.cepstrumSize];
			for (int i = 0; i < this.cepstrumSize; i++)
			{
				this.lifterWeights[i] = (double)1f + (double)(this.lifterValue / 2) * java.lang.Math.sin((double)i * 3.1415926535897931 / (double)this.lifterValue);
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			183,
			102
		})]
		
		public Lifter()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			188,
			103,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.lifterValue = ps.getInt("lifterValue");
		}

		[LineNumberTable(new byte[]
		{
			2,
			102
		})]
		
		public override void initialize()
		{
			base.initialize();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			16,
			108,
			107,
			140
		})]
		
		public override Data getData()
		{
			Data data = this.getPredecessor().getData();
			if (data != null && data is DoubleData)
			{
				this.liftCepstrum((DoubleData)data);
			}
			return data;
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			22
		})]
		public const string PROP_LIFTER_VALUE = "lifterValue";

		protected internal int lifterValue;

		protected internal int cepstrumSize;

		protected internal double[] lifterWeights;
	}
}
