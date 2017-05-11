using edu.cmu.sphinx.util.props;
using java.util;

namespace edu.cmu.sphinx.decoder.scorer
{
	public interface ScoreNormalizer : Configurable
	{
		Scoreable normalize(List l, Scoreable s);
	}
}
