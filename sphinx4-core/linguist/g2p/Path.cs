using edu.cmu.sphinx.fst.semiring;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.g2p
{
	public class Path : Object
	{
		public virtual ArrayList getPath()
		{
			return this.path;
		}
		
		public Path(Semiring semiring) : this(new ArrayList(), semiring)
		{
		}

		public virtual void setCost(float cost)
		{
			this.cost = cost;
		}

		public virtual float getCost()
		{
			return this.cost;
		}

		public virtual void setPath(ArrayList path)
		{
			this.path = path;
		}
		
		public Path(ArrayList path, Semiring semiring)
		{
			this.path = path;
			this.semiring = semiring;
			this.cost = this.semiring.zero();
		}
		
		public override string toString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.append(new StringBuilder().append(this.cost).append("\t").toString());
			Iterator iterator = this.path.iterator();
			while (iterator.hasNext())
			{
				string text = (string)iterator.next();
				stringBuilder.append(text);
				stringBuilder.append(' ');
			}
			return String.instancehelper_trim(stringBuilder.toString());
		}
		
		private ArrayList path;

		private float cost;

		private Semiring semiring;
	}
}
