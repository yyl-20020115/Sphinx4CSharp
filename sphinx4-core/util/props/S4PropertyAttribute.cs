using ikvm.@internal;

namespace edu.cmu.sphinx.util.props
{
	public sealed class S4PropertyAttribute : AnnotationAttributeBase, S4Property
	{
		public S4PropertyAttribute(object[] definition) : this()
		{
			base.setDefinition(definition);
		}

		public S4PropertyAttribute() : base(ClassLiteral<S4Property>.Value)
		{
		}
	}
}
