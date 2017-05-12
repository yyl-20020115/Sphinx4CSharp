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
	public interface S4ComponentList : Annotation
	{
		[AnnotationDefault(new object[]
		{
			91
		})]
		
		Class[] defaultList();

		[AnnotationDefault(false)]
		bool beTolerant();
		
		Class type();
	}
}
