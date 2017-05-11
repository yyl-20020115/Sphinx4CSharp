using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.frontend.filter
{
	public class EnergyFilter : BaseDataProcessor
	{	
		public EnergyFilter(double maxEnergy)
		{
			this.initLogger();
			this.maxEnergy = maxEnergy;
			this.initialize();
		}
	
		public EnergyFilter()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.maxEnergy = ps.getDouble("maxEnergy");
		}		
		public override Data getData()
		{
			Data data;
			for (;;)
			{
				data = this.getPredecessor().getData();
				if (data == null || !(data is DoubleData))
				{
					break;
				}
				float num = 0f;
				double[] values = ((DoubleData)data).getValues();
				int num2 = values.Length;
				for (int i = 0; i < num2; i++)
				{
					double num3 = values[i];
					num = (float)((double)num + num3 * num3);
				}
				if ((double)num >= this.maxEnergy)
				{
					return data;
				}
			}
			return data;
		}

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			2.0
		})]
		public const string PROP_MAX_ENERGY = "maxEnergy";

		private double maxEnergy;
	}
}
