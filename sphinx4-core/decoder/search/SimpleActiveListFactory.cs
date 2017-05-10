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
	public class SimpleActiveListFactory : ActiveListFactory
	{
		
		
		public override ActiveList newInstance()
		{
			return new SimpleActiveListFactory.SimpleActiveList(this, this.absoluteBeamWidth, this.logRelativeBeamWidth);
		}

		[LineNumberTable(new byte[]
		{
			159,
			176,
			105
		})]
		
		public SimpleActiveListFactory(int absoluteBeamWidth, double relativeBeamWidth) : base(absoluteBeamWidth, relativeBeamWidth)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			179,
			134
		})]
		
		public SimpleActiveListFactory()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			190,
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
		[SourceFile("SimpleActiveListFactory.java")]
		
		internal sealed class SimpleActiveList : java.lang.Object, ActiveList, Iterable, IEnumerable
		{
			[LineNumberTable(new byte[]
			{
				48,
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
				73,
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
				36,
				239,
				51,
				171,
				235,
				75,
				103,
				104
			})]
			
			public SimpleActiveList(SimpleActiveListFactory simpleActiveListFactory, int num, float num2)
			{
				this.absoluteBeamWidth = 2000;
				this.tokenList = new LinkedList();
				this.absoluteBeamWidth = num;
				this.logRelativeBeamWidth = num2;
			}

			[LineNumberTable(new byte[]
			{
				62,
				103,
				99,
				238,
				74
			})]
			
			public void replace(Token token, Token token2)
			{
				this.add(token2);
				if (token == null || !this.tokenList.remove(token))
				{
				}
			}

			[LineNumberTable(new byte[]
			{
				83,
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

			private int absoluteBeamWidth;

			
			private float logRelativeBeamWidth;

			private Token bestToken;

			
			private List tokenList;

			
			internal SimpleActiveListFactory this_0 = simpleActiveListFactory;
		}
	}
}
