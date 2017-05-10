using System;

using edu.cmu.sphinx.linguist.dictionary;
using IKVM.Attributes;

namespace edu.cmu.sphinx.linguist.lextree
{
	[SourceFile("HMMTree.java")]
	
	internal sealed class InitialWordNode : WordNode
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			161,
			3,
			110,
			103
		})]
		
		internal InitialWordNode(Pronunciation pronunciation, HMMNode hmmnode) : base(pronunciation, 0f)
		{
			this.parent = hmmnode;
		}

		internal HMMNode getParent()
		{
			return this.parent;
		}

		
		static InitialWordNode()
		{
			WordNode.__<clinit>();
		}

		
		internal HMMNode parent;
	}
}
