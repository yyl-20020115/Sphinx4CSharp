using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.frontend.transform
{
	public class DiscreteCosineTransform2 : DiscreteCosineTransform
	{		
		public DiscreteCosineTransform2(int numberMelFilters, int cepstrumSize) : base(numberMelFilters, cepstrumSize)
		{
		}
		
		public DiscreteCosineTransform2()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
		}
		
		protected internal override double[] applyMelCosine(double[] melspectrum)
		{
			double[] array = new double[this.cepstrumSize];
			double num = java.lang.Math.sqrt((double)1f / (double)this.numberMelFilters);
			double num2 = java.lang.Math.sqrt(2.0 / (double)this.numberMelFilters);
			array[0] = melspectrum[0];
			int num3;
			double[] array3;
			for (int i = 1; i < this.numberMelFilters; i++)
			{
				double[] array2 = array;
				num3 = 0;
				array3 = array2;
				array3[num3] += melspectrum[i];
			}
			double[] array4 = array;
			num3 = 0;
			array3 = array4;
			array3[num3] *= num;
			if (this.numberMelFilters <= 0)
			{
				return array;
			}
			for (int i = 1; i < array.Length; i++)
			{
				double[] array5 = this.melcosine[i];
				array[i] = (double)0f;
				for (int j = 0; j < this.numberMelFilters; j++)
				{
					double[] array6 = array;
					num3 = i;
					array3 = array6;
					array3[num3] += melspectrum[j] * array5[j];
				}
				double[] array7 = array;
				num3 = i;
				array3 = array7;
				array3[num3] *= num2;
			}
			return array;
		}
	}
}
