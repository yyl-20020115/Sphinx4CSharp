using System;
using edu.cmu.sphinx.decoder.adaptation;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.util.props.Configurable"
	})]
	public interface Loader : Configurable
	{
		void update(Transform t, ClusteredDensityFileData cdfd);

		
		Pool getMeansPool();

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
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
