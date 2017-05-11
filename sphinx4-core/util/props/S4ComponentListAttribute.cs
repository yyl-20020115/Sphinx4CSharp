using System;
using System.ComponentModel;
using IKVM.Attributes;
using ikvm.@internal;
using java.lang;

namespace edu.cmu.sphinx.util.props
{
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.S4ComponentList"
	})]
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class S4ComponentListAttribute : AnnotationAttributeBase, S4ComponentList
	{
		
		public S4ComponentListAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		private S4ComponentListAttribute() : base(ClassLiteral<S4ComponentList>.Value)
		{
		}

		
		public S4ComponentListAttribute(Type type) : this()
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

		
		public Type[] defaultList
		{
			
			get
			{
				throw new NotImplementedException();
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
				throw new NotImplementedException();
			}
			
			set
			{
				base.setValue("beTolerant", value);
			}
		}
	}
}
