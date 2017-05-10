using System;

using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.lextree
{
	[SourceFile("HMMTree.java")]
	internal class WordNode : Node
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			160,
			177,
			106,
			103,
			113
		})]
		
		internal WordNode(Pronunciation pronunciation, float num) : base(num)
		{
			this.pronunciation = pronunciation;
			this.isFinal = pronunciation.getWord().isSentenceEndWord();
		}

		
		
		internal virtual Word getWord()
		{
			return this.pronunciation.getWord();
		}

		internal virtual Pronunciation getPronunciation()
		{
			return this.pronunciation;
		}

		[LineNumberTable(new byte[]
		{
			160,
			209,
			108
		})]
		
		internal virtual Unit getLastUnit()
		{
			Unit[] units = this.pronunciation.getUnits();
			return units[units.Length - 1];
		}

		
		
		internal override Node[] getSuccessors()
		{
			string text = "Not supported";
			
			throw new Error(text);
		}

		[LineNumberTable(new byte[]
		{
			160,
			232,
			127,
			6,
			47
		})]
		
		public override string toString()
		{
			return new StringBuilder().append("WordNode ").append(this.pronunciation).append(" p ").append(this.getUnigramProbability()).toString();
		}

		public virtual bool isFinal()
		{
			return this.isFinal;
		}

		
		static WordNode()
		{
			Node.__<clinit>();
		}

		
		private Pronunciation pronunciation;

		
		private bool isFinal;
	}
}
