using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.util.props;
using java.lang;

namespace edu.cmu.sphinx.decoder.pruner
{
	public class SimplePruner : Object, Pruner, Configurable
	{
		public virtual void newProperties(PropertySheet ps)
		{
		}		
		public SimplePruner()
		{
		}

		public virtual string getName()
		{
			return this.name;
		}

		public virtual void startRecognition()
		{
		}
		
		public virtual ActiveList prune(ActiveList activeList)
		{
			return activeList.purge();
		}

		public virtual void stopRecognition()
		{
		}

		public virtual void allocate()
		{
		}

		public virtual void deallocate()
		{
		}

		private string name;
	}
}
