using java.lang;

namespace edu.cmu.sphinx.linguist.dictionary
{
	public class Word : java.lang.Object, Comparable
	{
		public virtual string getSpelling()
		{
			return this.spelling;
		}

		public virtual bool isFiller()
		{
			return this._isFiller;
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
		
		public Word(string spelling, Pronunciation[] pronunciations, bool isFiller)
		{
			this.spelling = spelling;
			this.pronunciations = pronunciations;
			this._isFiller = isFiller;
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
		int System.IComparable.CompareTo(object obj)
		{
			return this.compareTo((Word)obj);
		}

		static Word()
		{
			Pronunciation[] array = new Pronunciation[]
			{
				Pronunciation.__UNKNOWN
			};
			Word.__UNKNOWN = new Word("<unk>", array, false);
			Pronunciation.__UNKNOWN.setWord(Word.__UNKNOWN);
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
		
		private bool _isFiller;
	}
}
