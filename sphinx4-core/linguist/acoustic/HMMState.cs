using System;
using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic.tiedstate;

namespace edu.cmu.sphinx.linguist.acoustic
{
	public interface HMMState
	{
		long getMixtureId();

		HMM getHMM();

		MixtureComponent[] getMixtureComponents();

		float[] getLogMixtureWeights();

		int getState();

		float getScore(Data d);

		float[] calculateComponentScore(Data d);

		bool isEmitting();

		HMMStateArc[] getSuccessors();

		bool isExitState();
	}
}
