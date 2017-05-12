using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.util.props
{
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public sealed class S4ComponentListAttribute : AnnotationAttributeBase, S4ComponentList
	{		
		public S4ComponentListAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		private S4ComponentListAttribute() : base(ClassLiteral<S4ComponentList>.Value)
		{
		}
		
		public S4ComponentListAttribute(System.Type type) : this()
		{
			base.setValue("type", type);
		}

		Class S4ComponentList.type()
		{
			return (Class)base.getValue("type");
		}

		Class[] S4ComponentList.defaultList()
		{
			return (Class[])base.getValue("defaultList");
		}
		
		public System.Type[] defaultList
		{
			
			get
			{
				throw new System.NotImplementedException();
			}
			
			set
			{
				base.setValue("defaultList", value);
			}
		}

		bool S4ComponentList.beTolerant()
		{
			return base.getBooleanValue("beTolerant");
		}
		
		public bool beTolerant
		{
			
			get
			{
				throw new System.NotImplementedException();
			}
			
			set
			{
				base.setValue("beTolerant", value);
			}
		}
	}
}
