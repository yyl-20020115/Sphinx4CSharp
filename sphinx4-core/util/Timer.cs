using ikvm.@internal;
using java.lang;
using java.text;
using java.util.logging;

namespace edu.cmu.sphinx.util
{
	public class Timer : java.lang.Object
	{
		public virtual void start()
		{
			if (this.startTime != 0L)
			{
				this.notReliable = true;
				java.lang.System.@out.println(new StringBuilder().append(this.getName()).append(" timer.start() called without a stop()").toString());
			}
			this.startTime = java.lang.System.currentTimeMillis();
		}

		public virtual long stop()
		{
			if (this.startTime == 0L)
			{
				this.notReliable = true;
				java.lang.System.@out.println(new StringBuilder().append(this.getName()).append(" timer.stop() called without a start()").toString());
			}
			this.curTime = java.lang.System.currentTimeMillis() - this.startTime;
			this.startTime = 0L;
			if (this.curTime > this.maxTime)
			{
				this.maxTime = this.curTime;
			}
			if (this.curTime < this.minTime)
			{
				this.minTime = this.curTime;
			}
			this.count += 1L;
			this.sum += (double)this.curTime;
			return this.curTime;
		}

		public virtual void reset()
		{
			this.startTime = 0L;
			this.count = 0L;
			this.sum = (double)0f;
			this.minTime = long.MaxValue;
			this.maxTime = 0L;
			this.notReliable = false;
		}

		public virtual string getName()
		{
			return this.name;
		}

		private void showTimesShort(Logger logger)
		{
			double num = (double)0f;
			if (this.count == 0L)
			{
				return;
			}
			if (this.count > 0L)
			{
				num = this.sum / (double)this.count / 1000.0;
			}
			if (this.notReliable)
			{
				logger.info(new StringBuilder().append(Utilities.pad(this.name, 20)).append(' ').append("Not reliable.").toString());
			}
			else
			{
				logger.info(new StringBuilder().append(Utilities.pad(this.name, 20)).append(' ').append(Utilities.pad(java.lang.String.valueOf(this.count), 8)).append(this.fmtTime(this.curTime)).append(this.fmtTime(this.minTime)).append(this.fmtTime(this.maxTime)).append(this.fmtTime(num)).append(this.fmtTime(this.sum / 1000.0)).toString());
			}
		}

		private string fmtTime(double num)
		{
			return Utilities.pad(new StringBuilder().append(Timer.timeFormatter.format(num)).append('s').toString(), 10);
		}
		
		private string fmtTime(long num)
		{
			return this.fmtTime((double)num / 1000.0);
		}

		internal Timer(string text)
		{
			this.minTime = long.MaxValue;
			if (!Timer.assertionsDisabled && text == null)
			{
				object obj = "timers must have a name!";
				throw new AssertionError(obj);
			}
			this.name = text;
			this.reset();
		}

		public virtual bool isStarted()
		{
			return this.startTime > 0L;
		}

		public virtual void start(long time)
		{
			if (this.startTime != 0L)
			{
				this.notReliable = true;
				java.lang.System.@out.println(new StringBuilder().append(this.getName()).append(" timer.start() called without a stop()").toString());
			}
			if (time > java.lang.System.currentTimeMillis())
			{
				string text = "Start time is later than current time";
				throw new IllegalStateException(text);
			}
			this.startTime = time;
		}

		public virtual void dump(Logger logger)
		{
			this.showTimesShort(logger);
		}

		public virtual long getCount()
		{
			return this.count;
		}

		public virtual long getCurTime()
		{
			return this.curTime;
		}

		public virtual double getAverageTime()
		{
			if (this.count == 0L)
			{
				return (double)0f;
			}
			return this.sum / (double)this.count;
		}

		public virtual long getMinTime()
		{
			return this.minTime;
		}

		public virtual long getMaxTime()
		{
			return this.maxTime;
		}

		static Timer()
		{
			Timer.timeFormatter = new DecimalFormat("###0.0000");
		}

		private static DecimalFormat timeFormatter;

		private string name;

		private double sum;

		private long count;

		private long startTime;

		private long curTime;

		private long minTime;

		private long maxTime;

		private bool notReliable;
		
		internal static bool assertionsDisabled = !ClassLiteral<Timer>.Value.desiredAssertionStatus();
	}
}
