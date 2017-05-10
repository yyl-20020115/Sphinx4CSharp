using System;
using IKVM.Attributes;
using java.lang.annotation;

namespace edu.cmu.sphinx.util.props
{
	[Implements(new string[]
	{
		"java.lang.annotation.Annotation"
	})]
	
	[AnnotationAttribute("edu.cmu.sphinx.util.props.S4DoubleAttribute")]
	[Documented(new object[]
	{
		64,
		"Ljava/lang/annotation/Documented;"
	})]
	[Retention(new object[]
	{
		64,
		"Ljava/lang/annotation/Retention;",
		"value",
		new object[]
		{
			101,
			"Ljava/lang/annotation/RetentionPolicy, IKVM/OpenJDK/Core, Version=7/2/4630/5, Culture=neutral, PublicKeyToken=0738eb9f132ed756;",
			"RUNTIME"
		}
	})]
	[Target(new object[]
	{
		64,
		"Ljava/lang/annotation/Target;",
		"value",
		new object[]
		{
			91,
			new object[]
			{
				101,
				"Ljava/lang/annotation/ElementType, IKVM/OpenJDK/Core, Version=7/2/4630/5, Culture=neutral, PublicKeyToken=0738eb9f132ed756;",
				"FIELD"
			}
		}
	})]
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

		public const double NOT_DEFINED = -918273645.12345;

		
		public static class __Fields
		{
			public const double NOT_DEFINED = -918273645.12345;
		}
	}
}
