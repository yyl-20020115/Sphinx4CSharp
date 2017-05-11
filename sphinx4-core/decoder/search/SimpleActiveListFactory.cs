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
	public class SimpleActiveListFactory : ActiveListFactory
	{		
		public override ActiveList newInstance()
		{
			return new SimpleActiveListFactory.SimpleActiveList(this, this.absoluteBeamWidth, this.logRelativeBeamWidth);
		}
		
		public SimpleActiveListFactory(int absoluteBeamWidth, double relativeBeamWidth) : base(absoluteBeamWidth, relativeBeamWidth)
		{
		}
		
		public SimpleActiveListFactory()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
		}
		
		internal sealed class SimpleActiveList : java.lang.Object, ActiveList, Iterable, IEnumerable
		{			
			public void add(Token token)
			{
				this.tokenList.add(token);
				if (this.bestToken == null || token.getScore() > this.bestToken.getScore())
				{
					this.bestToken = token;
				}
			}
			
			public float getBestScore()
			{
				float result = float.MinValue;
				if (this.bestToken != null)
				{
					result = this.bestToken.getScore();
				}
				return result;
			}
		
			public SimpleActiveList(SimpleActiveListFactory simpleActiveListFactory, int num, float num2)
			{
				this.absoluteBeamWidth = 2000;
				this.tokenList = new LinkedList();
				this.absoluteBeamWidth = num;
				this.logRelativeBeamWidth = num2;
			}
			
			public void replace(Token token, Token token2)
			{
				this.add(token2);
				if (token == null || !this.tokenList.remove(token))
				{
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
				throw new NotImplementedException();
			}

			public void forEach(Iterable value1, Consumer value2)
			{
				throw new NotImplementedException();
			}

			public Spliterator spliterator()
			{
				throw new NotImplementedException();
			}

			public Spliterator spliterator(Iterable value)
			{
				throw new NotImplementedException();
			}

			private int absoluteBeamWidth;

			private float logRelativeBeamWidth;

			private Token bestToken;
			
			private List tokenList;
			
			internal SimpleActiveListFactory this_0 = simpleActiveListFactory;
		}
	}
}
