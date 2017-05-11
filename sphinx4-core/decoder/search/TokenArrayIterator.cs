using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.decoder.search
{
	[Implements(new string[]
	{
		"java.util.Iterator"
	})]
	
	.
	
	internal sealed class TokenArrayIterator : java.lang.Object, Iterator
	{
		[LineNumberTable(new byte[]
		{
			160,
			130,
			104,
			103,
			103,
			103
		})]
		
		internal TokenArrayIterator(Token[] array, int num)
		{
			this.tokenArray = array;
			this.pos = 0;
			this.size = num;
		}

		[Throws(new string[]
		{
			"java.util.NoSuchElementException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			145,
			111,
			139
		})]
		
		public Token next()
		{
			if (this.pos >= this.tokenArray.Length)
			{
				
				throw new NoSuchElementException();
			}
			Token[] array = this.tokenArray;
			int num = this.pos;
			int num2 = num;
			this.pos = num + 1;
			return array[num2];
		}

		public bool hasNext()
		{
			return this.pos < this.size;
		}

		
		
		public void remove()
		{
			string text = "TokenArrayIterator.remove() unimplemented";
			
			throw new Error(text);
		}

		
		
		
		public object next()
		{
			return this.next();
		}

		
		private Token[] tokenArray;

		
		private int size;

		private int pos;
	}
}
