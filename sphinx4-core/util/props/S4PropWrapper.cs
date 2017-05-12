using java.lang.annotation;
using java.lang;

namespace edu.cmu.sphinx.util.props
{
	public class S4PropWrapper : Object
	{
		public virtual Annotation getAnnotation()
		{
			return this.annotation;
		}

		public S4PropWrapper(Annotation annotation)
		{
			this.annotation = annotation;
		}

		private Annotation annotation;
	}
}
