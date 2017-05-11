﻿using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.HTK
{
	internal sealed class HMMSet_2 : java.lang.Object, Iterator
	{
		public SingleHMM next()
		{
			while (this.cur < this.this_0.__hmms.size())
			{
				List _hmms = this.this_0.__hmms;
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

		internal HMMSet_2(HMMSet hmmset)
		{
			this.this_0 = hmmset;
		}

		public void remove()
		{
		}

		public bool hasNext()
		{
			return false;
		}


		object Iterator.next()
		{
			return this.next();
		}

		internal int cur;

		
		internal HMMSet this_0;
	}
}
