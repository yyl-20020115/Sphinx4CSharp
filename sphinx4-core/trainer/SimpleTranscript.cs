using System;

using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.trainer
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.trainer.Transcript"
	})]
	public class SimpleTranscript : java.lang.Object, Transcript
	{
		public virtual void initialize(Dictionary dictionary, bool isExact)
		{
			this.dictionary = dictionary;
			this.isExact = isExact;
			this.wasInitialized = true;
		}

		[LineNumberTable(new byte[]
		{
			159,
			178,
			104,
			104,
			136,
			103
		})]
		
		public SimpleTranscript(string transcript)
		{
			if (!this.wasInitialized)
			{
				this.initialize(null, false);
			}
			this.transcript = transcript;
		}

		[LineNumberTable(new byte[]
		{
			159,
			130,
			130,
			104,
			104
		})]
		
		public SimpleTranscript(Dictionary dictionary, bool isExact)
		{
			this.initialize(dictionary, isExact);
		}

		[LineNumberTable(new byte[]
		{
			159,
			126,
			66,
			104,
			103,
			103,
			103,
			104
		})]
		
		public SimpleTranscript(string transcript, Dictionary dictionary, bool isExact, string wordSeparator)
		{
			this.transcript = transcript;
			this.dictionary = dictionary;
			this.isExact = isExact;
			this.wordSeparator = wordSeparator;
		}

		[LineNumberTable(new byte[]
		{
			159,
			122,
			66,
			104,
			103,
			103,
			103,
			107
		})]
		
		public SimpleTranscript(string transcript, Dictionary dictionary, bool isExact)
		{
			this.transcript = transcript;
			this.dictionary = dictionary;
			this.isExact = isExact;
			this.wordSeparator = " \t\n\r\f";
		}

		public virtual string getTranscriptText()
		{
			return this.transcript;
		}

		public virtual Dictionary getDictionary()
		{
			return this.dictionary;
		}

		public virtual bool isExact()
		{
			return this.isExact;
		}

		
		
		public virtual int numberOfWords()
		{
			return this.words.countTokens();
		}

		[LineNumberTable(new byte[]
		{
			93,
			119
		})]
		
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

		[LineNumberTable(new byte[]
		{
			124,
			134,
			127,
			11,
			104,
			157,
			155,
			124
		})]
		
		public override string toString()
		{
			string text = new StringBuilder().append("Dict: ").append(this.dictionary).append(" : transcript ").toString();
			if (this.isExact)
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

		private Dictionary dictionary;

		internal bool isExact;

		private bool wasInitialized;

		private StringTokenizer words;

		private string wordSeparator;
	}
}
