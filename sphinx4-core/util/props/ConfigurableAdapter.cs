using java.util.logging;

namespace edu.cmu.sphinx.util.props
{
	public abstract class ConfigurableAdapter : java.lang.Object, Configurable
	{
		private void init(string text, Logger logger)
		{
			this.name = text;
			this.logger = logger;
		}
		
		public virtual string getName()
		{
			return (this.name == null) ? java.lang.Object.instancehelper_getClass(this).getSimpleName() : this.name;
		}
		
		public ConfigurableAdapter()
		{
		}
		
		protected internal virtual void initLogger()
		{
			this.name = java.lang.Object.instancehelper_getClass(this).getSimpleName();
			this.init(this.name, Logger.getLogger(this.name));
		}
		
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
