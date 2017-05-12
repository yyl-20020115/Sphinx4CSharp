using java.util;
using java.lang;

namespace edu.cmu.sphinx.fst.operations
{
	public class OLabelCompare : Object, Comparator
	{		
		public virtual int compare(Arc o1, Arc o2)
		{
			if (o1 == null)
			{
				return 1;
			}
			if (o2 == null)
			{
				return -1;
			}
			return (o1.getOlabel() >= o2.getOlabel()) ? ((o1.getOlabel() != o2.getOlabel()) ? 1 : 0) : -1;
		}
		
		public OLabelCompare()
		{
		}		
		
		public virtual int compare(object obj1, object obj2)
		{
			return this.compare((Arc)obj1, (Arc)obj2);
		}
		
		bool Comparator.equals(object obj)
		{
			return Object.instancehelper_equals(this, obj);
		}
	}
}
