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
	
	[EnclosingMethod("edu.cmu.sphinx.fst.operations.NShortestPaths", "get", "(Ledu.cmu.sphinx.fst.Fst;IZ)Ledu.cmu.sphinx.fst.Fst;")]
	[SourceFile("NShortestPaths.java")]
	internal sealed class NShortestPaths$1 : java.lang.Object, Comparator
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
			float f2 = this.val$d[((State)pair.getLeft()).getId()];
			float f3 = ((Float)pair2.getRight()).floatValue();
			float f4 = this.val$d[((State)pair2.getLeft()).getId()];
			float num = this.val$semiring.times(f3, f4);
			float num2 = this.val$semiring.times(f, f2);
			if (this.val$semiring.naturalLess(num, num2))
			{
				return 1;
			}
			if (num == num2)
			{
				return 0;
			}
			return -1;
		}

		
		
		internal NShortestPaths$1(float[] array, Semiring semiring)
		{
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((Pair)obj, (Pair)obj2);
		}

		
		bool Comparator.Object;)Zequals(object obj)
		{
			return Object.instancehelper_equals(this, obj);
		}

		
		internal float[] val$d = array;

		
		internal Semiring val$semiring = semiring;
	}
}
