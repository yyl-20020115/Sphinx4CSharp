using java.lang;

namespace edu.cmu.sphinx.frontend.endpoint
{
	public class SpeechClassifiedData : Object, Data
	{
		public SpeechClassifiedData(DoubleData doubleData, bool isSpeech)
		{
			this.data = doubleData;
			this._isSpeech = isSpeech;
		}

		public virtual void setSpeech(bool isSpeech)
		{
			this._isSpeech = isSpeech;
		}

		public virtual bool isSpeech()
		{
			return this._isSpeech;
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
			return new StringBuilder().append("SpeechClassifiedData containing ").append(this.data.toString()).append(" classified as ").append((!this._isSpeech) ? "non-speech" : "speech").toString();
		}

		private bool _isSpeech;

		private DoubleData data;
	}
}
