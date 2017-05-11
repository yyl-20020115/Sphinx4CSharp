using edu.cmu.sphinx.frontend;
using java.io;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public interface Senone : Serializable.__Interface
	{
		float getScore(Data d);

		float[] calculateComponentScore(Data d);

		long getID();

		void dump(string str);

		MixtureComponent[] getMixtureComponents();

		float[] getLogMixtureWeights();
	}
}
