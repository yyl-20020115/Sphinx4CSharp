using java.util;
using java.lang;

namespace edu.cmu.sphinx.frontend
{
	public class Signal : Object, Data
	{		
		protected internal Signal(long time)
		{
			this.time = time;
		}

		public virtual long getTime()
		{
			return this.time;
		}
		
		public virtual Map getProps()
		{
			if (this.props == null)
			{
				this.props = new HashMap();
			}
			return this.props;
		}
		
		private long time;
		
		private Map props;
	}
}
