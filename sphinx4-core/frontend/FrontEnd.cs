using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend
{
	public class FrontEnd : BaseDataProcessor
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			115,
			145,
			103,
			127,
			1,
			149,
			104,
			140,
			104,
			135,
			103,
			98,
			102
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			74,
			102,
			127,
			1,
			102,
			98
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			156,
			127,
			6,
			105
		})]
		
		protected internal virtual void fireSignalListeners(Signal signal)
		{
			Iterator iterator = new ArrayList(this.signalListeners).iterator();
			while (iterator.hasNext())
			{
				SignalListener signalListener = (SignalListener)iterator.next();
				signalListener.signalOccurred(signal);
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			88,
			108
		})]
		
		public virtual void setDataSource(DataProcessor dataSource)
		{
			this.first.setPredecessor(dataSource);
		}

		
		[LineNumberTable(new byte[]
		{
			94,
			8,
			171,
			102,
			103,
			102
		})]
		
		public FrontEnd(List frontEndList)
		{
			this.signalListeners = new ArrayList();
			this.initLogger();
			this.frontEndList = frontEndList;
			this.init();
		}

		[LineNumberTable(new byte[]
		{
			100,
			232,
			56,
			235,
			74
		})]
		
		public FrontEnd()
		{
			this.signalListeners = new ArrayList();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			109,
			103,
			118,
			102
		})]
		
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

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			107,
			107,
			172,
			104,
			140,
			108
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			126,
			103
		})]
		
		public override void setPredecessor(DataProcessor dataSource)
		{
			this.setDataSource(dataSource);
		}

		[LineNumberTable(new byte[]
		{
			160,
			136,
			109
		})]
		
		public virtual void addSignalListener(SignalListener listener)
		{
			this.signalListeners.add(listener);
		}

		[LineNumberTable(new byte[]
		{
			160,
			146,
			109
		})]
		
		public virtual void removeSignalListener(SignalListener listener)
		{
			this.signalListeners.remove(listener);
		}

		public virtual DataProcessor getLastDataProcessor()
		{
			return this.last;
		}

		[LineNumberTable(new byte[]
		{
			160,
			177,
			104,
			127,
			1,
			102,
			106,
			39,
			137,
			118,
			124,
			117,
			110
		})]
		
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

		
		static FrontEnd()
		{
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

		private Timer timer;

		private DataProcessor first;

		private DataProcessor last;

		
		
		private List signalListeners;

		
		internal static bool assertionsDisabled = !ClassLiteral<FrontEnd>.Value.desiredAssertionStatus();
	}
}
