using System;

using edu.cmu.sphinx.fst.semiring;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.g2p
{
	public class Path : java.lang.Object
	{
		
		public virtual ArrayList getPath()
		{
			return this.path;
		}

		[LineNumberTable(new byte[]
		{
			159,
			191,
			108
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			159,
			180,
			104,
			103,
			103,
			113
		})]
		
		public Path(ArrayList path, Semiring semiring)
		{
			this.path = path;
			this.semiring = semiring;
			this.cost = this.semiring.zero();
		}

		[LineNumberTable(new byte[]
		{
			41,
			102,
			127,
			7,
			127,
			1,
			104,
			105,
			98
		})]
		
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
			return java.lang.String.instancehelper_trim(stringBuilder.toString());
		}

		
		private ArrayList path;

		private float cost;

		private Semiring semiring;
	}
}
