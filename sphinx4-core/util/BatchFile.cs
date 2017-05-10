using System;

using IKVM.Attributes;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	public class BatchFile : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			35,
			105
		})]
		
		public static string getReference(string batchFileLine)
		{
			int num = java.lang.String.instancehelper_indexOf(batchFileLine, 32);
			return java.lang.String.instancehelper_trim(java.lang.String.instancehelper_substring(batchFileLine, num + 1));
		}

		[LineNumberTable(new byte[]
		{
			22,
			105
		})]
		
		public static string getFilename(string batchFileLine)
		{
			int num = java.lang.String.instancehelper_indexOf(batchFileLine, 32);
			return java.lang.String.instancehelper_trim(java.lang.String.instancehelper_substring(batchFileLine, 0, num));
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		[LineNumberTable(new byte[]
		{
			159,
			188,
			98,
			102,
			140,
			130,
			106,
			104,
			104,
			104,
			196,
			102
		})]
		
		public static List getLines(string batchFile, int skip)
		{
			int num = skip;
			ArrayList arrayList = new ArrayList();
			BufferedReader bufferedReader = new BufferedReader(new FileReader(batchFile));
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				if (!java.lang.String.instancehelper_isEmpty(text))
				{
					num++;
					if (num >= skip)
					{
						arrayList.add(text);
						num = 0;
					}
				}
			}
			bufferedReader.close();
			return arrayList;
		}

		
		
		public BatchFile()
		{
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		
		
		
		public static List getLines(string batchFile)
		{
			return BatchFile.getLines(batchFile, 0);
		}
	}
}
