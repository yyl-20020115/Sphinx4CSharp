using java.util;

namespace edu.cmu.sphinx.frontend
{
	public class Signal : java.lang.Object, Data
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
