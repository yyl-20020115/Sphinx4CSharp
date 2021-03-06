﻿using edu.cmu.sphinx.frontend;
using java.util;
using java.util.concurrent;
using java.lang;

namespace edu.cmu.sphinx.decoder.scorer
{
	internal sealed class ThreadedAcousticScorer_1 : Object, Callable
	{
		public Scoreable call()
		{
			return ThreadedAcousticScorer.access_001(this.this_0, this.val_scoringJob, this.val_data);
		}

		object Callable.call()
		{
			return this.call();
		}

		internal ThreadedAcousticScorer_1(ThreadedAcousticScorer threadedAcousticScorer, List list, Data data)
		{
			this_0 = threadedAcousticScorer;
			this.val_scoringJob = list;
			this.val_data = data;
		}

		internal List val_scoringJob;
		
		internal Data val_data;
		
		internal ThreadedAcousticScorer this_0;
	}
}
