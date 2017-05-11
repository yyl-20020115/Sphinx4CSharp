using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.frontend
{
	public abstract class BaseDataProcessor : ConfigurableAdapter, DataProcessor, Configurable
	{
		public abstract Data getData();
		
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
