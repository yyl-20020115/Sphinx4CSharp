using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.trivial
{
	internal sealed class TrivialHMMState : Object, HMMState
	{
		internal TrivialHMMState(HMM hmm, int num, bool flag)
		{
			this.hmm = hmm;
			this.which = num;
			this.isFinal = flag;
		}

		public HMM getHMM()
		{
			return this.hmm;
		}

		public int getState()
		{
			return this.which;
		}

		public float getScore(Data data)
		{
			return 0f;
		}
		
		public float[] calculateComponentScore(Data data)
		{
			return new float[((FloatData)data).getValues().Length];
		}

		public bool isEmitting()
		{
			return !this.isFinal;
		}
		
		public HMMStateArc[] getSuccessors()
		{
			if (this.isFinal)
			{
				return TrivialHMMState.EMPTY_ARC;
			}
			return new HMMStateArc[]
			{
				new HMMStateArc(this, 0f),
				new HMMStateArc(this.hmm.getState(this.which + 1), 0f)
			};
		}

		public bool isExitState()
		{
			return this.isFinal;
		}

		public MixtureComponent[] getMixtureComponents()
		{
			return null;
		}

		public long getMixtureId()
		{
			return 0L;
		}

		public float[] getLogMixtureWeights()
		{
			return null;
		}

		private static HMMStateArc[] EMPTY_ARC = new HMMStateArc[0];

		private HMM hmm;

		private int which;

		private bool isFinal;
	}
}
