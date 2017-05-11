using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using java.util;

namespace edu.cmu.sphinx.frontend.databranch
{
	public class FrontEndSplitter : BaseDataProcessor, DataProducer, Configurable
	{
		public FrontEndSplitter()
		{
			this.listeners = new ArrayList();
		}

		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.listeners = ps.getComponentList("dataListeners", ClassLiteral<DataListener>.Value);
		}

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

		public virtual void addDataListener(DataListener l)
		{
			if (l == null)
			{
				return;
			}
			this.listeners.add(l);
		}

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
