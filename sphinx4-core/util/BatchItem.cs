using System;

using IKVM.Attributes;
using java.lang;

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

		[LineNumberTable(new byte[]
		{
			159,
			171,
			104,
			103,
			103
		})]
		
		public BatchItem(string filename, string transcript)
		{
			this.filename = filename;
			this.transcript = transcript;
		}

		
		private string filename;

		
		private string transcript;
	}
}
