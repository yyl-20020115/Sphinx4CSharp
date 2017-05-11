using System;

using IKVM.Attributes;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	.
	
	internal sealed class HypothesisFile : java.lang.Object
	{
		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			155,
			104,
			113
		})]
		
		internal HypothesisFile(string text)
		{
			this.reader = new BufferedReader(new FileReader(text));
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			166,
			117,
			103,
			99,
			147,
			167,
			104,
			130
		})]
		
		internal HypothesisWord nextWord()
		{
			if (this.iterator == null || !this.iterator.hasNext())
			{
				HypothesisUtterance hypothesisUtterance = this.nextUtterance();
				if (hypothesisUtterance != null)
				{
					this.iterator = hypothesisUtterance.getWords().iterator();
				}
				else
				{
					this.iterator = null;
				}
			}
			if (this.iterator == null)
			{
				return null;
			}
			return (HypothesisWord)this.iterator.next();
		}

		public int getUtteranceCount()
		{
			return this.utteranceCount;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			188,
			108,
			99,
			110,
			103,
			105,
			135,
			162
		})]
		
		private HypothesisUtterance nextUtterance()
		{
			string text = this.reader.readLine();
			if (text == null)
			{
				return null;
			}
			this.utteranceCount++;
			HypothesisUtterance hypothesisUtterance = new HypothesisUtterance(text);
			if (hypothesisUtterance.getWordCount() <= 0)
			{
				return this.nextUtterance();
			}
			return hypothesisUtterance;
		}

		private BufferedReader reader;

		
		private Iterator iterator;

		private int utteranceCount;
	}
}
