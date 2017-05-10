using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.fst
{
	public class Arc : java.lang.Object
	{
		[LineNumberTable(new byte[]
		{
			159,
			181,
			102
		})]
		
		public Arc()
		{
		}

		[LineNumberTable(new byte[]
		{
			0,
			104,
			104,
			103,
			103,
			104
		})]
		
		public Arc(int iLabel, int oLabel, float weight, State nextState)
		{
			this.weight = weight;
			this.iLabel = iLabel;
			this.oLabel = oLabel;
			this.nextState = nextState;
		}

		public virtual float getWeight()
		{
			return this.weight;
		}

		public virtual void setWeight(float weight)
		{
			this.weight = weight;
		}

		public virtual int getIlabel()
		{
			return this.iLabel;
		}

		public virtual void setIlabel(int iLabel)
		{
			this.iLabel = iLabel;
		}

		public virtual int getOlabel()
		{
			return this.oLabel;
		}

		public virtual void setOlabel(int oLabel)
		{
			this.oLabel = oLabel;
		}

		public virtual State getNextState()
		{
			return this.nextState;
		}

		public virtual void setNextState(State nextState)
		{
			this.nextState = nextState;
		}

		[LineNumberTable(new byte[]
		{
			81,
			100,
			98,
			99,
			98,
			110,
			98,
			103,
			110,
			98,
			104,
			104,
			98,
			120,
			98,
			110,
			98,
			110,
			113,
			103,
			130
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
			Arc arc = (Arc)obj;
			if (this.iLabel != arc.iLabel)
			{
				return false;
			}
			if (this.nextState == null)
			{
				if (arc.nextState != null)
				{
					return false;
				}
			}
			else if (this.nextState.getId() != arc.nextState.getId())
			{
				return false;
			}
			return this.oLabel == arc.oLabel && (this.weight == arc.weight || Float.floatToIntBits(this.weight) == Float.floatToIntBits(arc.weight));
		}

		[LineNumberTable(new byte[]
		{
			107,
			159,
			4,
			108,
			234,
			61
		})]
		
		public override int hashCode()
		{
			return 31 * (this.iLabel + 31 * (this.oLabel + (31 * ((this.nextState != null) ? this.nextState.getId() : 0) + Float.floatToIntBits(this.weight))));
		}

		
		
		public override string toString()
		{
			return new StringBuilder().append("(").append(this.iLabel).append(", ").append(this.oLabel).append(", ").append(this.weight).append(", ").append(this.nextState).append(")").toString();
		}

		private float weight;

		private int iLabel;

		private int oLabel;

		private State nextState;
	}
}
