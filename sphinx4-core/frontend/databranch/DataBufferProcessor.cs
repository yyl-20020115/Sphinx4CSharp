using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.databranch
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.frontend.databranch.DataListener"
	})]
	public class DataBufferProcessor : BaseDataProcessor, DataListener
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			105,
			99,
			129,
			109
		})]
		
		public virtual void addDataListener(DataListener l)
		{
			if (l == null)
			{
				return;
			}
			this.dataListeners.add(l);
		}

		
		[LineNumberTable(new byte[]
		{
			159,
			127,
			66,
			232,
			25,
			235,
			92,
			235,
			76,
			134,
			103,
			135,
			99,
			136,
			124,
			122,
			108,
			98
		})]
		
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

		[LineNumberTable(new byte[]
		{
			25,
			232,
			10,
			235,
			92,
			235,
			91
		})]
		
		public DataBufferProcessor()
		{
			this.featureBuffer = new LinkedList();
			this.dataListeners = new ArrayList();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			30,
			135,
			113,
			150,
			104,
			146,
			118
		})]
		
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

		[LineNumberTable(new byte[]
		{
			43,
			173,
			127,
			1,
			103,
			162,
			115,
			143
		})]
		
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

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			66,
			130,
			149,
			189,
			2,
			97,
			102,
			162,
			109,
			148,
			186
		})]
		
		public override Data getData()
		{
			Data result = null;
			while (this.waitIfEmpty && this.featureBuffer.isEmpty())
			{
				InterruptedException ex2;
				try
				{
					Thread.sleep(this.waitTime);
				}
				catch (InterruptedException ex)
				{
					ex2 = ByteCodeHelper.MapException<InterruptedException>(ex, 1);
					goto IL_31;
				}
				continue;
				IL_31:
				InterruptedException ex3 = ex2;
				Throwable.instancehelper_printStackTrace(ex3);
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

		[LineNumberTable(new byte[]
		{
			92,
			107
		})]
		
		public virtual void clearBuffer()
		{
			this.featureBuffer.clear();
		}

		
		
		
		public virtual List getBuffer()
		{
			return Collections.unmodifiableList(this.featureBuffer);
		}

		[LineNumberTable(new byte[]
		{
			116,
			99,
			129,
			109
		})]
		
		public virtual void removeDataListener(DataListener l)
		{
			if (l == null)
			{
				return;
			}
			this.dataListeners.remove(l);
		}

		
		static DataBufferProcessor()
		{
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
