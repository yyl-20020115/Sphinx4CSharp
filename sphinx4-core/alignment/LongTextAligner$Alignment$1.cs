using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment
{
	internal sealed class LongTextAligner_Alignment_1 : Object, Comparator
	{
		public int compare(LongTextAligner_Alignment_Node longTextAligner_Alignment_Node, LongTextAligner_Alignment_Node longTextAligner_Alignment_Node2)
		{
			return ((Integer)this.val_cost.get(longTextAligner_Alignment_Node)).compareTo((Integer)this.val_cost.get(longTextAligner_Alignment_Node2));
		}

		internal LongTextAligner_Alignment_1(LongTextAligner.Alignment alignment, LongTextAligner longTextAligner, Map map)
		{
			this_1 = alignment;
			this.val_this_0 = longTextAligner;
			this.val_cost = map;
		}

		public int compare(object obj, object obj2)
		{
			return this.compare((LongTextAligner_Alignment_Node)obj, (LongTextAligner_Alignment_Node)obj2);
		}

		bool Comparator.equals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}

		internal LongTextAligner val_this_0;

		internal Map val_cost;

		internal LongTextAligner.Alignment this_1;
	}
}
