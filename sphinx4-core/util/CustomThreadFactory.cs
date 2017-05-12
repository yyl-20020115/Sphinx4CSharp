using java.lang;
using java.util.concurrent;
using java.util.concurrent.atomic;

namespace edu.cmu.sphinx.util
{
	public class CustomThreadFactory : Object, ThreadFactory
	{
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
	
		public virtual Thread newThread(Runnable r)
		{
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
	
		internal static AtomicInteger poolNumber = new AtomicInteger(1);

		internal ThreadGroup group;
		
		internal AtomicInteger threadNumber;

		internal string namePrefix;

		internal bool daemon;
		
		internal int priority;
	}
}
