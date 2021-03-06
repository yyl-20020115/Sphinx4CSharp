﻿using java.io;
using java.lang;

namespace edu.cmu.sphinx.util
{
	internal sealed class ReferenceFile : Object
	{		
		internal ReferenceFile(string text)
		{
			this.reader = new BufferedReader(new FileReader(text));
		}
		
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
