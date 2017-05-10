using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.jsgf.parser
{
	[SourceFile("JSGFParser.java")]
	
	internal sealed class JSGFEncoding : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			173,
			104,
			103,
			103,
			103
		})]
		
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
