using IKVM.Attributes;
using java.lang;
using java.lang.annotation;

namespace edu.cmu.sphinx.util.props
{
	[S4Property(new object[]
	{
		64,
		"Ledu/cmu/sphinx/util/props/S4Property;"
	})]
	public interface S4Double : Annotation
	{
		[AnnotationDefault(-918273645.12345)]
		double defaultValue();

		[AnnotationDefault(true)]
		bool mandatory();

		[AnnotationDefault(new object[]
		{
			91,
			-1.7976931348623157E+308,
			1.7976931348623157E+308
		})]
		double[] range();

	}

	public abstract class S4DoubleBase : java.lang.Object, S4Double
	{
		public const double NOT_DEFINED = -918273645.12345;

		public abstract Class annotationType();
		public abstract double defaultValue();
		public abstract bool mandatory();
		public abstract double[] range();

		public static class __Fields
		{
			public const double NOT_DEFINED = -918273645.12345;
		}

	}
}
