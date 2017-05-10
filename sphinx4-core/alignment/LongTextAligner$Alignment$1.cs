using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.alignment
{
	
	[Implements(new string[]
	{
		"java.util.Comparator"
	})]
	
	[EnclosingMethod("edu.cmu.sphinx.alignment.LongTextAligner$Alignment", "<init>", "(Ledu.cmu.sphinx.alignment.LongTextAligner;Ljava.util.List;Ledu.cmu.sphinx.util.Range;)V")]
	[SourceFile("LongTextAligner.java")]
	
	internal sealed class LongTextAligner$Alignment$1 : java.lang.Object, Comparator
	{
		
		
		public int compare(LongTextAligner$Alignment$Node longTextAligner$Alignment$Node, LongTextAligner$Alignment$Node longTextAligner$Alignment$Node2)
		{
			return ((Integer)this.val$cost.get(longTextAligner$Alignment$Node)).compareTo((Integer)this.val$cost.get(longTextAligner$Alignment$Node2));
		}

		
		
		internal LongTextAligner$Alignment$1(LongTextAligner.Alignment alignment, LongTextAligner longTextAligner, Map map)
		{
			this.val$this$0 = longTextAligner;
			this.val$cost = map;
			base..ctor();
		}

		
		
		
		public int compare(object obj, object obj2)
		{
			return this.compare((LongTextAligner$Alignment$Node)obj, (LongTextAligner$Alignment$Node)obj2);
		}

		
		bool Comparator.Object;)Zequals(object obj)
		{
			return Object.instancehelper_equals(this, obj);
		}

		
		internal LongTextAligner val$this$0;

		
		internal Map val$cost;

		
		internal LongTextAligner.Alignment this$1 = alignment;
	}
}
