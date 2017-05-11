using edu.cmu.sphinx.linguist.acoustic;
using edu.cmu.sphinx.linguist.dictionary;
using java.lang;

namespace edu.cmu.sphinx.linguist.lextree
{
	internal class WordNode : Node
	{
		internal WordNode(Pronunciation pronunciation, float num) : base(num)
		{
			this.pronunciation = pronunciation;
			this._isFinal = pronunciation.getWord().isSentenceEndWord();
		}
		
		internal virtual Word getWord()
		{
			return this.pronunciation.getWord();
		}

		internal virtual Pronunciation getPronunciation()
		{
			return this.pronunciation;
		}
		
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
		
		public override string toString()
		{
			return new StringBuilder().append("WordNode ").append(this.pronunciation).append(" p ").append(this.getUnigramProbability()).toString();
		}

		public virtual bool isFinal()
		{
			return this._isFinal;
		}
		
		private Pronunciation pronunciation;
		
		private bool _isFinal;
	}
}
