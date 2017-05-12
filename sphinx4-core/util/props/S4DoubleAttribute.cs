using ikvm.@internal;

namespace edu.cmu.sphinx.util.props
{
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public sealed class S4DoubleAttribute : AnnotationAttributeBase, S4Double
	{
		public S4DoubleAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		public S4DoubleAttribute() : base(ClassLiteral<S4Double>.Value)
		{
		}

		double S4Double.defaultValue()
		{
			return base.getDoubleValue("defaultValue");
		}
		
		public double defaultValue
		{
			
			get
			{
				throw new System.NotImplementedException();
			}
			
			set
			{
				base.setValue("defaultValue", value);
			}
		}

		double[] S4Double.range()
		{
			return (double[])base.getValue("range");
		}

		public double[] range
		{
			
			get
			{
				throw new System.NotImplementedException();
			}
			
			set
			{
				base.setValue("range", value);
			}
		}

		bool S4Double.mandatory()
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
