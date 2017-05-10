using System;

using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.linguist.acoustic;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.result
{
	public abstract class FrameStatistics : java.lang.Object
	{
		
		
		public FrameStatistics()
		{
		}

		public abstract int getFrameNumber();

		public abstract Data getData();

		public abstract float getBestScore();

		public abstract Unit getBestUnit();

		public abstract int getBestState();
	}
}
