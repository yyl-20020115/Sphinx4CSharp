using System;
using edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;

namespace edu.cmu.sphinx.trainer
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public interface Learner : Configurable
	{
		void start();

		void stop();

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		void setUtterance(Utterance u);

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		void initializeComputation(Utterance u, UtteranceGraph ug);

		void setGraph(UtteranceGraph ug);

		TrainerScore[] getScore();
	}
}
