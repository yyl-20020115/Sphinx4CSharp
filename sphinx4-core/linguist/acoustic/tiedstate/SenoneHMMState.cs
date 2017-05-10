using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.acoustic.HMMState"
	})]
	public class SenoneHMMState : java.lang.Object, HMMState
	{
		[LineNumberTable(new byte[]
		{
			159,
			186,
			104,
			103,
			103,
			122,
			104,
			103,
			147,
			119,
			118
		})]
		
		internal SenoneHMMState(SenoneHMM senoneHMM, int num)
		{
			this.hmm = senoneHMM;
			this.state = num;
			this.isEmitting = (senoneHMM.getTransitionMatrix().Length - 1 != this.state);
			if (this.isEmitting)
			{
				SenoneSequence senoneSequence = senoneHMM.getSenoneSequence();
				this.senone = senoneSequence.getSenones()[this.state];
			}
			Utilities.objectTracker("HMMState", SenoneHMMState.objectCount++);
			this.hashCode = senoneHMM.hashCode() + 37 * this.state;
		}

		public virtual HMM getHMM()
		{
			return this.hmm;
		}

		public virtual int getState()
		{
			return this.state;
		}

		
		
		public virtual float getScore(Data feature)
		{
			return this.senone.getScore(feature);
		}

		[LineNumberTable(new byte[]
		{
			45,
			108
		})]
		
		public virtual float[] calculateComponentScore(Data feature)
		{
			this.hmm.getSenoneSequence();
			return this.senone.calculateComponentScore(feature);
		}

		public virtual Senone getSenone()
		{
			return this.senone;
		}

		[LineNumberTable(new byte[]
		{
			68,
			100,
			98,
			104,
			130,
			103
		})]
		public override bool equals(object other)
		{
			if (this == other)
			{
				return true;
			}
			if (!(other is SenoneHMMState))
			{
				return false;
			}
			SenoneHMMState senoneHMMState = (SenoneHMMState)other;
			return this.hmm == senoneHMMState.hmm && this.state == senoneHMMState.state;
		}

		public override int hashCode()
		{
			return this.hashCode;
		}

		public bool isEmitting()
		{
			return this.isEmitting;
		}

		[LineNumberTable(new byte[]
		{
			108,
			107,
			102,
			140,
			103,
			113,
			156,
			232,
			60,
			230,
			71,
			156
		})]
		
		public virtual HMMStateArc[] getSuccessors()
		{
			if (this.arcs == null)
			{
				ArrayList arrayList = new ArrayList();
				float[][] transitionMatrix = this.hmm.getTransitionMatrix();
				for (int i = 0; i < transitionMatrix.Length; i++)
				{
					if (transitionMatrix[this.state][i] > -3.40282347E+38f)
					{
						HMMStateArc hmmstateArc = new HMMStateArc(this.hmm.getState(i), transitionMatrix[this.state][i]);
						arrayList.add(hmmstateArc);
					}
				}
				this.arcs = (HMMStateArc[])arrayList.toArray(new HMMStateArc[arrayList.size()]);
			}
			return this.arcs;
		}

		public virtual bool isExitState()
		{
			return !this.isEmitting;
		}

		
		
		public override string toString()
		{
			return new StringBuilder().append("HMMS ").append(this.hmm).append(" state ").append(this.state).toString();
		}

		
		
		public virtual MixtureComponent[] getMixtureComponents()
		{
			return this.senone.getMixtureComponents();
		}

		
		
		public virtual long getMixtureId()
		{
			return this.senone.getID();
		}

		
		
		public virtual float[] getLogMixtureWeights()
		{
			return this.senone.getLogMixtureWeights();
		}

		
		private SenoneHMM hmm;

		
		private int state;

		internal HMMStateArc[] arcs;

		
		private bool isEmitting;

		private Senone senone;

		
		private int hashCode;

		private static int objectCount;
	}
}
