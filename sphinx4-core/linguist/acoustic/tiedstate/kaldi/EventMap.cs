using System;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.kaldi
{
	public interface EventMap
	{
		int map(int i, int[] iarr);
	}
}
