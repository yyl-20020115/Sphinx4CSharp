using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using ikvm.@internal;
using java.util;

namespace edu.cmu.sphinx.frontend.databranch
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.frontend.databranch.DataProducer"
	})]
	public class FrontEndSplitter : BaseDataProcessor, DataProducer, Configurable
	{
		[LineNumberTable(new byte[]
		{
			159,
			169,
			8,
			171
		})]
		
		public FrontEndSplitter()
		{
			this.listeners = new ArrayList();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			174,
			135,
			118
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.listeners = ps.getComponentList("dataListeners", ClassLiteral<DataListener>.Value);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			189,
			140,
			127,
			1,
			137
		})]
		
		public override Data getData()
		{
			Data data = this.getPredecessor().getData();
			Iterator iterator = this.listeners.iterator();
			while (iterator.hasNext())
			{
				DataListener dataListener = (DataListener)iterator.next();
				dataListener.processDataFrame(data);
			}
			return data;
		}

		[LineNumberTable(new byte[]
		{
			7,
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
			this.listeners.add(l);
		}

		[LineNumberTable(new byte[]
		{
			15,
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
			this.listeners.remove(l);
		}

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
		public const string PROP_DATA_LISTENERS = "dataListeners";

		
		private List listeners;
	}
}
