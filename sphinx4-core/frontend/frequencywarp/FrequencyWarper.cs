using java.lang;

namespace edu.cmu.sphinx.frontend.frequencywarp
{
	public class FrequencyWarper : Object
	{		
		public FrequencyWarper()
		{
		}

		public virtual double hertzToBark(double hertz)
		{
			double num = hertz / 600.0;
			return 6.0 * Math.log(num + Math.sqrt(num * num + (double)1f));
		}
		
		public virtual double barkToHertz(double bark)
		{
			double num = bark / 6.0;
			return 300.0 * (Math.exp(num) - Math.exp(-num));
		}
	}
}
