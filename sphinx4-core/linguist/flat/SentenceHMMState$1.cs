namespace edu.cmu.sphinx.linguist.flat
{
	internal sealed class SentenceHMMState_1 : java.lang.Object, SentenceHMMStateVisitor
	{
		internal SentenceHMMState_1(SentenceHMMState sentenceHMMState)
		{
			this.this_0 = sentenceHMMState;
		}
		
		public bool visit(SentenceHMMState sentenceHMMState)
		{
			sentenceHMMState.setProcessed(false);
			return false;
		}

		internal SentenceHMMState this_0;
	}
}
