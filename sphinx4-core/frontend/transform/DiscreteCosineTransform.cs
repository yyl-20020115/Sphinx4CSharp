using edu.cmu.sphinx.util.props;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.frontend.transform
{
	public class DiscreteCosineTransform : BaseDataProcessor
	{		
		public DiscreteCosineTransform(int numberMelFilters, int cepstrumSize)
		{
			this.initLogger();
			this.numberMelFilters = numberMelFilters;
			this.cepstrumSize = cepstrumSize;
		}
		
		private DoubleData process(DoubleData doubleData)
		{
			double[] values = doubleData.getValues();
			if (this.melcosine == null)
			{
				this.numberMelFilters = values.Length;
				this.computeMelCosine();
			}
			else if (values.Length != this.numberMelFilters)
			{
				string text = new StringBuilder().append("MelSpectrum size is incorrect: melspectrum.length == ").append(values.Length).append(", numberMelFilters == ").append(this.numberMelFilters).toString();
				
				throw new IllegalArgumentException(text);
			}
			for (int i = 0; i < values.Length; i++)
			{
				values[i] = Math.log(values[i] + 0.0001);
			}
			double[] values2 = this.applyMelCosine(values);
			return new DoubleData(values2, doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
		}
		
		protected internal virtual void computeMelCosine()
		{
			int num = this.cepstrumSize;
			int num2 = this.numberMelFilters;
			int[] array = new int[2];
			int num3 = num2;
			array[1] = num3;
			num3 = num;
			array[0] = num3;
			this.melcosine = (double[][])ByteCodeHelper.multianewarray(typeof(double[][]).TypeHandle, array);
			double num4 = 2.0 * (double)this.numberMelFilters;
			for (int i = 0; i < this.cepstrumSize; i++)
			{
				double num5 = 6.2831853071795862 * (double)i / num4;
				for (int j = 0; j < this.numberMelFilters; j++)
				{
					this.melcosine[i][j] = Math.cos(num5 * ((double)j + 0.5));
				}
			}
		}

		protected internal virtual double[] applyMelCosine(double[] melspectrum)
		{
			double[] array = new double[this.cepstrumSize];
			double num = (double)this.numberMelFilters;
			double num2 = 0.5;
			for (int i = 0; i < array.Length; i++)
			{
				if (this.numberMelFilters > 0)
				{
					double[] array2 = this.melcosine[i];
					int j = 0;
					double[] array3 = array;
					int num3 = i;
					double[] array4 = array3;
					array4[num3] += num2 * melspectrum[j] * array2[j];
					for (j = 1; j < this.numberMelFilters; j++)
					{
						double[] array5 = array;
						num3 = i;
						array4 = array5;
						array4[num3] += melspectrum[j] * array2[j];
					}
					double[] array6 = array;
					num3 = i;
					array4 = array6;
					array4[num3] /= num;
				}
			}
			return array;
		}

		public DiscreteCosineTransform()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.numberMelFilters = ps.getInt("numberFilters");
			this.cepstrumSize = ps.getInt("cepstrumLength");
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
			40
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

		protected internal int cepstrumSize;

		protected internal int numberMelFilters;

		protected internal double[][] melcosine;

		internal const double LOG_FLOOR = 0.0001;
	}
}
