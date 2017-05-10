using System;
using System.ComponentModel;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.dictionary
{
	[Implements(new string[]
	{
		"java.lang.Comparable"
	})]
	
	public class Word : java.lang.Object, Comparable
	{
		
		public static void __<clinit>()
		{
		}

		public virtual string getSpelling()
		{
			return this.spelling;
		}

		public virtual bool isFiller()
		{
			return this.isFiller;
		}

		
		
		public override int hashCode()
		{
			return java.lang.String.instancehelper_hashCode(this.spelling);
		}

		
		
		public override bool equals(object obj)
		{
			return obj is Word && java.lang.String.instancehelper_equals(this.spelling, ((Word)obj).spelling);
		}

		
		
		public virtual int compareTo(Word other)
		{
			return java.lang.String.instancehelper_compareTo(this.getSpelling(), other.getSpelling());
		}

		public virtual Pronunciation[] getPronunciations()
		{
			return this.pronunciations;
		}

		[LineNumberTable(new byte[]
		{
			159,
			132,
			66,
			104,
			103,
			103,
			103
		})]
		
		public Word(string spelling, Pronunciation[] pronunciations, bool isFiller)
		{
			this.spelling = spelling;
			this.pronunciations = pronunciations;
			this.isFiller = isFiller;
		}

		public override string toString()
		{
			return this.spelling;
		}

		
		
		public virtual bool isSentenceEndWord()
		{
			return java.lang.String.instancehelper_equals("</s>", this.spelling);
		}

		
		
		public virtual bool isSentenceStartWord()
		{
			return java.lang.String.instancehelper_equals("<s>", this.spelling);
		}

		[LineNumberTable(new byte[]
		{
			48,
			102,
			98,
			120,
			106,
			104,
			227,
			61,
			232,
			70
		})]
		
		public virtual Pronunciation getMostLikelyPronunciation()
		{
			float num = float.NegativeInfinity;
			Pronunciation result = null;
			Pronunciation[] array = this.pronunciations;
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				Pronunciation pronunciation = array[i];
				if (pronunciation.getProbability() > num)
				{
					num = pronunciation.getProbability();
					result = pronunciation;
				}
			}
			return result;
		}

		
		[EditorBrowsable(EditorBrowsableState.Never)]
		
		
		public virtual int compareTo(object obj)
		{
			return this.compareTo((Word)obj);
		}

		[LineNumberTable(new byte[]
		{
			159,
			164,
			111,
			113,
			111
		})]
		static Word()
		{
			Pronunciation[] array = new Pronunciation[]
			{
				Pronunciation.__UNKNOWN
			};
			Word.__UNKNOWN = new Word("<unk>", array, false);
			Pronunciation.__UNKNOWN.setWord(Word.__UNKNOWN);
		}

		
		int IComparable.Object;)IcompareTo(object obj)
		{
			return this.compareTo(obj);
		}

		
		public static Word UNKNOWN
		{
			
			get
			{
				return Word.__UNKNOWN;
			}
		}

		internal static Word __UNKNOWN;

		
		private string spelling;

		
		private Pronunciation[] pronunciations;

		
		private bool isFiller;
	}
}
