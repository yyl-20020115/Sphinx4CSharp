using System;
using edu.cmu.sphinx.frontend;
using IKVM.Attributes;
using java.io;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[Implements(new string[]
	{
		"java.io.Serializable"
	})]
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
