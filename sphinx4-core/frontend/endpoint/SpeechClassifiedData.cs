using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend.endpoint
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.frontend.Data"
	})]
	public class SpeechClassifiedData : java.lang.Object, Data
	{
		[LineNumberTable(new byte[]
		{
			159,
			134,
			98,
			104,
			103,
			103
		})]
		
		public SpeechClassifiedData(DoubleData doubleData, bool isSpeech)
		{
			this.data = doubleData;
			this.isSpeech = isSpeech;
		}

		public virtual void setSpeech(bool isSpeech)
		{
			this.isSpeech = isSpeech;
		}

		public virtual bool isSpeech()
		{
			return this.isSpeech;
		}

		
		
		public virtual double[] getValues()
		{
			return this.data.getValues();
		}

		
		
		public virtual int getSampleRate()
		{
			return this.data.getSampleRate();
		}

		
		
		public virtual long getCollectTime()
		{
			return this.data.getCollectTime();
		}

		
		
		public virtual long getFirstSampleNumber()
		{
			return this.data.getFirstSampleNumber();
		}

		public virtual DoubleData getDoubleData()
		{
			return this.data;
		}

		
		
		public override string toString()
		{
			return new StringBuilder().append("SpeechClassifiedData containing ").append(this.data.toString()).append(" classified as ").append((!this.isSpeech) ? "non-speech" : "speech").toString();
		}

		private bool isSpeech;

		
		private DoubleData data;
	}
}
