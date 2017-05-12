using ikvm.@internal;
using java.lang;
using java.util;
using java.util.logging;

namespace edu.cmu.sphinx.util
{
	public class TimerPool : java.lang.Object
	{
		public static Timer getTimer(object owner, string timerName)
		{
			lock (ClassLiteral<TimerPool>.Value)
			{
				if (!TimerPool.weakRefTimerPool.containsKey(owner))
				{
					TimerPool.weakRefTimerPool.put(owner, new ArrayList());
				}
				List list = (List)TimerPool.weakRefTimerPool.get(owner);
				Iterator iterator = list.iterator();
				while (iterator.hasNext())
				{
					Timer timer = (Timer)iterator.next();
					if (java.lang.String.instancehelper_equals(timer.getName(), timerName))
					{
						return timer;
					}
				}
				Timer timer2 = new Timer(timerName);
				list.add(timer2);
				return timer2;
			}
		}

		public static void dumpAll(Logger logger)
		{
			TimerPool.showTimesShortTitle(logger);
			Iterator iterator = TimerPool.weakRefTimerPool.values().iterator();
			while (iterator.hasNext())
			{
				List list = (List)iterator.next();
				Iterator iterator2 = list.iterator();
				while (iterator2.hasNext())
				{
					Timer timer = (Timer)iterator2.next();
					timer.dump(logger);
				}
			}
		}

		private static void showTimesShortTitle(Logger logger)
		{
			string text = "Timers";
			string @string = new StringBuilder().append("# ----------------------------- ").append(text).append("----------------------------------------------------------- ").toString();
			logger.info(Utilities.pad(@string, 78));
			logger.info(new StringBuilder().append(Utilities.pad("# Name", 20)).append(' ').append(Utilities.pad("Count", 8)).append(Utilities.pad("CurTime", 10)).append(Utilities.pad("MinTime", 10)).append(Utilities.pad("MaxTime", 10)).append(Utilities.pad("AvgTime", 10)).append(Utilities.pad("TotTime", 10)).toString());
		}

		private TimerPool()
		{
		}

		public static int getNumCachedTimers()
		{
			int num = 0;
			Iterator iterator = TimerPool.weakRefTimerPool.values().iterator();
			while (iterator.hasNext())
			{
				List list = (List)iterator.next();
				num += list.size();
			}
			return num;
		}

		public static void resetAll()
		{
			Iterator iterator = TimerPool.weakRefTimerPool.values().iterator();
			while (iterator.hasNext())
			{
				List list = (List)iterator.next();
				Iterator iterator2 = list.iterator();
				while (iterator2.hasNext())
				{
					Timer timer = (Timer)iterator2.next();
					timer.reset();
				}
			}
		}
				
		private static Map weakRefTimerPool = new WeakHashMap();
	}
}
