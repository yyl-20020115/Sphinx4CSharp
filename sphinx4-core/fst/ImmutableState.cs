using java.lang;
using java.util;

namespace edu.cmu.sphinx.fst
{
	public class ImmutableState : State
	{
		protected internal ImmutableState(int numArcs) : base(0)
		{
			this.arcs = null;
			this.initialNumArcs = numArcs;
			this.arcs = new Arc[numArcs];
		}

		public override void setArc(int index, Arc arc)
		{
			this.arcs[index] = arc;
		}
		
		protected internal ImmutableState()
		{
			this.arcs = null;
		}
		
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
