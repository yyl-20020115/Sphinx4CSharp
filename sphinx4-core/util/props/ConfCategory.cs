using IKVM.Attributes;
using java.lang.annotation;

namespace edu.cmu.sphinx.util.props
{
	[AnnotationAttribute("edu.cmu.sphinx.util.props.ConfCategoryAttribute")]
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
	public interface ConfCategory : Annotation
	{
		string[] value();
	}
}
