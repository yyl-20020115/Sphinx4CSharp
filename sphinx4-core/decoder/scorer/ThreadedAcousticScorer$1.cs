using System;

using edu.cmu.sphinx.frontend;
using IKVM.Attributes;
using java.lang;
using java.util;
using java.util.concurrent;

namespace edu.cmu.sphinx.decoder.scorer
{

	
	internal sealed class ThreadedAcousticScorer_1 : java.lang.Object, Callable
	{

					
		public Scoreable call()
		{
			return ThreadedAcousticScorer.access_001(this.this_0, this.val_scoringJob, this.val_data);
		}

		
		
		internal ThreadedAcousticScorer_1(ThreadedAcousticScorer threadedAcousticScorer, List list, Data data)
		{
			this.val_scoringJob = list;
			this.val_data = data;
		}

		
		
		public object call()
		{
			return this.call();
		}

		
		internal List val_scoringJob;

		
		internal Data val_data;

		
		internal ThreadedAcousticScorer this_0 = threadedAcousticScorer;
	}
}
