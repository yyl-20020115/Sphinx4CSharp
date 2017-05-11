using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.databranch
{
	public class DataBufferProcessor : BaseDataProcessor, DataListener
	{				
		public virtual void addDataListener(DataListener l)
		{
			if (l == null)
			{
				return;
			}
			this.dataListeners.add(l);
		}
		
		public DataBufferProcessor(int maxBufferSize, bool waitIfEmpty, int waitTime, List listeners)
		{
			this.featureBuffer = new LinkedList();
			this.dataListeners = new ArrayList();
			this.initLogger();
			this.maxBufferSize = maxBufferSize;
			this.waitIfEmpty = waitIfEmpty;
			if (waitIfEmpty)
			{
				this.waitTime = (long)waitTime;
			}
			Iterator iterator = listeners.iterator();
			while (iterator.hasNext())
			{
				Configurable configurable = (Configurable)iterator.next();
				if (!DataBufferProcessor.assertionsDisabled && !(configurable is DataListener))
				{
					
					throw new AssertionError();
				}
				this.addDataListener((DataListener)configurable);
			}
		}		
		public DataBufferProcessor()
		{
			this.featureBuffer = new LinkedList();
			this.dataListeners = new ArrayList();
		}
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.maxBufferSize = ps.getInt("maxBufferSize");
			this.waitIfEmpty = ps.getBoolean("waitIfEmpty").booleanValue();
			if (this.waitIfEmpty)
			{
				this.waitTime = (long)ps.getInt("waitTimeMs");
			}
			this.dataListeners = ps.getComponentList("dataListeners", ClassLiteral<DataListener>.Value);
		}
		
		public virtual void processDataFrame(Data data)
		{
			this.featureBuffer.add(data);
			Iterator iterator = this.dataListeners.iterator();
			while (iterator.hasNext())
			{
				DataListener dataListener = (DataListener)iterator.next();
				dataListener.processDataFrame(data);
			}
			while (this.featureBuffer.size() > this.maxBufferSize)
			{
				this.featureBuffer.remove(0);
			}
		}
		
		public override Data getData()
		{
			Data result = null;
			while (this.waitIfEmpty && this.featureBuffer.isEmpty())
			{
				try
				{
					Thread.sleep(this.waitTime);
				}
				catch (InterruptedException ex)
				{
					Throwable.instancehelper_printStackTrace(ex);
				}
				continue;
			}
			if (!this.featureBuffer.isEmpty())
			{
				result = (Data)this.featureBuffer.remove(0);
			}
			else if (!DataBufferProcessor.assertionsDisabled && this.waitIfEmpty)
			{
				
				throw new AssertionError();
			}
			return result;
		}
		public virtual int getBufferSize()
		{
			return this.featureBuffer.size();
		}
		
		public virtual void clearBuffer()
		{
			this.featureBuffer.clear();
		}		
		
		public virtual List getBuffer()
		{
			return Collections.unmodifiableList(this.featureBuffer);
		}
		
		public virtual void removeDataListener(DataListener l)
		{
			if (l == null)
			{
				return;
			}
			this.dataListeners.remove(l);
		}
		
		private List featureBuffer;

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_WAIT_IF_EMPTY = "waitIfEmpty";

		private bool waitIfEmpty;

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			10
		})]
		public const string PROP_WAIT_TIME_MS = "waitTimeMs";

		private long waitTime;

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			50000
		})]
		public const string PROP_BUFFER_SIZE = "maxBufferSize";

		private int maxBufferSize;

		[S4ComponentList(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4ComponentList;",
			"type",
			new object[]
			{
				99,
				"Ledu/cmu/sphinx/util/props/Configurable, sphinx4-core-5prealpha-SNAPSHOT, Version=0/0/0/0, Culture=neutral, PublicKeyToken=null;"
			},
			"beTolerant",
			true
		})]
		public const string DATA_LISTENERS = "dataListeners";
		
		private List dataListeners;
		
		internal static bool assertionsDisabled = !ClassLiteral<DataBufferProcessor>.Value.desiredAssertionStatus();
	}
}
