using System;

namespace edu.cmu.sphinx.linguist.flat
{
	internal interface SentenceHMMStateVisitor
	{
		bool visit(SentenceHMMState);
	}
}
