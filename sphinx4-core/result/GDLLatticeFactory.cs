using edu.cmu.sphinx.linguist.dictionary;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.result
{
	public class GDLLatticeFactory : Object
	{
		private static void createNode(string text, Lattice lattice, Dictionary dictionary)
		{
			string[] array = String.instancehelper_split(text, "\\s");
			string id = String.instancehelper_substring(array[3], 1, String.instancehelper_length(array[3]) - 1);
			string text2 = String.instancehelper_substring(array[5], 1);
			string text3 = String.instancehelper_substring(array[6], 2, String.instancehelper_length(array[6]) - 2);
			string text4 = String.instancehelper_substring(text2, 0, String.instancehelper_indexOf(text2, 91));
			text2 = String.instancehelper_substring(text2, String.instancehelper_indexOf(text2, 91) + 1);
			string text5 = String.instancehelper_substring(text2, 0, String.instancehelper_indexOf(text2, 44));
			string text6 = String.instancehelper_substring(text2, String.instancehelper_indexOf(text2, 44) + 1);
			Node node = lattice.addNode(id, dictionary.getWord(text4), (long)Integer.parseInt(text5), (long)Integer.parseInt(text6));
			node.setPosterior(Double.parseDouble(text3));
			if (String.instancehelper_equals(text4, "<s>"))
			{
				lattice.setInitialNode(node);
			}
			else if (String.instancehelper_equals(text4, "</s>"))
			{
				lattice.setTerminalNode(node);
			}
		}

		private static void createEdge(string text, Lattice lattice)
		{
			string[] array = String.instancehelper_split(text, "\\s");
			string id = String.instancehelper_substring(array[3], 1, String.instancehelper_length(array[3]) - 1);
			string id2 = String.instancehelper_substring(array[5], 1, String.instancehelper_length(array[5]) - 1);
			string text2 = String.instancehelper_substring(array[7], 1, String.instancehelper_length(array[7]) - 1);
			string[] array2 = String.instancehelper_split(text2, ",");
			lattice.addEdge(lattice.getNode(id), lattice.getNode(id2), Double.parseDouble(array2[0]), Double.parseDouble(array2[1]));
		}
	
		private GDLLatticeFactory()
		{
		}

		public static Lattice getLattice(string gdlFile, Dictionary dictionary)
		{
			Lattice lattice = new Lattice();
			BufferedReader bufferedReader = new BufferedReader(new FileReader(gdlFile));
			string text;
			while ((text = bufferedReader.readLine()) != null)
			{
				if (String.instancehelper_startsWith(text, "node"))
				{
					GDLLatticeFactory.createNode(text, lattice, dictionary);
				}
				else if (String.instancehelper_startsWith(text, "edge"))
				{
					GDLLatticeFactory.createEdge(text, lattice);
				}
			}
			bufferedReader.close();
			return lattice;
		}
	}
}
