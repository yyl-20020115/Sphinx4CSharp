using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	internal sealed class SentenceHMMState_2 : Object, SentenceHMMStateVisitor
	{
		internal SentenceHMMState_2(SentenceHMMState sentenceHMMState)
		{
			this.this_0 = sentenceHMMState;
		}
		
		public bool visit(SentenceHMMState sentenceHMMState)
		{
			SentenceHMMState.access_000(sentenceHMMState);
			return false;
		}
		
		internal SentenceHMMState this_0;
	}
}
