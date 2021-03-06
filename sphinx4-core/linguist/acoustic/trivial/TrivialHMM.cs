﻿using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.trivial
{
	internal sealed class TrivialHMM : Object, HMM
	{		
		internal TrivialHMM(Unit unit, HMMPosition hmmposition)
		{
			this.unit = unit;
			this.position = hmmposition;
			this.hmmStates = new HMMState[4];
			this.baseUnit = unit.getBaseUnit();
			for (int i = 0; i < this.hmmStates.Length; i++)
			{
				int num = (i == this.hmmStates.Length - 1) ? 1 : 0;
				this.hmmStates[i] = new TrivialHMMState(this, i, num != 0);
			}
		}

		public Unit getUnit()
		{
			return this.unit;
		}

		public Unit getBaseUnit()
		{
			return this.baseUnit;
		}
		
		public HMMState getState(int num)
		{
			return this.hmmStates[num];
		}
		
		public int getOrder()
		{
			return this.hmmStates.Length;
		}

		public HMMPosition getPosition()
		{
			return this.position;
		}

		public HMMState getInitialState()
		{
			return this.hmmStates[0];
		}

		private const int NUM_STATES = 4;
		
		internal Unit unit;
		
		internal HMMPosition position;
		
		internal HMMState[] hmmStates;
		
		private Unit baseUnit;
	}
}
