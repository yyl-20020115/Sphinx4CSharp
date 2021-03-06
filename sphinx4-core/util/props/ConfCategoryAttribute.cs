﻿using ikvm.@internal;

namespace edu.cmu.sphinx.util.props
{
	public sealed class ConfCategoryAttribute : AnnotationAttributeBase, ConfCategory
	{
		public ConfCategoryAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		private ConfCategoryAttribute() : base(ClassLiteral<ConfCategory>.Value)
		{
		}
		
		public ConfCategoryAttribute(string[] value) : this()
		{
			base.setValue("value", value);
		}

		string[] ConfCategory.value()
		{
			return (string[])base.getValue("value");
		}
	}
}
