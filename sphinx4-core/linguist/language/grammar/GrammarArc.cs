using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class GrammarArc : Object
	{
		public virtual float getProbability()
		{
			return this.logProbability;
		}

		public virtual GrammarNode getGrammarNode()
		{
			return this.grammarNode;
		}
	
		public GrammarArc(GrammarNode grammarNode, float logProbability)
		{
			if (!GrammarArc.assertionsDisabled && grammarNode == null)
			{
				
				throw new AssertionError();
			}
			this.grammarNode = grammarNode;
			this.logProbability = logProbability;
		}

		private GrammarNode grammarNode;

		private float logProbability;

		internal static bool assertionsDisabled = !ClassLiteral<GrammarArc>.Value.desiredAssertionStatus();
	}
}
