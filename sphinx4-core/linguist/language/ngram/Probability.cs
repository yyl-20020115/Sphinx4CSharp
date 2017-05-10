using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	[SourceFile("SimpleNGramModel.java")]
	
	internal sealed class Probability : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			161,
			87,
			104,
			104,
			104
		})]
		
		internal Probability(float num, float num2)
		{
			this.logProbability = num;
			this.logBackoff = num2;
		}

		
		
		public override string toString()
		{
			return new StringBuilder().append("Prob: ").append(this.logProbability).append(' ').append(this.logBackoff).toString();
		}

		
		internal float logProbability;

		
		internal float logBackoff;
	}
}
