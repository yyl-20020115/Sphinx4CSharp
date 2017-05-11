using System;
using System.Collections;
using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.util.props;
using ikvm.lang;
using java.lang;
using java.util;
using java.util.function;

namespace edu.cmu.sphinx.decoder.search
{
	public class SortingActiveListFactory : ActiveListFactory
	{
		public override ActiveList newInstance()
		{
			return new SortingActiveListFactory.SortingActiveList(this, this.absoluteBeamWidth, this.logRelativeBeamWidth);
		}
		
		public SortingActiveListFactory(int absoluteBeamWidth, double relativeBeamWidth) : base(absoluteBeamWidth, relativeBeamWidth)
		{
		}		
		public SortingActiveListFactory()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
		}
		
		internal sealed class SortingActiveList : java.lang.Object, ActiveList, Iterable, IEnumerable
		{			
			public float getBestScore()
			{
				float result = float.MinValue;
				if (this.bestToken != null)
				{
					result = this.bestToken.getScore();
				}
				return result;
			}
			
			public SortingActiveList(SortingActiveListFactory sortingActiveListFactory, int num, float num2)
			{
				this.absoluteBeamWidth = num;
				this.logRelativeBeamWidth = num2;
				int num3 = (num <= 0) ? 1000 : num;
				this.tokenList = new java.util.ArrayList(num3);
			}
			
			public void add(Token token)
			{
				this.tokenList.add(token);
				if (this.bestToken == null || token.getScore() > this.bestToken.getScore())
				{
					this.bestToken = token;
				}
			}
			
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
				return this.this_0.newInstance();
			}
			
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new IterableEnumerator(this);
			}

			public void forEach(Consumer action)
			{
				tokenList.forEach(action);
			}

			public void forEach(Iterable value1, Consumer value2)
			{
				throw new NotImplementedException();
			}

			public Spliterator spliterator()
			{
				return ((Iterable)tokenList).spliterator();
			}

			public Spliterator spliterator(Iterable value)
			{
				throw new NotImplementedException();
			}

			private const int DEFAULT_SIZE = 1000;

			private int absoluteBeamWidth;
			
			private float logRelativeBeamWidth;

			private Token bestToken;
			
			private List tokenList;
			
			internal SortingActiveListFactory this_0 = sortingActiveListFactory;
		}
	}
}
