using System;

using IKVM.Attributes;

namespace edu.cmu.sphinx.frontend.endpoint
{
	public abstract class AbstractVoiceActivityDetector : BaseDataProcessor
	{
		
		
		public AbstractVoiceActivityDetector()
		{
		}

		public abstract bool isSpeech();
	}
}
