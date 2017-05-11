using java.lang.annotation;

namespace edu.cmu.sphinx.util.props
{
	public interface ConfCategory : Annotation
	{
		string[] value();
	}
}
