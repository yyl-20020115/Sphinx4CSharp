using edu.cmu.sphinx.fst.semiring;
using edu.cmu.sphinx.fst.utils;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst.operations
{
	internal sealed class NShortestPaths_1 : Object, Comparator
	{		
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
			this.val_d = array;
			this.val_semiring = semiring;
		}
		
		public int compare(object obj, object obj2)
		{
			return this.compare((Pair)obj, (Pair)obj2);
		}
		
		bool Comparator.equals(object obj)
		{
			return Object.instancehelper_equals(this, obj);
		}

		internal float[] val_d;
		
		internal Semiring val_semiring;
	}
}
