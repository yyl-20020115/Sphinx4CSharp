using edu.cmu.sphinx.util.props;
using ikvm.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search
{
	public class PartitionActiveListFactory : ActiveListFactory
	{
		public override ActiveList newInstance()
		{
			return new PartitionActiveListFactory.PartitionActiveList(this, this.absoluteBeamWidth, this.logRelativeBeamWidth);
		}
		
		public PartitionActiveListFactory(int absoluteBeamWidth, double relativeBeamWidth) : base(absoluteBeamWidth, relativeBeamWidth)
		{
		}
		
		public PartitionActiveListFactory()
		{
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
		}
		
		internal sealed class PartitionActiveList : ActiveListBase, System.Collections.IEnumerable
		{			
			private void doubleCapacity()
			{
				this.tokenList = (Token[])Arrays.copyOf(this.tokenList, this.tokenList.Length * 2);
			}

			public override void add(Token token)
			{
				if (this._size < this.tokenList.Length)
				{
					this.tokenList[this._size] = token;
					this._size++;
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

			public override float getBestScore()
			{
				float result = float.MinValue;
				if (this.bestToken != null)
				{
					result = this.bestToken.getScore();
				}
				return result;
			}
			
			public PartitionActiveList(PartitionActiveListFactory partitionActiveListFactory, int num, float num2)
			{
				this_0 = partitionActiveListFactory;
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

			public override ActiveList purge()
			{
				if (this.absoluteBeamWidth > 0 && this._size > this.absoluteBeamWidth)
				{
					this._size = this.partitioner.partition(this.tokenList, this._size, this.absoluteBeamWidth) + 1;
				}
				return this;
			}

			public override float getBeamThreshold()
			{
				return this.getBestScore() + this.logRelativeBeamWidth;
			}

			public override void setBestToken(Token token)
			{
				this.bestToken = token;
			}

			public override Token getBestToken()
			{
				return this.bestToken;
			}

			public override Iterator iterator()
			{
				return new TokenArrayIterator(this.tokenList, this._size);
			}

			public override List getTokens()
			{
				return Arrays.asList(this.tokenList).subList(0, this._size);
			}

			public override int size()
			{
				return this._size;
			}

			public override ActiveList newInstance()
			{
				return this.this_0.newInstance();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return new IterableEnumerator(this);
			}

			private int _size;

			private int absoluteBeamWidth;
			
			private float logRelativeBeamWidth;

			private Token bestToken;

			private Token[] tokenList;

			private Partitioner partitioner;
			
			internal PartitionActiveListFactory this_0;
		}
	}
}
