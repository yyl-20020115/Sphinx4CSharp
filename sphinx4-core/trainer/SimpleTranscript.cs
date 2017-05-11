using java.lang;
using java.util;

namespace edu.cmu.sphinx.trainer
{
	public class SimpleTranscript : java.lang.Object, Transcript
	{
		public virtual void initialize(linguist.dictionary.Dictionary dictionary, bool isExact)
		{
			this.dictionary = dictionary;
			this._isExact = isExact;
			this.wasInitialized = true;
		}
		
		public SimpleTranscript(string transcript)
		{
			if (!this.wasInitialized)
			{
				this.initialize(null, false);
			}
			this.transcript = transcript;
		}
		
		public SimpleTranscript(linguist.dictionary.Dictionary dictionary, bool isExact)
		{
			this.initialize(dictionary, isExact);
		}
		
		public SimpleTranscript(string transcript, linguist.dictionary.Dictionary dictionary, bool isExact, string wordSeparator)
		{
			this.transcript = transcript;
			this.dictionary = dictionary;
			this._isExact = isExact;
			this.wordSeparator = wordSeparator;
		}
		
		public SimpleTranscript(string transcript, linguist.dictionary.Dictionary dictionary, bool isExact)
		{
			this.transcript = transcript;
			this.dictionary = dictionary;
			this._isExact = isExact;
			this.wordSeparator = " \t\n\r\f";
		}

		public virtual string getTranscriptText()
		{
			return this.transcript;
		}

		public virtual linguist.dictionary.Dictionary getDictionary()
		{
			return this.dictionary;
		}

		public virtual bool isExact()
		{
			return this._isExact;
		}
		
		public virtual int numberOfWords()
		{
			return this.words.countTokens();
		}
		
		public virtual void startWordIterator()
		{
			this.words = new StringTokenizer(this.transcript, this.wordSeparator);
		}
		
		public virtual bool hasMoreWords()
		{
			return this.words.hasMoreTokens();
		}
		
		public virtual string nextWord()
		{
			return this.words.nextToken();
		}
		
		public override string toString()
		{
			string text = new StringBuilder().append("Dict: ").append(this.dictionary).append(" : transcript ").toString();
			if (this._isExact)
			{
				text = new StringBuilder().append(text).append("IS exact: ").toString();
			}
			else
			{
				text = new StringBuilder().append(text).append("is NOT exact: ").toString();
			}
			return new StringBuilder().append(text).append(this.transcript).toString();
		}

		private string transcript;

		private linguist.dictionary.Dictionary dictionary;

		internal bool _isExact;

		private bool wasInitialized;

		private StringTokenizer words;

		private string wordSeparator;
	}
}
