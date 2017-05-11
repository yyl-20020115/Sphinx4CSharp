using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend
{
	public class FrontEnd : BaseDataProcessor
	{
		private void init()
		{
			this.timer = TimerPool.getTimer(this, "Frontend");
			this.last = null;
			Iterator iterator = this.frontEndList.iterator();
			while (iterator.hasNext())
			{
				DataProcessor dataProcessor = (DataProcessor)iterator.next();
				if (!FrontEnd.assertionsDisabled && dataProcessor == null)
				{
					
					throw new AssertionError();
				}
				if (this.last != null)
				{
					dataProcessor.setPredecessor(this.last);
				}
				if (this.first == null)
				{
					this.first = dataProcessor;
				}
				this.last = dataProcessor;
			}
			this.initialize();
		}

		public override void initialize()
		{
			base.initialize();
			Iterator iterator = this.frontEndList.iterator();
			while (iterator.hasNext())
			{
				DataProcessor dataProcessor = (DataProcessor)iterator.next();
				dataProcessor.initialize();
			}
		}
	
		protected internal virtual void fireSignalListeners(Signal signal)
		{
			Iterator iterator = new ArrayList(this.signalListeners).iterator();
			while (iterator.hasNext())
			{
				SignalListener signalListener = (SignalListener)iterator.next();
				signalListener.signalOccurred(signal);
			}
		}
		
		public virtual void setDataSource(DataProcessor dataSource)
		{
			this.first.setPredecessor(dataSource);
		}
	
		public FrontEnd(List frontEndList)
		{
			this.signalListeners = new ArrayList();
			this.initLogger();
			this.frontEndList = frontEndList;
			this.init();
		}
	
		public FrontEnd()
		{
			this.signalListeners = new ArrayList();
		}
	
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.frontEndList = ps.getComponentList("pipeline", ClassLiteral<DataProcessor>.Value);
			this.init();
		}
		
		public virtual List getElements()
		{
			return this.frontEndList;
		}
	
		public override Data getData()
		{
			this.timer.start();
			Data data = this.last.getData();
			if (data is Signal)
			{
				this.fireSignalListeners((Signal)data);
			}
			this.timer.stop();
			return data;
		}		
		public override void setPredecessor(DataProcessor dataSource)
		{
			this.setDataSource(dataSource);
		}

		public virtual void addSignalListener(SignalListener listener)
		{
			this.signalListeners.add(listener);
		}
		
		public virtual void removeSignalListener(SignalListener listener)
		{
			this.signalListeners.remove(listener);
		}

		public virtual DataProcessor getLastDataProcessor()
		{
			return this.last;
		}
		
		public override string toString()
		{
			if (this.last == null)
			{
				return new StringBuilder().append(base.toString()).append(" {}").toString();
			}
			LinkedList linkedList = new LinkedList();
			for (DataProcessor predecessor = this.last; predecessor != null; predecessor = predecessor.getPredecessor())
			{
				linkedList.addFirst(predecessor);
			}
			StringBuilder stringBuilder = new StringBuilder(base.toString()).append(" {");
			Iterator iterator = linkedList.iterator();
			while (iterator.hasNext())
			{
				DataProcessor dataProcessor = (DataProcessor)iterator.next();
				stringBuilder.append(dataProcessor).append(", ");
			}
			stringBuilder.setLength(stringBuilder.length() - 2);
			return stringBuilder.append('}').toString();
		}

		[S4ComponentList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4ComponentList;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/frontend/DataProcessor, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			}
		})]
		public const string PROP_PIPELINE = "pipeline";

		
		private List frontEndList;

		private sphinx.util.Timer timer;

		private DataProcessor first;

		private DataProcessor last;
		
		private List signalListeners;
		
		internal static bool assertionsDisabled = !ClassLiteral<FrontEnd>.Value.desiredAssertionStatus();
	}
}
