using System;
using System.ComponentModel;
using IKVM.Attributes;
using ikvm.@internal;

namespace edu.cmu.sphinx.util.props
{
	
	
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.S4Property"
	})]
	public sealed class S4PropertyAttribute : AnnotationAttributeBase, S4Property
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		public S4PropertyAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		public S4PropertyAttribute() : base(ClassLiteral<S4Property>.Value)
		{
		}
	}
}
