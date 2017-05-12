using java.util;
using java.lang;

namespace edu.cmu.sphinx.linguist
{
	internal sealed class WordSequence_1 : Object, Comparator
	{
		public int compare(WordSequence wordSequence, WordSequence wordSequence2)
		{
			return wordSequence.getOldest().compareTo(wordSequence2.getOldest());
		}

		internal WordSequence_1()
		{
		}
		
		public int compare(object obj, object obj2)
		{
			return this.compare((WordSequence)obj, (WordSequence)obj2);
		}
		
		bool Comparator.equals(object obj)
		{
			return Object.instancehelper_equals(this, obj);
		}
	}
}
