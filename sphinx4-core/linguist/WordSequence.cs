using edu.cmu.sphinx.linguist.dictionary;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist
{
	public sealed class WordSequence : Object, Comparable
	{		
		public WordSequence(List list)
		{
			this._hashCode = -1;
			this.words = (Word[])list.toArray(new Word[list.size()]);
			this.check();
		}
		
		public WordSequence getOldest()
		{
			WordSequence wordSequence = WordSequence.__EMPTY;
			if (this.size() >= 1)
			{
				wordSequence = new WordSequence(this.words.Length - 1);
				ByteCodeHelper.arraycopy(this.words, 0, wordSequence.words, 0, wordSequence.words.Length);
			}
			return wordSequence;
		}
		
		public int compareTo(WordSequence other)
		{
			int num = Math.min(this.words.Length, other.words.Length);
			for (int i = 0; i < num; i++)
			{
				if (!this.words[i].equals(other.words[i]))
				{
					return this.words[i].compareTo(other.words[i]);
				}
			}
			return this.words.Length - other.words.Length;
		}
		
		public WordSequence(params Word[] words) : this(Arrays.asList(words))
		{
		}
		
		private void check()
		{
			Word[] array = this.words;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				if (array[i] == null)
				{
					string text = "WordSequence should not have null Words.";
					
					throw new Error(text);
				}
			}
		}

		public int size()
		{
			return this.words.Length;
		}

		private WordSequence(int num)
		{
			this._hashCode = -1;
			this.words = new Word[num];
		}
		
		public Word getWord(int n)
		{
			if (!WordSequence.assertionsDisabled && n >= this.words.Length)
			{
				
				throw new AssertionError();
			}
			return this.words[n];
		}
		
		public WordSequence getSubSequence(int startIndex, int stopIndex)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = startIndex; i < stopIndex; i++)
			{
				arrayList.add(this.getWord(i));
			}
			return new WordSequence(arrayList);
		}
		
		public static WordSequence asWordSequence(dictionary.Dictionary dictionary, params string[] words)
		{
			Word[] array = new Word[words.Length];
			for (int i = 0; i < words.Length; i++)
			{
				array[i] = dictionary.getWord(words[i]);
			}
			return new WordSequence(array);
		}
		
		public WordSequence addWord(Word word, int maxSize)
		{
			if (maxSize <= 0)
			{
				return WordSequence.__EMPTY;
			}
			int num = (this.size() + 1 <= maxSize) ? (this.size() + 1) : maxSize;
			WordSequence wordSequence = new WordSequence(num);
			int num2 = num - 1;
			int num3 = this.size() - 1;
			Word[] array = wordSequence.words;
			int num4 = num2;
			num2 --;
			array[num4] = word;
			while (num2 >= 0 && num3 >= 0)
			{
				Word[] array2 = wordSequence.words;
				int num5 = num2;
				num2 --;
				Word[] array3 = this.words;
				int num6 = num3;
				num3 --;
				array2[num5] = array3[num6];
			}
			wordSequence.check();
			return wordSequence;
		}
		
		public WordSequence getNewest()
		{
			WordSequence wordSequence = WordSequence.__EMPTY;
			if (this.size() >= 1)
			{
				wordSequence = new WordSequence(this.words.Length - 1);
				ByteCodeHelper.arraycopy(this.words, 1, wordSequence.words, 0, wordSequence.words.Length);
			}
			return wordSequence;
		}
		
		public WordSequence trim(int maxSize)
		{
			if (maxSize <= 0 || this.size() == 0)
			{
				return WordSequence.__EMPTY;
			}
			if (maxSize == this.size())
			{
				return this;
			}
			if (maxSize > this.size())
			{
				maxSize = this.size();
			}
			WordSequence wordSequence = new WordSequence(maxSize);
			int num = this.words.Length - 1;
			int num2 = wordSequence.words.Length - 1;
			for (int i = 0; i < maxSize; i++)
			{
				Word[] array = wordSequence.words;
				int num3 = num2;
				num2 --;
				Word[] array2 = this.words;
				int num4 = num;
				num --;
				array[num3] = array2[num4];
			}
			return wordSequence;
		}
		
		public override string toString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Word[] array = this.words;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Word word = array[i];
				stringBuilder.append('[').append(word).append(']');
			}
			return stringBuilder.toString();
		}
		
		public override int hashCode()
		{
			if (this._hashCode == -1)
			{
				int num = 123;
				for (int i = 0; i < this.words.Length; i++)
				{
					num += this.words[i].hashCode() * (2 * i + 1);
				}
				this._hashCode = num;
			}
			return this._hashCode;
		}
		
		public override bool equals(object @object)
		{
			return this == @object || (@object is WordSequence && Arrays.equals(this.words, ((WordSequence)@object).words));
		}
		
		public Word[] getWords()
		{
			return this.getSubSequence(0, this.size()).words;
		}		
		
		public int compareTo(object obj)
		{
			return this.compareTo((WordSequence)obj);
		}

		static WordSequence()
		{
			WordSequence.__OLDEST_COMPARATOR = new WordSequence_1();
			WordSequence.__EMPTY = new WordSequence(0);
		}

		int System.IComparable.CompareTo(object obj)
		{
			return this.compareTo(obj);
		}
		
		public static Comparator OLDEST_COMPARATOR
		{
			
			get
			{
				return WordSequence.__OLDEST_COMPARATOR;
			}
		}
		
		public static WordSequence EMPTY
		{
			
			get
			{
				return WordSequence.__EMPTY;
			}
		}

		internal static Comparator __OLDEST_COMPARATOR;

		internal static WordSequence __EMPTY;
		
		private Word[] words;

		[System.NonSerialized]
		private int _hashCode;
		
		internal static bool assertionsDisabled = !ClassLiteral<WordSequence>.Value.desiredAssertionStatus();
	}
}
