using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.frontend.util
{
	public class AudioContinuityTester : BaseDataProcessor
	{
		private bool isAudioStreamContinuous(Data data)
		{
			if (data is DoubleData)
			{
				DoubleData doubleData = (DoubleData)data;
				if (this.lastSampleNum != -1L && this.lastSampleNum != doubleData.getFirstSampleNumber())
				{
					return false;
				}
				this.lastSampleNum = doubleData.getFirstSampleNumber() + (long)doubleData.getValues().Length;
			}
			else if (data is DataStartSignal)
			{
				this.lastSampleNum = -1L;
			}
			return true;
		}

		public AudioContinuityTester()
		{
			this.lastSampleNum = -1L;
			this.initLogger();
		}

		public override Data getData()
		{
			Data data = this.getPredecessor().getData();
			if (!AudioContinuityTester.assertionsDisabled && !this.isAudioStreamContinuous(data))
			{
				object obj = "audio stream is not continuous";
				
				throw new AssertionError(obj);
			}
			return data;
		}

		internal long lastSampleNum;
		
		internal static bool assertionsDisabled = !ClassLiteral<AudioContinuityTester>.Value.desiredAssertionStatus();
	}
}
