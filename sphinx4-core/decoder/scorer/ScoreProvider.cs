using System;
using edu.cmu.sphinx.frontend;

namespace edu.cmu.sphinx.decoder.scorer
{
	public interface ScoreProvider
	{
		float getScore(Data d);

		float[] getComponentScore(Data d);
	}
}
