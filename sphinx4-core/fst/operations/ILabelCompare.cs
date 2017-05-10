using System;
using System.ComponentModel;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	[Implements(new string[]
	{
		"java.util.Comparator"
	})]
	
	public class ILabelCompare : java.lang.Object, Comparator
	{
		[LineNumberTable(new byte[]
		{
			159,
			176,
			99,
			130,
			99,
			162,
			120,
			43
		})]
		
		public virtual int compare(Arc o1, Arc o2)
		{
			if (o1 == null)
			{
				return 1;
			}
			if (o2 == null)
			{
				return -1;
			}
			return (o1.getIlabel() >= o2.getIlabel()) ? ((o1.getIlabel() != o2.getIlabel()) ? 1 : 0) : -1;
		}

		
		
		public ILabelCompare()
		{
		}

		
		[EditorBrowsable(EditorBrowsableState.Never)]
		
		
		public virtual int compare(object obj1, object obj2)
		{
			return this.compare((Arc)obj1, (Arc)obj2);
		}

		
		bool Comparator.Object;)Zequals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}
	}
}
