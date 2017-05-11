using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.trivial
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.linguist.acoustic.HMMState"
	})]
	.
	
	internal sealed class TrivialHMMState : java.lang.Object, HMMState
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			69,
			130,
			104,
			103,
			103,
			103
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			238,
			104,
			134,
			103,
			110,
			127,
			1
		})]
		
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

		
		static TrivialHMMState()
		{
		}

		
		private static HMMStateArc[] EMPTY_ARC = new HMMStateArc[0];

		
		private HMM hmm;

		
		private int which;

		
		private bool isFinal;
	}
}
