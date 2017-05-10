using System;
using IKVM.Attributes;
using java.lang.annotation;

namespace edu.cmu.sphinx.util.props
{
	[Implements(new string[]
	{
		"java.lang.annotation.Annotation"
	})]
	
	[AnnotationAttribute("edu.cmu.sphinx.util.props.S4PropertyAttribute")]
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
	public interface S4Property : Annotation
	{
	}
}
