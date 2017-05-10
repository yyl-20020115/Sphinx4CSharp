using System;
using System.ComponentModel;
using IKVM.Attributes;
using ikvm.@internal;

namespace edu.cmu.sphinx.util.props
{
	
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.S4String"
	})]
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class S4StringAttribute : AnnotationAttributeBase, S4String
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		public S4StringAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		public S4StringAttribute() : base(ClassLiteral<S4String>.Value)
		{
		}

		string S4String.defaultValue()
		{
			return (string)base.getValue("defaultValue");
		}

		
		public string defaultValue
		{
			
			get
			{
				throw new NotImplementedException();
			}
			
			set
			{
				base.setValue("defaultValue", value);
			}
		}

		string[] S4String.range()
		{
			return (string[])base.getValue("range");
		}

		
		public string[] range
		{
			
			get
			{
				throw new NotImplementedException();
			}
			
			set
			{
				base.setValue("range", value);
			}
		}

		bool S4String.mandatory()
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
