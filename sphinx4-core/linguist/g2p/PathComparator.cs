using java.util;

namespace edu.cmu.sphinx.linguist.g2p
{
	public class PathComparator : java.lang.Object, Comparator
	{
		public PathComparator()
		{
		}
		
		public virtual int compare(Path o1, Path o2)
		{
			if (o1.getCost() < o2.getCost())
			{
				return -1;
			}
			if (o1.getCost() > o2.getCost())
			{
				return 1;
			}
			return 0;
		}
		
		public virtual int compare(object obj1, object obj2)
		{
			return this.compare((Path)obj1, (Path)obj2);
		}
		
		bool Comparator.equals(object obj)
		{
			return java.lang.Object.instancehelper_equals(this, obj);
		}
	}
}
