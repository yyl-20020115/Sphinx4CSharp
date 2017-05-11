using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.frontend.filter
{
	public class Preemphasizer : BaseDataProcessor
	{
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
		
		public Preemphasizer(double preemphasisFactor)
		{
			this.initLogger();
			this.preemphasisFactor = preemphasisFactor;
		}
		
		public Preemphasizer()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.preemphasisFactor = ps.getDouble("factor");
		}
		
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
