using System;

using edu.cmu.sphinx.util.props;
using IKVM.Attributes;

namespace edu.cmu.sphinx.frontend
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.frontend.DataProcessor"
	})]
	public abstract class BaseDataProcessor : ConfigurableAdapter, DataProcessor, Configurable
	{
		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		public abstract Data getData();

		[LineNumberTable(new byte[]
		{
			159,
			168,
			102
		})]
		
		public BaseDataProcessor()
		{
		}

		public virtual void initialize()
		{
		}

		public virtual DataProcessor getPredecessor()
		{
			return this.predecessor;
		}

		public virtual void setPredecessor(DataProcessor predecessor)
		{
			this.predecessor = predecessor;
		}

		private DataProcessor predecessor;
	}
}
