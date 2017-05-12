using edu.cmu.sphinx.util.props;
using java.lang;

namespace edu.cmu.sphinx.frontend.transform
{
	public class Lifter : BaseDataProcessor
	{		
		public Lifter(int lifterValue)
		{
			this.initLogger();
			this.lifterValue = lifterValue;
		}
		
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
		
		private void computeLifterWeights()
		{
			this.lifterWeights = new double[this.cepstrumSize];
			for (int i = 0; i < this.cepstrumSize; i++)
			{
				this.lifterWeights[i] = (double)1f + (double)(this.lifterValue / 2) * Math.sin((double)i * 3.1415926535897931 / (double)this.lifterValue);
			}
		}
		
		public Lifter()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.lifterValue = ps.getInt("lifterValue");
		}
		
		public override void initialize()
		{
			base.initialize();
		}
		
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
