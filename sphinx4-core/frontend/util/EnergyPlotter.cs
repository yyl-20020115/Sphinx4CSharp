using edu.cmu.sphinx.util.props;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.util
{
	public class EnergyPlotter : Object, Configurable
	{		
		private void buildPlots(int num)
		{
			this.plots = new string[num + 1];
			for (int i = 0; i < num + 1; i++)
			{
				this.plots[i] = this.getPlotString(i);
			}
		}
		
		private string getPlotString(int num)
		{
			char[] array = new char[num];
			Arrays.fill(array, '.');
			if (num > 0)
			{
				if (num < 10)
				{
					array[array.Length - 1] = (char)(48 + num);
				}
				else
				{
					array[array.Length - 2] = '1';
					array[array.Length - 1] = (char)(48 + (num - 10));
				}
			}
			return new StringBuilder().append('+').append(String.newhelper(array)).toString();
		}
		
		private string getPlot(int num)
		{
			if (num < 0)
			{
				return "-";
			}
			if (num <= this.maxEnergy)
			{
				return this.plots[num];
			}
			return this.getPlotString(num);
		}
		
		public EnergyPlotter(int maxEnergy)
		{
			this.maxEnergy = maxEnergy;
			this.buildPlots(maxEnergy);
		}
		
		public EnergyPlotter()
		{
		}

		public virtual void newProperties(PropertySheet ps)
		{
			this.maxEnergy = ps.getInt("maxEnergy");
			this.buildPlots(this.maxEnergy);
		}
		
		public virtual void plot(Data cepstrum)
		{
			if (cepstrum != null)
			{
				if (cepstrum is DoubleData)
				{
					int num = ByteCodeHelper.d2i(((DoubleData)cepstrum).getValues()[0]);
					java.lang.System.@out.println(this.getPlot(num));
				}
				else
				{
					java.lang.System.@out.println(cepstrum);
				}
			}
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			20
		})]
		public const string PROP_MAX_ENERGY = "maxEnergy";

		private int maxEnergy;

		private string[] plots;
	}
}
