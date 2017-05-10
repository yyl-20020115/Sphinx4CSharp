using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util
{
	
	public class Cache : java.lang.Object
	{
		
		[LineNumberTable(new byte[]
		{
			159,
			175,
			109,
			99,
			144,
			142
		})]
		
		public virtual object cache(object @object)
		{
			object obj = this.map.get(@object);
			if (obj == null)
			{
				this.map.put(@object, @object);
			}
			else
			{
				this.hits++;
			}
			return obj;
		}

		[LineNumberTable(new byte[]
		{
			159,
			159,
			136,
			139
		})]
		
		public Cache()
		{
			this.map = new HashMap();
			this.hits = 0;
		}

		
		
		public virtual int getMisses()
		{
			return this.map.size();
		}

		public virtual int getHits()
		{
			return this.hits;
		}

		
		private Map map;

		private int hits;
	}
}
