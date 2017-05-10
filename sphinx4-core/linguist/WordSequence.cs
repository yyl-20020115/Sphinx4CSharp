using System;
using System.ComponentModel;

using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist
{
	[Implements(new string[]
	{
		"java.lang.Comparable"
	})]
	
	public sealed class WordSequence : java.lang.Object, Comparable
	{
		
		public static void __<clinit>()
		{
		}

		
		[LineNumberTable(new byte[]
		{
			28,
			232,
			39,
			231,
			90,
			124,
			102
		})]
		
		public WordSequence(List list)
		{
			this.hashCode = -1;
			this.words = (Word[])list.toArray(new Word[list.size()]);
			this.check();
		}

		[LineNumberTable(new byte[]
		{
			71,
			134,
			105,
			111,
			154
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			150,
			116,
			102,
			119,
			22,
			230,
			70
		})]
		
		public int compareTo(WordSequence other)
		{
			int num = java.lang.Math.min(this.words.Length, other.words.Length);
			for (int i = 0; i < num; i++)
			{
				if (!this.words[i].equals(other.words[i]))
				{
					return this.words[i].compareTo(other.words[i]);
				}
			}
			return this.words.Length - other.words.Length;
		}

		[LineNumberTable(new byte[]
		{
			20,
			108
		})]
		
		public WordSequence(params Word[] words) : this(Arrays.asList(words))
		{
		}

		[LineNumberTable(new byte[]
		{
			34,
			116,
			99,
			16,
			166
		})]
		
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

		[LineNumberTable(new byte[]
		{
			10,
			232,
			57,
			231,
			72,
			108
		})]
		
		private WordSequence(int num)
		{
			this.hashCode = -1;
			this.words = new Word[num];
		}

		[LineNumberTable(new byte[]
		{
			160,
			65,
			124
		})]
		
		public Word getWord(int n)
		{
			if (!WordSequence.assertionsDisabled && n >= this.words.Length)
			{
				
				throw new AssertionError();
			}
			return this.words[n];
		}

		[LineNumberTable(new byte[]
		{
			160,
			132,
			134,
			102,
			46,
			198
		})]
		
		public WordSequence getSubSequence(int startIndex, int stopIndex)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = startIndex; i < stopIndex; i++)
			{
				arrayList.add(this.getWord(i));
			}
			return new WordSequence(arrayList);
		}

		[LineNumberTable(new byte[]
		{
			159,
			187,
			104,
			103,
			44,
			166
		})]
		
		public static WordSequence asWordSequence(Dictionary dictionary, params string[] words)
		{
			Word[] array = new Word[words.Length];
			for (int i = 0; i < words.Length; i++)
			{
				array[i] = dictionary.getWord(words[i]);
			}
			return new WordSequence(array);
		}

		[LineNumberTable(new byte[]
		{
			48,
			100,
			134,
			119,
			103,
			100,
			105,
			141,
			104,
			154,
			134
		})]
		
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
			num2 += -1;
			array[num4] = word;
			while (num2 >= 0 && num3 >= 0)
			{
				Word[] array2 = wordSequence.words;
				int num5 = num2;
				num2 += -1;
				Word[] array3 = this.words;
				int num6 = num3;
				num3 += -1;
				array2[num5] = array3[num6];
			}
			wordSequence.check();
			return wordSequence;
		}

		[LineNumberTable(new byte[]
		{
			86,
			134,
			105,
			111,
			154
		})]
		
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

		[LineNumberTable(new byte[]
		{
			103,
			108,
			102,
			105,
			130,
			105,
			136,
			103,
			106,
			138,
			102,
			56,
			166
		})]
		
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
				num2 += -1;
				Word[] array2 = this.words;
				int num4 = num;
				num += -1;
				array[num3] = array2[num4];
			}
			return wordSequence;
		}

		[LineNumberTable(new byte[]
		{
			160,
			86,
			102,
			117,
			55,
			134
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			99,
			105,
			99,
			108,
			54,
			166,
			135
		})]
		
		public override int hashCode()
		{
			if (this.hashCode == -1)
			{
				int num = 123;
				for (int i = 0; i < this.words.Length; i++)
				{
					num += this.words[i].hashCode() * (2 * i + 1);
				}
				this.hashCode = num;
			}
			return this.hashCode;
		}

		[LineNumberTable(new byte[]
		{
			160,
			117,
			100,
			98,
			104,
			130
		})]
		
		public override bool equals(object @object)
		{
			return this == @object || (@object is WordSequence && Arrays.equals(this.words, ((WordSequence)@object).words));
		}

		
		
		public Word[] getWords()
		{
			return this.getSubSequence(0, this.size()).words;
		}

		
		[EditorBrowsable(EditorBrowsableState.Never)]
		
		
		public int compareTo(object obj)
		{
			return this.compareTo((WordSequence)obj);
		}

		[LineNumberTable(new byte[]
		{
			159,
			170,
			245,
			69,
			234,
			72
		})]
		static WordSequence()
		{
			WordSequence.__OLDEST_COMPARATOR = new WordSequence$1();
			WordSequence.__EMPTY = new WordSequence(0);
		}

		
		int IComparable.Object;)IcompareTo(object obj)
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

		[NonSerialized]
		private int hashCode;

		
		internal static bool assertionsDisabled = !ClassLiteral<WordSequence>.Value.desiredAssertionStatus();
	}
}
