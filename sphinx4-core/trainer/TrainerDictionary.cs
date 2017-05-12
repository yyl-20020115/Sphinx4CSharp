using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.trainer
{
	public class TrainerDictionary : TextDictionary
	{
		public virtual Graph getWordGraph(string word, bool hasDummy)
		{
			Graph graph = new Graph();
			Node node = null;
			string text = java.lang.String.instancehelper_replaceFirst(word, "\\(.*\\)", "");
			int num;
			if (!java.lang.String.instancehelper_equals(word, text))
			{
				string text2 = java.lang.String.instancehelper_replaceFirst(java.lang.String.instancehelper_replaceFirst(word, ".*\\(", ""), "\\)", "");
				try
				{
					num = Integer.parseInt(text2);
				}
				catch (NumberFormatException ex)
				{
					throw new Error("Word with invalid pronunciation ID", ex);
				}
				goto IL_7F;

			}
			num = 0;
			IL_7F:
			Pronunciation[] pronunciations = this.getWord(text).getPronunciations();
			if (pronunciations == null)
			{
				java.lang.System.@out.println(new StringBuilder().append("Pronunciation not found for word ").append(text).toString());
				return null;
			}
			if (num >= pronunciations.Length)
			{
				java.lang.System.@out.println(new StringBuilder().append("Dictionary has only ").append(pronunciations.Length).append(" for word ").append(word).toString());
				return null;
			}
			Unit[] units = pronunciations[num].getUnits();
			if (!TrainerDictionary.assertionsDisabled && units == null)
			{
				object obj = "units is empty: problem with dictionary?";
				
				throw new AssertionError(obj);
			}
			Node node3;
			if (hasDummy)
			{
				Node node2 = new Node(NodeType.__DUMMY);
				graph.addNode(node2);
				graph.setInitialNode(node2);
				node3 = node2;
			}
			else
			{
				node3 = null;
			}
			Unit[] array = units;
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				Unit unit = array[i];
				node = new Node(NodeType.__PHONE, unit.getName());
				if (node3 == null)
				{
					graph.addNode(node);
					graph.setInitialNode(node);
				}
				else
				{
					graph.linkNodes(node3, node);
				}
				node3 = node;
			}
			if (hasDummy)
			{
				node = new Node(NodeType.__DUMMY);
				graph.linkNodes(node3, node);
			}
			if (!TrainerDictionary.assertionsDisabled && node == null)
			{
				
				throw new AssertionError();
			}
			graph.setFinalNode(node);
			return graph;
		}

		public TrainerDictionary()
		{
		}

		public override string toString()
		{
			return "DEFAULT";
		}
		
		internal const string UTTERANCE_BEGIN_SYMBOL = "<s>";

		internal const string UTTERANCE_END_SYMBOL = "</s>";

		internal const string SILENCE_SYMBOL = "SIL";

		internal static bool assertionsDisabled = !ClassLiteral<TrainerDictionary>.Value.desiredAssertionStatus();
	}
}
