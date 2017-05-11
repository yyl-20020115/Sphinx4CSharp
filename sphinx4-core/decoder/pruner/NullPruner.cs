using edu.cmu.sphinx.decoder.search;
using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.decoder.pruner
{
	public class NullPruner : java.lang.Object, Pruner, Configurable
	{
		public virtual void newProperties(PropertySheet ps)
		{
		}
		
		public NullPruner()
		{
		}

		public virtual void startRecognition()
		{
		}

		public virtual ActiveList prune(ActiveList activeList)
		{
			return activeList;
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
	}
}
