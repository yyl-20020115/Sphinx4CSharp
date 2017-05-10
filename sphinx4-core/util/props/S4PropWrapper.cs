using System;

using IKVM.Attributes;
using java.lang;
using java.lang.annotation;

namespace edu.cmu.sphinx.util.props
{
	public class S4PropWrapper : java.lang.Object
	{
		public virtual Annotation getAnnotation()
		{
			return this.annotation;
		}

		[LineNumberTable(new byte[]
		{
			159,
			157,
			104,
			103
		})]
		
		public S4PropWrapper(Annotation annotation)
		{
			this.annotation = annotation;
		}

		
		private Annotation annotation;
	}
}
