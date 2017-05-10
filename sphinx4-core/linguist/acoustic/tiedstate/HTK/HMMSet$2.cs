using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.HTK
{
	
	[Implements(new string[]
	{
		"java.util.Iterator"
	})]
	
	[EnclosingMethod("edu.cmu.sphinx.linguist.acoustic.tiedstate.HTK.HMMSet", "get3phIt", "()Ljava.util.Iterator;")]
	[SourceFile("HMMSet.java")]
	
	internal sealed class HMMSet$2 : java.lang.Object, Iterator
	{
		[LineNumberTable(new byte[]
		{
			17,
			120,
			98,
			127,
			10,
			120,
			104,
			101
		})]
		
		public SingleHMM next()
		{
			while (this.cur < this.this$0.__hmms.size())
			{
				List _hmms = this.this$0.__hmms;
				int num = this.cur;
				int num2 = num;
				this.cur = num + 1;
				SingleHMM singleHMM = (SingleHMM)_hmms.get(num2);
				if (java.lang.String.instancehelper_indexOf(singleHMM.getName(), 45) >= 0 || java.lang.String.instancehelper_indexOf(singleHMM.getName(), 43) >= 0)
				{
					return singleHMM;
				}
			}
			return null;
		}

		
		
		internal HMMSet$2(HMMSet hmmset)
		{
		}

		public void remove()
		{
		}

		public bool hasNext()
		{
			return false;
		}

		
		
		
		public object next()
		{
			return this.next();
		}

		internal int cur;

		
		internal HMMSet this$0 = hmmset;
	}
}
