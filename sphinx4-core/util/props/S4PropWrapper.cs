using java.lang.annotation;

namespace edu.cmu.sphinx.util.props
{
	public class S4PropWrapper : java.lang.Object
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
