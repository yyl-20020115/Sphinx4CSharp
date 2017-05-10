using System;

using IKVM.Attributes;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.frontend.util
{
	public class AudioContinuityTester : BaseDataProcessor
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			175,
			104,
			103,
			120,
			162,
			149,
			106,
			136
		})]
		
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

		[LineNumberTable(new byte[]
		{
			159,
			160,
			8,
			168,
			102
		})]
		
		public AudioContinuityTester()
		{
			this.lastSampleNum = -1L;
			this.initLogger();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			166,
			140,
			159,
			1
		})]
		
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

		
		static AudioContinuityTester()
		{
		}

		internal long lastSampleNum;

		
		internal static bool assertionsDisabled = !ClassLiteral<AudioContinuityTester>.Value.desiredAssertionStatus();
	}
}
