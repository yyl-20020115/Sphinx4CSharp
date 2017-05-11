using IKVM.Attributes;
using java.lang.annotation;

namespace edu.cmu.sphinx.util.props
{
	[S4Property(new object[]
	{
		64,
		"Ledu/cmu/sphinx/util/props/S4Property;"
	})]
	public interface S4Integer : Annotation
	{
		[AnnotationDefault(-918273645)]
		int defaultValue();

		[AnnotationDefault(true)]
		bool mandatory();

		[AnnotationDefault(new object[]
		{
			91,
			-2147483647,
			2147483647
		})]
		int[] range();

		public const int NOT_DEFINED = -918273645;

		
		public static class __Fields
		{
			public const int NOT_DEFINED = -918273645;
		}
	}
}
