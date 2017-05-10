using System;

using IKVM.Attributes;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.ReferenceSource"
	})]
	public class FileReferenceSource : java.lang.Object, ReferenceSource
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			174,
			104,
			107,
			108,
			98,
			109,
			109,
			98,
			99,
			104,
			101,
			105,
			110,
			99,
			162,
			236,
			56,
			232,
			74,
			100,
			142,
			142,
			133,
			102
		})]
		
		public FileReferenceSource(string file)
		{
			this.references = new LinkedList();
			BufferedReader bufferedReader = new BufferedReader(new FileReader(file));
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				if (!java.lang.String.instancehelper_startsWith(text, ";;"))
				{
					int num = 0;
					int num2 = 0;
					for (int i = 0; i < 6; i++)
					{
						if (i == 2)
						{
							string text2 = java.lang.String.instancehelper_substring(text, num);
							if (java.lang.String.instancehelper_startsWith(text2, "inter_segment_gap"))
							{
								num2 = 1;
								break;
							}
						}
						num = java.lang.String.instancehelper_indexOf(text, 32, num) + 1;
					}
					if (num2 == 0)
					{
						string text3 = java.lang.String.instancehelper_trim(java.lang.String.instancehelper_substring(text, num));
						this.references.add(text3);
					}
				}
			}
			bufferedReader.close();
		}

		
		public virtual List getReferences()
		{
			return this.references;
		}

		
		
		private List references;
	}
}
