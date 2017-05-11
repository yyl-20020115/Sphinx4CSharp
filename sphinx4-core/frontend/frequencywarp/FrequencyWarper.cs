namespace edu.cmu.sphinx.frontend.frequencywarp
{
	public class FrequencyWarper : java.lang.Object
	{		
		public FrequencyWarper()
		{
		}

		public virtual double hertzToBark(double hertz)
		{
			double num = hertz / 600.0;
			return 6.0 * java.lang.Math.log(num + java.lang.Math.sqrt(num * num + (double)1f));
		}
		
		public virtual double barkToHertz(double bark)
		{
			double num = bark / 6.0;
			return 300.0 * (java.lang.Math.exp(num) - java.lang.Math.exp(-num));
		}
	}
}
