using System;

using IKVM.Attributes;
using java.lang;
using java.util.concurrent;
using java.util.concurrent.atomic;

namespace edu.cmu.sphinx.util
{
	[Implements(new string[]
	{
		"java.util.concurrent.ThreadFactory"
	})]
	public class CustomThreadFactory : java.lang.Object, ThreadFactory
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			137,
			98,
			232,
			59,
			236,
			70,
			105,
			112,
			102,
			123,
			127,
			26,
			103,
			103
		})]
		
		public CustomThreadFactory(string namePrefix, bool daemon, int priority)
		{
			this.threadNumber = new AtomicInteger(1);
			if (priority > 10 || priority < 1)
			{
				string text = "illegal thread priority";
				
				throw new IllegalArgumentException(text);
			}
			SecurityManager securityManager = java.lang.System.getSecurityManager();
			this.group = ((securityManager == null) ? Thread.currentThread().getThreadGroup() : securityManager.getThreadGroup());
			this.namePrefix = new StringBuilder().append(namePrefix).append("-").append(CustomThreadFactory.poolNumber.getAndIncrement()).append("-thread-").toString();
			this.daemon = daemon;
			this.priority = priority;
		}

		[LineNumberTable(new byte[]
		{
			159,
			174,
			127,
			26,
			110,
			108,
			110,
			108
		})]
		
		public virtual Thread newThread(Runnable r)
		{
			Thread.__<clinit>();
			Thread thread = new Thread(this.group, r, new StringBuilder().append(this.namePrefix).append(this.threadNumber.getAndIncrement()).toString(), 0L);
			if (thread.isDaemon() != this.daemon)
			{
				thread.setDaemon(this.daemon);
			}
			if (thread.getPriority() != this.priority)
			{
				thread.setPriority(this.priority);
			}
			return thread;
		}

		
		static CustomThreadFactory()
		{
		}

		
		internal static AtomicInteger poolNumber = new AtomicInteger(1);

		
		internal ThreadGroup group;

		
		internal AtomicInteger threadNumber;

		
		internal string namePrefix;

		
		internal bool daemon;

		
		internal int priority;
	}
}
