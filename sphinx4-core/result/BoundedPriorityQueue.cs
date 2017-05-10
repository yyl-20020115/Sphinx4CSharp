using System;
using System.Collections;

using IKVM.Attributes;
using ikvm.lang;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	[Implements(new string[]
	{
		"java.lang.Iterable"
	})]
	
	public class BoundedPriorityQueue : java.lang.Object, Iterable, IEnumerable
	{
		[LineNumberTable(new byte[]
		{
			159,
			165,
			104,
			107,
			103
		})]
		
		public BoundedPriorityQueue(int maxSize)
		{
			this.items = new TreeSet();
			this.maxSize = maxSize;
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			171,
			109,
			115,
			108
		})]
		
		public virtual void add(object item)
		{
			this.items.add(item);
			if (this.items.size() > this.maxSize)
			{
				this.items.pollFirst();
			}
		}

		
		
		public virtual int size()
		{
			return this.items.size();
		}

		
		
		
		public virtual object poll()
		{
			return this.items.pollLast();
		}

		
		
		
		public virtual Iterator iterator()
		{
			return this.items.iterator();
		}

		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new IterableEnumerator(this);
		}

		
		internal TreeSet items;

		internal int maxSize;
	}
}
