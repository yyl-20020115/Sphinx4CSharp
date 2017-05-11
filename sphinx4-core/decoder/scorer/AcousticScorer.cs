using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.decoder.scorer
{
	public interface AcousticScorer : Configurable
	{
		void allocate();

		void deallocate();

		void startRecognition();

		void stopRecognition();

		Data calculateScores(List l);
		
		Data calculateScoresAndStoreData(List l);
	}
}
