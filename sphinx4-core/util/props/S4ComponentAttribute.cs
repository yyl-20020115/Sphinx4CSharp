using System;
using System.ComponentModel;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.util.props
{
	
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.S4Component"
	})]
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class S4ComponentAttribute : AnnotationAttributeBase, S4Component
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		public S4ComponentAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		private S4ComponentAttribute() : base(ClassLiteral<S4Component>.Value)
		{
		}

		
		public S4ComponentAttribute(Type type) : this()
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

		
		public Type defaultClass
		{
			
			get
			{
				throw new NotImplementedException();
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
				throw new NotImplementedException();
			}
			
			set
			{
				base.setValue("mandatory", value);
			}
		}
	}
}
