using IKVM.Runtime;

namespace edu.cmu.sphinx.tools.audio
{
	public class Sinusoid : AudioData
	{		
		public Sinusoid(double frequency, double phase, double amplitude, double duration)
		{
			this.shorts = new short[ByteCodeHelper.d2i(8000.0 * duration)];
			double num = frequency * 2.0 * 3.1415926535897931 / 8000.0;
			for (int i = 0; i < this.shorts.Length; i++)
			{
				this.shorts[i] = (short)ByteCodeHelper.d2i(amplitude * java.lang.Math.cos(num * (double)i + phase));
			}
		}
	}
}
