using System;

using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst
{
	public class ImmutableState : State
	{
		[LineNumberTable(new byte[]
		{
			159,
			182,
			233,
			44,
			231,
			85,
			103,
			108
		})]
		
		protected internal ImmutableState(int numArcs) : base(0)
		{
			this.arcs = null;
			this.initialNumArcs = numArcs;
			this.arcs = new Arc[numArcs];
		}

		[LineNumberTable(new byte[]
		{
			24,
			105
		})]
		public override void setArc(int index, Arc arc)
		{
			this.arcs[index] = arc;
		}

		[LineNumberTable(new byte[]
		{
			159,
			173,
			232,
			53,
			231,
			76
		})]
		
		protected internal ImmutableState()
		{
			this.arcs = null;
		}

		
		[LineNumberTable(new byte[]
		{
			2,
			108
		})]
		
		public override void arcSort(Comparator cmp)
		{
			Arrays.sort(this.arcs, cmp);
		}

		
		
		public override void addArc(Arc arc)
		{
			string text = "You cannot modify an ImmutableState.";
			
			throw new IllegalArgumentException(text);
		}

		
		
		public override Arc deleteArc(int index)
		{
			string text = "You cannot modify an ImmutableState.";
			
			throw new IllegalArgumentException(text);
		}

		public virtual void setArcs(Arc[] arcs)
		{
			this.arcs = arcs;
		}

		public override int getNumArcs()
		{
			return this.initialNumArcs;
		}

		
		public override Arc getArc(int index)
		{
			return this.arcs[index];
		}

		public override int hashCode()
		{
			int num = 1;
			return 31 * num + this.id;
		}

		[LineNumberTable(new byte[]
		{
			87,
			100,
			98,
			110,
			98,
			103,
			115,
			98,
			105,
			98
		})]
		
		public override bool equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (this.GetType() != obj.GetType())
			{
				return false;
			}
			ImmutableState immutableState = (ImmutableState)obj;
			return Arrays.equals(this.arcs, immutableState.arcs) && base.equals(obj);
		}

		private Arc[] arcs;
	}
}
