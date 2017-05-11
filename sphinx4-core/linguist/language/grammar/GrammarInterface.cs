using java.util;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public interface GrammarInterface
	{
		GrammarNode getInitialNode();

		Set getGrammarNodes();
	}
}
