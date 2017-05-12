using java.io;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.util
{
	public class BatchFile : Object
	{		
		public static string getReference(string batchFileLine)
		{
			int num = String.instancehelper_indexOf(batchFileLine, 32);
			return String.instancehelper_trim(String.instancehelper_substring(batchFileLine, num + 1));
		}
		
		public static string getFilename(string batchFileLine)
		{
			int num = String.instancehelper_indexOf(batchFileLine, 32);
			return String.instancehelper_trim(String.instancehelper_substring(batchFileLine, 0, num));
		}
		
		public static List getLines(string batchFile, int skip)
		{
			int num = skip;
			ArrayList arrayList = new ArrayList();
			BufferedReader bufferedReader = new BufferedReader(new FileReader(batchFile));
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				if (!String.instancehelper_isEmpty(text))
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
