using ikvm.lang;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.result
{
	public class BoundedPriorityQueue : java.lang.Object, Iterable, System.Collections.IEnumerable
	{
		public BoundedPriorityQueue(int maxSize)
		{
			this.items = new TreeSet();
			this.maxSize = maxSize;
		}
		
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

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new IterableEnumerator(this);
		}
		
		internal TreeSet items;

		internal int maxSize;
	}
}
