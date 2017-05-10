using System;
using System.Collections;

using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.lang;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search
{
	public class SortingActiveListFactory : ActiveListFactory
	{
		
		
		public override ActiveList newInstance()
		{
			return new SortingActiveListFactory.SortingActiveList(this, this.absoluteBeamWidth, this.logRelativeBeamWidth);
		}

		[LineNumberTable(new byte[]
		{
			159,
			175,
			105
		})]
		
		public SortingActiveListFactory(int absoluteBeamWidth, double relativeBeamWidth) : base(absoluteBeamWidth, relativeBeamWidth)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			178,
			134
		})]
		
		public SortingActiveListFactory()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			189,
			103
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
		}

		
		[Implements(new string[]
		{
			"edu.cmu.sphinx.decoder.search.ActiveList"
		})]
		[SourceFile("SortingActiveListFactory.java")]
		
		internal sealed class SortingActiveList : java.lang.Object, ActiveList, Iterable, IEnumerable
		{
			[LineNumberTable(new byte[]
			{
				93,
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
				38,
				111,
				103,
				136,
				109,
				108
			})]
			
			public SortingActiveList(SortingActiveListFactory sortingActiveListFactory, int num, float num2)
			{
				this.absoluteBeamWidth = num;
				this.logRelativeBeamWidth = num2;
				int num3 = (num <= 0) ? 1000 : num;
				this.tokenList = new ArrayList(num3);
			}

			[LineNumberTable(new byte[]
			{
				53,
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
				69,
				124,
				112,
				152
			})]
			
			public ActiveList purge()
			{
				if (this.absoluteBeamWidth > 0 && this.tokenList.size() > this.absoluteBeamWidth)
				{
					Collections.sort(this.tokenList, Scoreable.COMPARATOR);
					this.tokenList = this.tokenList.subList(0, this.absoluteBeamWidth);
				}
				return this;
			}

			
			
			public float getBeamThreshold()
			{
				return this.getBestScore() + this.logRelativeBeamWidth;
			}

			public void setBestToken(Token token)
			{
				this.bestToken = token;
			}

			public Token getBestToken()
			{
				return this.bestToken;
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

			
			
			public ActiveList newInstance()
			{
				return this.this$0.newInstance();
			}

			
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new IterableEnumerator(this);
			}

			private const int DEFAULT_SIZE = 1000;

			
			private int absoluteBeamWidth;

			
			private float logRelativeBeamWidth;

			private Token bestToken;

			
			private List tokenList;

			
			internal SortingActiveListFactory this$0 = sortingActiveListFactory;
		}
	}
}
