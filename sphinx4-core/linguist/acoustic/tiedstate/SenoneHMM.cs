﻿using edu.cmu.sphinx.util;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public class SenoneHMM : Object, HMM
	{		
		public SenoneHMM(Unit unit, SenoneSequence senoneSequence, float[][] transitionMatrix, HMMPosition position)
		{
			this.unit = unit;
			this.senoneSequence = senoneSequence;
			this.transitionMatrix = transitionMatrix;
			this.position = position;
			Utilities.objectTracker("HMM", SenoneHMM.objectCount++);
			this.hmmStates = new HMMState[transitionMatrix.Length];
			for (int i = 0; i < this.hmmStates.Length; i++)
			{
				this.hmmStates[i] = new SenoneHMMState(this, i);
			}
			this.baseUnit = unit.getBaseUnit();
		}

		public virtual SenoneSequence getSenoneSequence()
		{
			return this.senoneSequence;
		}
		
		public virtual HMMState getState(int which)
		{
			return this.hmmStates[which];
		}
		
		public virtual bool isComposite()
		{
			Senone[] senones = this.getSenoneSequence().getSenones();
			Senone[] array = senones;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Senone senone = array[i];
				if (senone is CompositeSenone)
				{
					return true;
				}
			}
			return false;
		}

		public virtual Unit getUnit()
		{
			return this.unit;
		}

		public virtual Unit getBaseUnit()
		{
			return this.baseUnit;
		}
		
		public virtual int getOrder()
		{
			return this.getSenoneSequence().getSenones().Length;
		}

		public virtual float[][] getTransitionMatrix()
		{
			return this.transitionMatrix;
		}
		
		public virtual float getTransitionProbability(int stateFrom, int stateTo)
		{
			return this.transitionMatrix[stateFrom][stateTo];
		}

		public virtual HMMPosition getPosition()
		{
			return this.position;
		}		
		
		public virtual bool isFiller()
		{
			return this.unit.isFiller();
		}
		
		public virtual bool isContextDependent()
		{
			return this.unit.isContextDependent();
		}		
		
		public virtual HMMState getInitialState()
		{
			return this.getState(0);
		}
		
		public override string toString()
		{
			string text = (!this.isComposite()) ? "HMM" : "HMM@";
			return new StringBuilder().append(text).append('(').append(this.unit).append("):").append(this.position).toString();
		}
		
		public override int hashCode()
		{
			return this.getSenoneSequence().hashCode();
		}

		public override bool equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (o is SenoneHMM)
			{
				SenoneHMM senoneHMM = (SenoneHMM)o;
				return this.getSenoneSequence().equals(senoneHMM.getSenoneSequence());
			}
			return false;
		}
		
		private Unit unit;
		
		private Unit baseUnit;
		
		private SenoneSequence senoneSequence;
		
		private float[][] transitionMatrix;
		
		private HMMPosition position;

		private static int objectCount;
		
		private HMMState[] hmmStates;
	}
}
