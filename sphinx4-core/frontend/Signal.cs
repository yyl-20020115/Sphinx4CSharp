using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.frontend.Data"
	})]
	public class Signal : java.lang.Object, Data
	{
		[LineNumberTable(new byte[]
		{
			159,
			186,
			104,
			103
		})]
		
		protected internal Signal(long time)
		{
			this.time = time;
		}

		public virtual long getTime()
		{
			return this.time;
		}

		
		[LineNumberTable(new byte[]
		{
			13,
			104,
			139
		})]
		
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
