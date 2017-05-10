using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;
using java.util.concurrent;

namespace edu.cmu.sphinx.decoder.scorer
{
	public class ThreadedAcousticScorer : SimpleAcousticScorer
	{
	
		
		
		
		internal static Scoreable access_001(ThreadedAcousticScorer threadedAcousticScorer, List list, Data data)
		{
			return threadedAcousticScorer.__(list, data);
		}

		private void init(int num, bool flag, int num2, int num3)
		{
			this.minScoreablesPerThread = num;
			if (flag)
			{
				num2 += Runtime.getRuntime().availableProcessors();
			}
			this.numThreads = num2;
			this.threadPriority = num3;
		}
	
		public ThreadedAcousticScorer(BaseDataProcessor frontEnd, ScoreNormalizer scoreNormalizer, int minScoreablesPerThread, bool cpuRelative, int numThreads, int threadPriority) : base(frontEnd, scoreNormalizer)
		{
			this.init(minScoreablesPerThread, cpuRelative, numThreads, threadPriority);
		}


		public ThreadedAcousticScorer()
		{
		}

		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.init(ps.getInt("minScoreablesPerThread"), ps.getBoolean("isCpuRelative").booleanValue(), ps.getInt("numThreads"), ps.getInt("threadPriority"));
		}
	
		public override void allocate()
		{
			base.allocate();
			if (this.executorService == null)
			{
				if (this.numThreads > 1)
				{
					this.logger.fine(new StringBuilder().append("# of scoring threads: ").append(this.numThreads).toString());
					int num = this.numThreads;
					this.executorService = Executors.newFixedThreadPool(num, new CustomThreadFactory(ThreadedAcousticScorer.className, true, this.threadPriority));
				}
				else
				{
					this.logger.fine("no scoring threads");
				}
			}
		}
		
		public override void deallocate()
		{
			base.deallocate();
			if (this.executorService != null)
			{
				this.executorService.shutdown();
				this.executorService = null;
			}
		}

				
		protected internal override Scoreable doScoring(List scoreableList, Data data)
		{
			if (this.numThreads > 1)
			{
				int num = scoreableList.size();
				int num2 = num + this.numThreads - 1;
				int num3 = this.numThreads;
				int num4 = java.lang.Math.max((num3 != -1) ? (num2 / num3) : (-num2), this.minScoreablesPerThread);
				if (num4 < num)
				{
					ArrayList arrayList = new ArrayList();
					int i = 0;
					int num5 = num4;
					while (i < num)
					{
						List list = scoreableList.subList(i, java.lang.Math.min(num5, num));
						arrayList.add(new ThreadedAcousticScorer_1(this, list, data));
						i = num5;
						num5 += num4;
					}
					ArrayList arrayList2 = new ArrayList(arrayList.size());
					try
					{
						Iterator iterator = this.executorService.invokeAll(arrayList).iterator();
						while (iterator.hasNext())
						{
							Future future = (Future)iterator.next();
							arrayList2.add(future.get());
						}
					}
					catch (System.Exception ex)
					{
						throw new DataProcessingException("No scoring jobs ended", ex);

					}
					return (Scoreable)Collections.min(arrayList2, Scoreable.COMPARATOR);

				}
			}
			return base.doScoring(scoreableList, data);
		}

		
		static ThreadedAcousticScorer()
		{
		}

		Scoreable __(List scoreableList, Data data)
		{
			return base.doScoring(scoreableList, data);
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			5
		})]
		public const string PROP_THREAD_PRIORITY = "threadPriority";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			0
		})]
		public const string PROP_NUM_THREADS = "numThreads";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_IS_CPU_RELATIVE = "isCpuRelative";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			10
		})]
		public const string PROP_MIN_SCOREABLES_PER_THREAD = "minScoreablesPerThread";

		
		private static string className = ClassLiteral<ThreadedAcousticScorer>.Value.getSimpleName();

		private int numThreads;

		private int threadPriority;

		private int minScoreablesPerThread;

		private ExecutorService executorService;
	}
}
