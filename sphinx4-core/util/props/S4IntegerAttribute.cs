using System;
using System.ComponentModel;
using IKVM.Attributes;
using ikvm.@internal;

namespace edu.cmu.sphinx.util.props
{
	
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.S4Integer"
	})]
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class S4IntegerAttribute : AnnotationAttributeBase, S4Integer
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		public S4IntegerAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		public S4IntegerAttribute() : base(ClassLiteral<S4Integer>.Value)
		{
		}

		int S4Integer.defaultValue()
		{
			return base.getIntValue("defaultValue");
		}

		
		public int defaultValue
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

		int[] S4Integer.range()
		{
			return (int[])base.getValue("range");
		}

		
		public int[] range
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

		bool S4Integer.mandatory()
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
