using System;

using IKVM.Attributes;
using java.lang;
using java.util.logging;

namespace edu.cmu.sphinx.util.props
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public abstract class ConfigurableAdapter : java.lang.Object, Configurable
	{
		private void init(string text, Logger logger)
		{
			this.name = text;
			this.logger = logger;
		}

		
		
		public virtual string getName()
		{
			return (this.name == null) ? Object.instancehelper_getClass(this).getSimpleName() : this.name;
		}

		[LineNumberTable(new byte[]
		{
			159,
			158,
			102
		})]
		
		public ConfigurableAdapter()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			162,
			113,
			119
		})]
		
		protected internal virtual void initLogger()
		{
			this.name = Object.instancehelper_getClass(this).getSimpleName();
			this.init(this.name, Logger.getLogger(this.name));
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			167,
			114
		})]
		
		public virtual void newProperties(PropertySheet ps)
		{
			this.init(ps.getInstanceName(), ps.getLogger());
		}

		
		
		public override string toString()
		{
			return this.getName();
		}

		private string name;

		protected internal Logger logger;
	}
}
