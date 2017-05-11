using edu.cmu.sphinx.decoder.adaptation;
using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public interface Loader : Configurable
	{
		void update(Transform t, ClusteredDensityFileData cdfd);

		
		Pool getMeansPool();

		void load();

		Properties getProperties();

		float[][] getTransformMatrix();

		Pool getMeansTransformationMatrixPool();

		Pool getMeansTransformationVectorPool();

		Pool getVariancePool();

		Pool getVarianceTransformationMatrixPool();

		Pool getVarianceTransformationVectorPool();

		GaussianWeights getMixtureWeights();
		
		Pool getTransitionMatrixPool();
		
		Pool getSenonePool();

		HMMManager getHMMManager();

		Map getContextIndependentUnits();

		void logInfo();

		int getLeftContextSize();

		int getRightContextSize();
	}
}
