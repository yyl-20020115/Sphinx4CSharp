using java.lang;

namespace edu.cmu.sphinx.jsgf.parser
{
	internal sealed class JSGFEncoding : Object
	{		
		internal JSGFEncoding(string text, string text2, string text3)
		{
			this.version = text;
			this.encoding = text2;
			this.locale = text3;
		}

		public string version;

		public string encoding;

		public string locale;
	}
}
