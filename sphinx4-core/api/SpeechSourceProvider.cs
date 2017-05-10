using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.api
{
	public class SpeechSourceProvider : java.lang.Object
	{
		
		
		public SpeechSourceProvider()
		{
		}

		
		
		internal virtual Microphone getMicrophone()
		{
			return new Microphone(16000f, 16, true, false);
		}
	}
}
