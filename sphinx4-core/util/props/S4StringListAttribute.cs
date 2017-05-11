using System;
using ikvm.@internal;

namespace edu.cmu.sphinx.util.props
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class S4StringListAttribute : AnnotationAttributeBase, S4StringList
	{
		public S4StringListAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		public S4StringListAttribute() : base(ClassLiteral<S4StringList>.Value)
		{
		}
	}
}
