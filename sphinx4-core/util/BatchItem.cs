namespace edu.cmu.sphinx.util
{
	public class BatchItem : java.lang.Object
	{
		public virtual string getFilename()
		{
			return this.filename;
		}

		public virtual string getTranscript()
		{
			return this.transcript;
		}
		
		public BatchItem(string filename, string transcript)
		{
			this.filename = filename;
			this.transcript = transcript;
		}

		private string filename;
		
		private string transcript;
	}
}
