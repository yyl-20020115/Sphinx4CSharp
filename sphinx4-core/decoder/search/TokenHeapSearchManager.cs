using edu.cmu.sphinx.decoder.scorer;
using edu.cmu.sphinx.linguist;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search
{
	public class TokenHeapSearchManager : WordPruningBreadthFirstSearchManager
	{
		public TokenHeapSearchManager()
		{
			this.__maxTokenHeapSize = 3;
		}
		
		protected internal override void createBestTokenMap()
		{
			int num = this.activeList.size() << 2;
			if (num == 0)
			{
				num = 1;
			}
			this.bestTokenMap = new HashMap(num, 0.3f);
		}
		
		protected internal override void setBestToken(Token token, SearchState state)
		{
			TokenHeapSearchManager.TokenHeap tokenHeap = (TokenHeapSearchManager.TokenHeap)this.bestTokenMap.get(state);
			if (tokenHeap == null)
			{
				tokenHeap = new TokenHeapSearchManager.TokenHeap(this, 3);
				this.bestTokenMap.put(state, tokenHeap);
			}
			tokenHeap.add(token);
		}
		
		protected internal override Token getBestToken(SearchState state)
		{
			TokenHeapSearchManager.TokenHeap tokenHeap = (TokenHeapSearchManager.TokenHeap)this.bestTokenMap.get(state);
			if (tokenHeap == null)
			{
				return null;
			}
			Token result;
			if ((result = tokenHeap.get(state)) != null)
			{
				return result;
			}
			if (!tokenHeap.isFull())
			{
				return null;
			}
			return tokenHeap.getSmallest();
		}
		protected internal int maxTokenHeapSize
		{
			
			get
			{
				return this.__maxTokenHeapSize;
			}
			
			private set
			{
				this.__maxTokenHeapSize = value;
			}
		}

		internal int __maxTokenHeapSize;

		internal new Map bestTokenMap;
		
		internal sealed class TokenHeap : java.lang.Object
		{
			
			private bool tryReplace(Token token)
			{
				int i = 0;
				while (i < this.curSize)
				{
					if (java.lang.Object.instancehelper_equals(token.getSearchState(), this.tokens[i].getSearchState()))
					{
						if (!TokenHeapSearchManager.TokenHeap.assertionsDisabled && token.getScore() <= this.tokens[i].getScore())
						{
							
							throw new AssertionError();
						}
						this.tokens[i] = token;
						return true;
					}
					else
					{
						i++;
					}
				}
				return false;
			}
			
			private void fixupInsert()
			{
				Arrays.sort(this.tokens, 0, this.curSize - 1, Scoreable.COMPARATOR);
			}
			
			internal TokenHeap(TokenHeapSearchManager tokenHeapSearchManager, int num)
			{
				this.tokens = new Token[num];
			}
			
			internal void add(Token token)
			{
				if (!this.tryReplace(token))
				{
					if (this.curSize < this.tokens.Length)
					{
						Token[] array = this.tokens;
						int num = this.curSize;
						int num2 = num;
						this.curSize = num + 1;
						array[num2] = token;
					}
					else if (token.getScore() > this.tokens[this.curSize - 1].getScore())
					{
						this.tokens[this.curSize - 1] = token;
					}
				}
				this.fixupInsert();
			}
			internal Token getSmallest()
			{
				if (this.curSize == 0)
				{
					return null;
				}
				return this.tokens[this.curSize - 1];
			}
			internal bool isFull()
			{
				return this.curSize == this.tokens.Length;
			}


			internal Token get(SearchState searchState)
			{
				for (int i = 0; i < this.curSize; i++)
				{
					if (java.lang.Object.instancehelper_equals(this.tokens[i].getSearchState(), searchState))
					{
						return this.tokens[i];
					}
				}
				return null;
			}
			
			internal Token[] tokens;

			internal int curSize;

			
			internal static bool assertionsDisabled = !ClassLiteral<TokenHeapSearchManager>.Value.desiredAssertionStatus();

			
			internal TokenHeapSearchManager this_0 = tokenHeapSearchManager;
		}
	}
}
