using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.util;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public class SenoneHMMState : Object, HMMState
	{		
		internal SenoneHMMState(SenoneHMM senoneHMM, int num)
		{
			this.hmm = senoneHMM;
			this.state = num;
			this._isEmitting = (senoneHMM.getTransitionMatrix().Length - 1 != this.state);
			if (this._isEmitting)
			{
				SenoneSequence senoneSequence = senoneHMM.getSenoneSequence();
				this.senone = senoneSequence.getSenones()[this.state];
			}
			Utilities.objectTracker("HMMState", SenoneHMMState.objectCount++);
			this._hashCode = senoneHMM.hashCode() + 37 * this.state;
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
		
		public virtual float[] calculateComponentScore(Data feature)
		{
			this.hmm.getSenoneSequence();
			return this.senone.calculateComponentScore(feature);
		}

		public virtual Senone getSenone()
		{
			return this.senone;
		}

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
			return this._hashCode;
		}

		public bool isEmitting()
		{
			return this._isEmitting;
		}
		
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
			return !this._isEmitting;
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

		
		private bool _isEmitting;

		private Senone senone;
		
		private int _hashCode;

		private static int objectCount;
	}
}
