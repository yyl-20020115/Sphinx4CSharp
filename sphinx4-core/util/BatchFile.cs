using java.io;
using java.util;

namespace edu.cmu.sphinx.util
{
	public class BatchFile : java.lang.Object
	{		
		public static string getReference(string batchFileLine)
		{
			int num = java.lang.String.instancehelper_indexOf(batchFileLine, 32);
			return java.lang.String.instancehelper_trim(java.lang.String.instancehelper_substring(batchFileLine, num + 1));
		}
		
		public static string getFilename(string batchFileLine)
		{
			int num = java.lang.String.instancehelper_indexOf(batchFileLine, 32);
			return java.lang.String.instancehelper_trim(java.lang.String.instancehelper_substring(batchFileLine, 0, num));
		}
		
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
				
		public static List getLines(string batchFile)
		{
			return BatchFile.getLines(batchFile, 0);
		}
	}
}
