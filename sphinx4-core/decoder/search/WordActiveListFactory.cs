using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.linguist;
using edu.cmu.sphinx.linguist.dictionary;
using edu.cmu.sphinx.util.props;
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
				
		internal static int access_100(WordActiveListFactory wordActiveListFactory)
		{
			return wordActiveListFactory.maxPathsPerWord;
		}

		public override ActiveList newInstance()
		{
			return new WordActiveListFactory.WordActiveList(this);
		}
		
		public WordActiveListFactory(int absoluteBeamWidth, double relativeBeamWidth, int maxPathsPerWord, int maxFiller) : base(absoluteBeamWidth, relativeBeamWidth)
		{
			this.maxPathsPerWord = maxPathsPerWord;
			this.maxFiller = maxFiller;
		}
		
		public WordActiveListFactory()
		{
		}
		
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

		internal sealed class WordActiveList : ActiveListBase, System.Collections.IEnumerable
		{
			public override void add(Token token)
			{
				this.tokenList.add(token);
				if (this.bestToken == null || token.getScore() > this.bestToken.getScore())
				{
					this.bestToken = token;
				}
			}

			public override float getBestScore()
			{
				float result = float.MinValue;
				if (this.bestToken != null)
				{
					result = this.bestToken.getScore();
				}
				return result;
			}
			
			internal WordActiveList(WordActiveListFactory wordActiveListFactory)
			{
				this_0 = wordActiveListFactory;
				this.tokenList = new LinkedList();
			}
			
			public void replace(Token token, Token token2)
			{
				this.add(token2);
				if (token != null)
				{
					this.tokenList.remove(token);
				}
			}

			public override ActiveList purge()
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
					if (WordActiveListFactory.access_000(this.this_0) > 0 && word.isFiller())
					{
						if (num >= WordActiveListFactory.access_000(this.this_0))
						{
							iterator.remove();
							continue;
						}
						num++;
					}
					if (WordActiveListFactory.access_100(this.this_0) > 0)
					{
						Integer integer = (Integer)hashMap.get(word);
						int num2 = (integer != null) ? integer.intValue() : 0;
						if (num2 < WordActiveListFactory.access_100(this.this_0) - 1)
						{
							hashMap.put(word, Integer.valueOf(num2 + 1));
						}
						else
						{
							iterator.remove();
						}
					}
				}
				if (this.tokenList.size() > this.this_0.absoluteBeamWidth)
				{
					this.tokenList = this.tokenList.subList(0, this.this_0.absoluteBeamWidth);
				}
				return this;
			}

			public override Iterator iterator()
			{
				return this.tokenList.iterator();
			}

			public override List getTokens()
			{
				return this.tokenList;
			}

			public override int size()
			{
				return this.tokenList.size();
			}

			public override float getBeamThreshold()
			{
				return this.getBestScore() + this.this_0.logRelativeBeamWidth;
			}

			public override void setBestToken(Token token)
			{
				this.bestToken = token;
			}

			public override Token getBestToken()
			{
				return this.bestToken;
			}

			public override ActiveList newInstance()
			{
				return this.this_0.newInstance();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return new IterableEnumerator(this);
			}

			private Token bestToken;
			
			private List tokenList;
			
			internal WordActiveListFactory this_0;
		}
	}
}
