using edu.cmu.sphinx.linguist.dictionary;

namespace edu.cmu.sphinx.linguist.lextree
{
	internal sealed class InitialWordNode : WordNode
	{
		internal InitialWordNode(Pronunciation pronunciation, HMMNode hmmnode) : base(pronunciation, 0f)
		{
			this.parent = hmmnode;
		}

		internal HMMNode getParent()
		{
			return this.parent;
		}
		
		internal HMMNode parent;
	}
}
