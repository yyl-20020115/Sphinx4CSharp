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
	public interface S4String : Annotation
	{
		[AnnotationDefault("nullnullnull")]
		string defaultValue();

		[AnnotationDefault(true)]
		bool mandatory();

		[AnnotationDefault(new object[]
		{
			91
		})]
		string[] range();
	}
	
	public abstract class S4StringBase : java.lang.Object, S4String
	{
		public const string NOT_DEFINED = "nullnullnull";

		public abstract Class annotationType();
		public abstract string defaultValue();
		public abstract bool mandatory();
		public abstract string[] range();

		public static class __Fields
		{
			public const string NOT_DEFINED = "nullnullnull";
		}
	}
}
