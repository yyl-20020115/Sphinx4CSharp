using System;
using System.Collections;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.lang;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search
{
	public class PartitionActiveListFactory : ActiveListFactory
	{
		
		
		public override ActiveList newInstance()
		{
			return new PartitionActiveListFactory.PartitionActiveList(this, this.absoluteBeamWidth, this.logRelativeBeamWidth);
		}

		[LineNumberTable(new byte[]
		{
			159,
			174,
			105
		})]
		
		public PartitionActiveListFactory(int absoluteBeamWidth, double relativeBeamWidth) : base(absoluteBeamWidth, relativeBeamWidth)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			177,
			134
		})]
		
		public PartitionActiveListFactory()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			188,
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
		[SourceFile("PartitionActiveListFactory.java")]
		
		internal sealed class PartitionActiveList : java.lang.Object, ActiveList, Iterable, IEnumerable
		{
			[LineNumberTable(new byte[]
			{
				72,
				127,
				0
			})]
			
			private void doubleCapacity()
			{
				this.tokenList = (Token[])Arrays.copyOf(this.tokenList, this.tokenList.Length * 2);
			}

			[LineNumberTable(new byte[]
			{
				56,
				111,
				110,
				176,
				102,
				135,
				123,
				135
			})]
			
			public void add(Token token)
			{
				if (this.size < this.tokenList.Length)
				{
					this.tokenList[this.size] = token;
					this.size++;
				}
				else
				{
					this.doubleCapacity();
					this.add(token);
				}
				if (this.bestToken == null || token.getScore() > this.bestToken.getScore())
				{
					this.bestToken = token;
				}
			}

			[LineNumberTable(new byte[]
			{
				114,
				102,
				104,
				236,
				73
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
				39,
				239,
				56,
				235,
				73,
				103,
				104,
				102,
				100,
				132,
				108
			})]
			
			public PartitionActiveList(PartitionActiveListFactory partitionActiveListFactory, int num, float num2)
			{
				this.partitioner = new Partitioner();
				this.absoluteBeamWidth = num;
				this.logRelativeBeamWidth = num2;
				int num3 = 2000;
				if (num > 0)
				{
					num3 = num / 3;
				}
				this.tokenList = new Token[num3];
			}

			[LineNumberTable(new byte[]
			{
				86,
				169,
				110,
				223,
				6
			})]
			
			public ActiveList purge()
			{
				if (this.absoluteBeamWidth > 0 && this.size > this.absoluteBeamWidth)
				{
					this.size = this.partitioner.partition(this.tokenList, this.size, this.absoluteBeamWidth) + 1;
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
				return new TokenArrayIterator(this.tokenList, this.size);
			}

			
			
			
			public List getTokens()
			{
				return Arrays.asList(this.tokenList).subList(0, this.size);
			}

			public int size()
			{
				return this.size;
			}

			
			
			public ActiveList newInstance()
			{
				return this.this_0.newInstance();
			}

			
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new IterableEnumerator(this);
			}

			private int size;

			
			private int absoluteBeamWidth;

			
			private float logRelativeBeamWidth;

			private Token bestToken;

			private Token[] tokenList;

			
			private Partitioner partitioner;

			
			internal PartitionActiveListFactory this_0 = partitionActiveListFactory;
		}
	}
}
