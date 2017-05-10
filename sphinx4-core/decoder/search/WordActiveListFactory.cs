using System;
using System.Collections;

using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.lang;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search
{
	public class WordActiveListFactory : ActiveListFactory
	{
		
		
		internal static int access_000(WordActiveListFactory wordActiveListFactory)
		{
			return wordActiveListFactory.maxFiller;
		}

		
		
		internal static int access$100(WordActiveListFactory wordActiveListFactory)
		{
			return wordActiveListFactory.maxPathsPerWord;
		}

		
		
		public override ActiveList newInstance()
		{
			return new WordActiveListFactory.WordActiveList(this);
		}

		[LineNumberTable(new byte[]
		{
			3,
			107,
			103,
			104
		})]
		
		public WordActiveListFactory(int absoluteBeamWidth, double relativeBeamWidth, int maxPathsPerWord, int maxFiller) : base(absoluteBeamWidth, relativeBeamWidth)
		{
			this.maxPathsPerWord = maxPathsPerWord;
			this.maxFiller = maxFiller;
		}

		[LineNumberTable(new byte[]
		{
			8,
			102
		})]
		
		public WordActiveListFactory()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			19,
			135,
			113,
			113
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.maxPathsPerWord = ps.getInt("maxPathsPerWord");
			this.maxFiller = ps.getInt("maxFillerWords");
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			0
		})]
		public const string PROP_MAX_PATHS_PER_WORD = "maxPathsPerWord";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			1
		})]
		public const string PROP_MAX_FILLER_WORDS = "maxFillerWords";

		private int maxPathsPerWord;

		private int maxFiller;

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.decoder.search.ActiveList"
		})]
		[SourceFile("WordActiveListFactory.java")]
		
		internal sealed class WordActiveList : java.lang.Object, ActiveList, Iterable, IEnumerable
		{
			[LineNumberTable(new byte[]
			{
				55,
				109,
				123,
				135
			})]
			
			public void add(Token token)
			{
				this.tokenList.add(token);
				if (this.bestToken == null || token.getScore() > this.bestToken.getScore())
				{
					this.bestToken = token;
				}
			}

			[LineNumberTable(new byte[]
			{
				160,
				110,
				102,
				104,
				140
			})]
			
			public float getBestScore()
			{
				float result = float.MinValue;
				if (this.bestToken != null)
				{
					result = this.bestToken.getScore();
				}
				return result;
			}

			[LineNumberTable(new byte[]
			{
				43,
				175
			})]
			
			internal WordActiveList(WordActiveListFactory wordActiveListFactory)
			{
				this.tokenList = new LinkedList();
			}

			[LineNumberTable(new byte[]
			{
				69,
				103,
				99,
				141
			})]
			
			public void replace(Token token, Token token2)
			{
				this.add(token2);
				if (token != null)
				{
					this.tokenList.remove(token);
				}
			}

			[LineNumberTable(new byte[]
			{
				83,
				98,
				102,
				144,
				119,
				108,
				141,
				174,
				110,
				105,
				110,
				134,
				102,
				229,
				69,
				110,
				111,
				240,
				69,
				113,
				148,
				166,
				133,
				120,
				189
			})]
			
			public ActiveList purge()
			{
				int num = 0;
				HashMap hashMap = new HashMap();
				Collections.sort(this.tokenList, Scoreable.COMPARATOR);
				Iterator iterator = this.tokenList.iterator();
				while (iterator.hasNext())
				{
					Token token = (Token)iterator.next();
					WordSearchState wordSearchState = (WordSearchState)token.getSearchState();
					Word word = wordSearchState.getPronunciation().getWord();
					if (WordActiveListFactory.access_000(this.this$0) > 0 && word.isFiller())
					{
						if (num >= WordActiveListFactory.access_000(this.this$0))
						{
							iterator.remove();
							continue;
						}
						num++;
					}
					if (WordActiveListFactory.access$100(this.this$0) > 0)
					{
						Integer integer = (Integer)hashMap.get(word);
						int num2 = (integer != null) ? integer.intValue() : 0;
						if (num2 < WordActiveListFactory.access$100(this.this$0) - 1)
						{
							hashMap.put(word, Integer.valueOf(num2 + 1));
						}
						else
						{
							iterator.remove();
						}
					}
				}
				if (this.tokenList.size() > this.this$0.absoluteBeamWidth)
				{
					this.tokenList = this.tokenList.subList(0, this.this$0.absoluteBeamWidth);
				}
				return this;
			}

			
			
			
			public Iterator iterator()
			{
				return this.tokenList.iterator();
			}

			
			public List getTokens()
			{
				return this.tokenList;
			}

			
			
			public int size()
			{
				return this.tokenList.size();
			}

			
			
			public float getBeamThreshold()
			{
				return this.getBestScore() + this.this$0.logRelativeBeamWidth;
			}

			public void setBestToken(Token token)
			{
				this.bestToken = token;
			}

			public Token getBestToken()
			{
				return this.bestToken;
			}

			
			
			public ActiveList newInstance()
			{
				return this.this$0.newInstance();
			}

			
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new IterableEnumerator(this);
			}

			private Token bestToken;

			
			private List tokenList;

			
			internal WordActiveListFactory this$0 = wordActiveListFactory;
		}
	}
}
