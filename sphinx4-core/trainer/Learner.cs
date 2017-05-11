using edu.cmu.sphinx.linguist.acoustic.tiedstate.trainer;
using edu.cmu.sphinx.util.props;

namespace edu.cmu.sphinx.trainer
{
	public interface Learner : Configurable
	{
		void start();

		void stop();

		void setUtterance(Utterance u);

		void initializeComputation(Utterance u, UtteranceGraph ug);

		void setGraph(UtteranceGraph ug);

		TrainerScore[] getScore();
	}
}
