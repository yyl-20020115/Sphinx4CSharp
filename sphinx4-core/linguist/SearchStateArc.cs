using System;

namespace edu.cmu.sphinx.linguist
{
	public interface SearchStateArc
	{
		SearchState getState();

		float getProbability();

		float getInsertionProbability();

		float getLanguageProbability();
	}
}
