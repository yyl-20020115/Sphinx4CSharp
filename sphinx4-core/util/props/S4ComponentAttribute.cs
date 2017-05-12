using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.util.props
{
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public sealed class S4ComponentAttribute : AnnotationAttributeBase, S4Component
	{
		public S4ComponentAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		private S4ComponentAttribute() : base(ClassLiteral<S4Component>.Value)
		{
		}

		public S4ComponentAttribute(System.Type type) : this()
		{
			base.setValue("type", type);
		}

		Class S4Component.type()
		{
			return (Class)base.getValue("type");
		}

		Class S4Component.defaultClass()
		{
			return (Class)base.getValue("defaultClass");
		}

		public System.Type defaultClass
		{
			
			get
			{
				throw new System.NotImplementedException();
			}
			
			set
			{
				base.setValue("defaultClass", value);
			}
		}

		bool S4Component.mandatory()
		{
			return base.getBooleanValue("mandatory");
		}
		
		public bool mandatory
		{
			
			get
			{
				throw new System.NotImplementedException();
			}
			
			set
			{
				base.setValue("mandatory", value);
			}
		}
	}
}
