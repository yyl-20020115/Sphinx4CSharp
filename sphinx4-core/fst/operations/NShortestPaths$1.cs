using System;

using edu.cmu.sphinx.fst.semiring;
using edu.cmu.sphinx.fst.utils;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	
	[Implements(new string[]
	{
		"java.util.Comparator"
	})]
	
	
	.
	internal sealed class NShortestPaths_1 : java.lang.Object, Comparator
	{
		
		[LineNumberTable(new byte[]
		{
			89,
			113,
			152,
			113,
			152,
			111,
			143,
			113,
			130,
			102,
			130
		})]
		
		public int compare(Pair pair, Pair pair2)
		{
			float f = ((Float)pair.getRight()).floatValue();
			float f2 = this.val_d[((State)pair.getLeft()).getId()];
			float f3 = ((Float)pair2.getRight()).floatValue();
			float f4 = this.val_d[((State)pair2.getLeft()).getId()];
			float num = this.val_semiring.times(f3, f4);
			float num2 = this.val_semiring.times(f, f2);
			if (this.val_semiring.naturalLess(num, num2))
			{
				return 1;
			}
			if (num == num2)
			{
				return 0;
			}
			return -1;
		}

		
		
		internal NShortestPaths_1(float[] array, Semiring semiring)
		{
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((Pair)obj, (Pair)obj2);
		}

		
		bool Comparator.equals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}

		
		internal float[] val_d = array;

		
		internal Semiring val_semiring = semiring;
	}
}
