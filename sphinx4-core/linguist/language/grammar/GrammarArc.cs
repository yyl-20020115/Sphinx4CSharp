using System;

using IKVM.Attributes;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.linguist.language.grammar
{
	public class GrammarArc : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		public virtual float getProbability()
		{
			return this.logProbability;
		}

		public virtual GrammarNode getGrammarNode()
		{
			return this.grammarNode;
		}

		[LineNumberTable(new byte[]
		{
			159,
			176,
			104,
			117,
			103,
			104
		})]
		
		public GrammarArc(GrammarNode grammarNode, float logProbability)
		{
			if (!GrammarArc.assertionsDisabled && grammarNode == null)
			{
				
				throw new AssertionError();
			}
			this.grammarNode = grammarNode;
			this.logProbability = logProbability;
		}

		
		static GrammarArc()
		{
		}

		private GrammarNode grammarNode;

		private float logProbability;

		
		internal static bool assertionsDisabled = !ClassLiteral<GrammarArc>.Value.desiredAssertionStatus();
	}
}
