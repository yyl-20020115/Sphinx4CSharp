using System;

using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.language.ngram
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.language.ngram.LanguageModel"
	})]
	public class DynamicTrigramModel : java.lang.Object, LanguageModel, Configurable
	{
		
		public virtual void setText(List sentences)
		{
			this.sentences = sentences;
		}

		[LineNumberTable(new byte[]
		{
			159,
			172,
			104,
			107,
			107,
			107
		})]
		
		public DynamicTrigramModel()
		{
			this.vocabulary = new HashSet();
			this.logProbs = new HashMap();
			this.logBackoffs = new HashMap();
		}

		
		[LineNumberTable(new byte[]
		{
			113,
			109,
			99,
			151,
			142
		})]
		
		private void addSequence(HashMap hashMap, WordSequence wordSequence)
		{
			Integer integer = (Integer)hashMap.get(wordSequence);
			if (integer != null)
			{
				hashMap.put(wordSequence, Integer.valueOf(integer.intValue() + 1));
			}
			else
			{
				hashMap.put(wordSequence, Integer.valueOf(1));
			}
		}

		[LineNumberTable(new byte[]
		{
			126,
			110,
			121,
			105,
			119,
			99,
			150,
			149,
			98,
			134
		})]
		
		public virtual float getProbability(WordSequence wordSequence)
		{
			float result;
			if (this.logProbs.containsKey(wordSequence))
			{
				result = ((Float)this.logProbs.get(wordSequence)).floatValue();
			}
			else if (wordSequence.size() > 1)
			{
				Float @float = (Float)this.logBackoffs.get(wordSequence.getOldest());
				if (@float == null)
				{
					result = 0f + this.getProbability(wordSequence.getNewest());
				}
				else
				{
					result = @float.floatValue() + this.getProbability(wordSequence.getNewest());
				}
			}
			else
			{
				result = float.MinValue;
			}
			return result;
		}

		[LineNumberTable(new byte[]
		{
			159,
			179,
			104,
			103
		})]
		
		public DynamicTrigramModel(Dictionary dictionary) : this()
		{
			this.dictionary = dictionary;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			184,
			118,
			113,
			113
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.dictionary = (Dictionary)ps.getComponent("dictionary");
			this.maxDepth = ps.getInt("maxDepth");
			this.unigramWeight = ps.getFloat("unigramWeight");
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			190,
			107,
			107,
			107,
			102,
			102,
			102,
			130,
			127,
			8,
			110,
			103,
			115,
			124,
			105,
			130,
			110,
			111,
			100,
			143,
			234,
			55,
			235,
			76,
			147,
			106,
			127,
			8,
			164,
			106,
			100,
			127,
			8,
			191,
			24,
			113,
			100,
			127,
			9,
			127,
			28,
			255,
			47,
			60,
			235,
			70,
			133,
			103,
			107,
			103,
			127,
			5,
			127,
			15,
			130,
			103,
			112,
			119,
			146,
			114,
			119,
			122,
			127,
			4,
			125,
			104,
			113,
			149,
			103,
			103,
			112,
			101,
			130,
			100,
			158,
			122,
			133,
			127,
			9,
			133,
			103,
			127,
			5,
			127,
			4,
			127,
			16,
			133,
			114,
			119,
			122,
			127,
			4,
			159,
			15,
			103,
			103,
			112,
			101,
			130,
			100,
			158,
			122,
			101,
			127,
			9,
			133,
			127,
			8,
			120,
			127,
			9,
			127,
			3,
			101
		})]
		
		public virtual void allocate()
		{
			this.vocabulary.clear();
			this.logProbs.clear();
			this.logBackoffs.clear();
			HashMap hashMap = new HashMap();
			HashMap hashMap2 = new HashMap();
			HashMap hashMap3 = new HashMap();
			int num = 0;
			Iterator iterator = this.sentences.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				string[] array = java.lang.String.instancehelper_split(text, "\\s+");
				ArrayList arrayList = new ArrayList();
				arrayList.add(this.dictionary.getSentenceStartWord());
				string[] array2 = array;
				int num2 = array2.Length;
				for (int i = 0; i < num2; i++)
				{
					string text2 = array2[i];
					if (java.lang.String.instancehelper_length(text2) != 0)
					{
						this.vocabulary.add(text2);
						Word word = this.dictionary.getWord(text2);
						if (word == null)
						{
							arrayList.add(Word.__UNKNOWN);
						}
						else
						{
							arrayList.add(word);
						}
					}
				}
				arrayList.add(this.dictionary.getSentenceEndWord());
				if (arrayList.size() > 0)
				{
					HashMap hashMap4 = hashMap;
					WordSequence.__<clinit>();
					this.addSequence(hashMap4, new WordSequence(new Word[]
					{
						(Word)arrayList.get(0)
					}));
					num++;
				}
				if (arrayList.size() > 1)
				{
					num++;
					HashMap hashMap5 = hashMap;
					WordSequence.__<clinit>();
					this.addSequence(hashMap5, new WordSequence(new Word[]
					{
						(Word)arrayList.get(1)
					}));
					HashMap hashMap6 = hashMap2;
					WordSequence.__<clinit>();
					this.addSequence(hashMap6, new WordSequence(new Word[]
					{
						(Word)arrayList.get(0),
						(Word)arrayList.get(1)
					}));
				}
				for (int j = 2; j < arrayList.size(); j++)
				{
					num++;
					HashMap hashMap7 = hashMap;
					WordSequence.__<clinit>();
					this.addSequence(hashMap7, new WordSequence(new Word[]
					{
						(Word)arrayList.get(j)
					}));
					HashMap hashMap8 = hashMap2;
					WordSequence.__<clinit>();
					this.addSequence(hashMap8, new WordSequence(new Word[]
					{
						(Word)arrayList.get(j - 1),
						(Word)arrayList.get(j)
					}));
					HashMap hashMap9 = hashMap3;
					WordSequence.__<clinit>();
					this.addSequence(hashMap9, new WordSequence(new Word[]
					{
						(Word)arrayList.get(j - 2),
						(Word)arrayList.get(j - 1),
						(Word)arrayList.get(j)
					}));
				}
			}
			float num3 = 0.5f;
			float num4 = 1f - num3;
			HashMap hashMap10 = new HashMap();
			Iterator iterator2 = hashMap.entrySet().iterator();
			while (iterator2.hasNext())
			{
				Map.Entry entry = (Map.Entry)iterator2.next();
				hashMap10.put(entry.getKey(), Float.valueOf((float)((Integer)entry.getValue()).intValue() * num4 / (float)num));
			}
			LogMath logMath = LogMath.getLogMath();
			float num5 = logMath.linearToLog((double)this.unigramWeight);
			float num6 = logMath.linearToLog((double)(1f - this.unigramWeight));
			float num7 = -logMath.linearToLog((double)hashMap10.size());
			TreeSet.__<clinit>();
			TreeSet treeSet = new TreeSet(hashMap.keySet());
			TreeSet.__<clinit>();
			Iterator iterator3 = new TreeSet(hashMap2.keySet()).iterator();
			WordSequence wordSequence = (!iterator3.hasNext()) ? null : ((WordSequence)iterator3.next());
			Iterator iterator4 = treeSet.iterator();
			while (iterator4.hasNext())
			{
				WordSequence wordSequence2 = (WordSequence)iterator4.next();
				float num8 = logMath.linearToLog((double)((Float)hashMap10.get(wordSequence2)).floatValue());
				num8 += num5;
				num8 = logMath.addAsLinear(num8, num7 + num6);
				this.logProbs.put(wordSequence2, Float.valueOf(num8));
				float num9 = 0f;
				while (wordSequence != null)
				{
					int num10 = wordSequence.getOldest().compareTo(wordSequence2);
					if (num10 > 0)
					{
						break;
					}
					if (num10 == 0)
					{
						num9 += ((Float)hashMap10.get(wordSequence.getNewest())).floatValue();
					}
					wordSequence = ((!iterator3.hasNext()) ? null : ((WordSequence)iterator3.next()));
				}
				this.logBackoffs.put(wordSequence2, Float.valueOf(logMath.linearToLog((double)(num3 / (1f - num9)))));
			}
			HashMap hashMap11 = new HashMap();
			Iterator iterator5 = hashMap2.entrySet().iterator();
			while (iterator5.hasNext())
			{
				Map.Entry entry2 = (Map.Entry)iterator5.next();
				int num11 = ((Integer)hashMap.get(((WordSequence)entry2.getKey()).getOldest())).intValue();
				hashMap11.put(entry2.getKey(), Float.valueOf((float)((Integer)entry2.getValue()).intValue() * num4 / (float)num11));
			}
			TreeSet.__<clinit>();
			TreeSet treeSet2 = new TreeSet(hashMap2.keySet());
			TreeSet.__<clinit>();
			iterator3 = new TreeSet(hashMap3.keySet()).iterator();
			wordSequence = ((!iterator3.hasNext()) ? null : ((WordSequence)iterator3.next()));
			Iterator iterator6 = treeSet2.iterator();
			while (iterator6.hasNext())
			{
				WordSequence wordSequence3 = (WordSequence)iterator6.next();
				this.logProbs.put(wordSequence3, Float.valueOf(logMath.linearToLog((double)((Float)hashMap11.get(wordSequence3)).floatValue())));
				float num12 = 0f;
				while (wordSequence != null)
				{
					int num13 = wordSequence.getOldest().compareTo(wordSequence3);
					if (num13 > 0)
					{
						break;
					}
					if (num13 == 0)
					{
						num12 += ((Float)hashMap11.get(wordSequence.getNewest())).floatValue();
					}
					wordSequence = ((!iterator3.hasNext()) ? null : ((WordSequence)iterator3.next()));
				}
				this.logBackoffs.put(wordSequence3, Float.valueOf(logMath.linearToLog((double)(num3 / (1f - num12)))));
			}
			iterator6 = hashMap3.entrySet().iterator();
			while (iterator6.hasNext())
			{
				Map.Entry entry3 = (Map.Entry)iterator6.next();
				float num12 = (float)((Integer)entry3.getValue()).intValue() * num4;
				num12 /= (float)((Integer)hashMap2.get(((WordSequence)entry3.getKey()).getOldest())).intValue();
				this.logProbs.put(entry3.getKey(), Float.valueOf(logMath.linearToLog((double)num12)));
			}
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		public virtual void deallocate()
		{
		}

		public virtual float getSmear(WordSequence wordSequence)
		{
			return 0f;
		}

		
		public virtual Set getVocabulary()
		{
			return this.vocabulary;
		}

		public virtual int getMaxDepth()
		{
			return this.maxDepth;
		}

		public virtual void onUtteranceEnd()
		{
		}

		private Dictionary dictionary;

		
		
		private Set vocabulary;

		private int maxDepth;

		private float unigramWeight;

		
		private List sentences;

		
		private Map logProbs;

		
		private Map logBackoffs;
	}
}
