using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst
{
	public class State : java.lang.Object
	{
		public virtual int getId()
		{
			return this.id;
		}

		public virtual float getFinalWeight()
		{
			return this.fnlWeight;
		}

		
		
		public virtual int getNumArcs()
		{
			return this.arcs.size();
		}

		
		
		public virtual Arc getArc(int index)
		{
			return (Arc)this.arcs.get(index);
		}

		[LineNumberTable(new byte[]
		{
			5,
			104,
			104
		})]
		
		public State(float fnlWeight) : this()
		{
			this.fnlWeight = fnlWeight;
		}

		[LineNumberTable(new byte[]
		{
			78,
			109
		})]
		
		public virtual void addArc(Arc arc)
		{
			this.arcs.add(arc);
		}

		public virtual void setFinalWeight(float fnlfloat)
		{
			this.fnlWeight = fnlfloat;
		}

		[LineNumberTable(new byte[]
		{
			15,
			232,
			30,
			231,
			70,
			167,
			231,
			90,
			103,
			100,
			140
		})]
		
		public State(int initialNumArcs)
		{
			this.id = -1;
			this.arcs = null;
			this.initialNumArcs = -1;
			this.initialNumArcs = initialNumArcs;
			if (initialNumArcs > 0)
			{
				this.arcs = new ArrayList(initialNumArcs);
			}
		}

		[LineNumberTable(new byte[]
		{
			98,
			100,
			98,
			99,
			98,
			110,
			98,
			103,
			110,
			98,
			110,
			113,
			103,
			130,
			104,
			104,
			98,
			115,
			98
		})]
		
		public override bool equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (this.GetType() != obj.GetType())
			{
				return false;
			}
			State state = (State)obj;
			if (this.id != state.id)
			{
				return false;
			}
			if (this.fnlWeight != state.fnlWeight && Float.floatToIntBits(this.fnlWeight) != Float.floatToIntBits(state.fnlWeight))
			{
				return false;
			}
			if (this.arcs == null)
			{
				if (state.arcs != null)
				{
					return false;
				}
			}
			else if (!this.arcs.equals(state.arcs))
			{
				return false;
			}
			return true;
		}

		public override int hashCode()
		{
			return this.id * 991;
		}

		
		public virtual void setArcs(ArrayList arcs)
		{
			this.arcs = arcs;
		}

		[LineNumberTable(new byte[]
		{
			160,
			95,
			110
		})]
		
		public virtual void setArc(int index, Arc arc)
		{
			this.arcs.set(index, arc);
		}

		[LineNumberTable(new byte[]
		{
			159,
			187,
			232,
			50,
			231,
			70,
			167,
			231,
			70,
			107
		})]
		
		protected internal State()
		{
			this.id = -1;
			this.arcs = null;
			this.initialNumArcs = -1;
			this.arcs = new ArrayList();
		}

		
		[LineNumberTable(new byte[]
		{
			27,
			108
		})]
		
		public virtual void arcSort(Comparator cmp)
		{
			Collections.sort(this.arcs, cmp);
		}

		[LineNumberTable(new byte[]
		{
			127,
			102,
			127,
			38
		})]
		
		public override string toString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.append(new StringBuilder().append("(").append(this.id).append(", ").append(this.fnlWeight).append(")").toString());
			return stringBuilder.toString();
		}

		
		
		public virtual Arc deleteArc(int index)
		{
			return (Arc)this.arcs.remove(index);
		}

		protected internal int id;

		private float fnlWeight;

		
		private ArrayList arcs;

		protected internal int initialNumArcs;
	}
}
