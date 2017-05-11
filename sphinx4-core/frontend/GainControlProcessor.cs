using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.frontend
{
	public class GainControlProcessor : BaseDataProcessor
	{		
		public GainControlProcessor(double gainFactor)
		{
			this.initLogger();
			this.gainFactor = gainFactor;
		}
		
		public GainControlProcessor()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.gainFactor = ps.getDouble("gainFactor");
		}
	
		public override Data getData()
		{
			Data data = this.getPredecessor().getData();
			if (data is FloatData)
			{
				float[] values = ((FloatData)data).getValues();
				if (this.gainFactor != (double)1f)
				{
					for (int i = 0; i < values.Length; i++)
					{
						float[] array = values;
						int num = i;
						float[] array2 = array;
						array2[num] = (float)((double)array2[num] * this.gainFactor);
					}
				}
			}
			else if (data is DoubleData)
			{
				double[] values2 = ((DoubleData)data).getValues();
				if (this.gainFactor != (double)1f)
				{
					for (int i = 0; i < values2.Length; i++)
					{
						double[] array3 = values2;
						int num = i;
						double[] array4 = array3;
						array4[num] *= this.gainFactor;
					}
				}
			}
			return data;
		}
		public virtual double getGainFactor()
		{
			return this.gainFactor;
		}

		public virtual void setGainFactor(double gainFactor)
		{
			this.gainFactor = gainFactor;
		}

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			1.0
		})]
		public const string GAIN_FACTOR = "gainFactor";

		private double gainFactor;
	}
}
