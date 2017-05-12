using java.io;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.util
{
	public class FileReferenceSource : Object, ReferenceSource
	{	
		public FileReferenceSource(string file)
		{
			this.references = new LinkedList();
			BufferedReader bufferedReader = new BufferedReader(new FileReader(file));
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				if (!String.instancehelper_startsWith(text, ";;"))
				{
					int num = 0;
					int num2 = 0;
					for (int i = 0; i < 6; i++)
					{
						if (i == 2)
						{
							string text2 = String.instancehelper_substring(text, num);
							if (String.instancehelper_startsWith(text2, "inter_segment_gap"))
							{
								num2 = 1;
								break;
							}
						}
						num = String.instancehelper_indexOf(text, 32, num) + 1;
					}
					if (num2 == 0)
					{
						string text3 = String.instancehelper_trim(String.instancehelper_substring(text, num));
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
