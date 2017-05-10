using System;

using IKVM.Attributes;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.util
{
	[SourceFile("GapInsertionDetector.java")]
	
	internal sealed class ReferenceFile : java.lang.Object
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			107,
			104,
			113
		})]
		
		internal ReferenceFile(string text)
		{
			this.reader = new BufferedReader(new FileReader(text));
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			118,
			108,
			99,
			135
		})]
		
		internal ReferenceUtterance nextUtterance()
		{
			string text = this.reader.readLine();
			if (text != null)
			{
				return new ReferenceUtterance(text);
			}
			return null;
		}

		private BufferedReader reader;
	}
}
