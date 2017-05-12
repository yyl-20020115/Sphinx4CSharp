using edu.cmu.sphinx.linguist.language.grammar;

namespace edu.cmu.sphinx.linguist.flat
{
	[System.Serializable]
	public class GrammarState : SentenceHMMState
	{
		public virtual GrammarNode getGrammarNode()
		{
			return this.grammarNode;
		}
		
		public GrammarState(GrammarNode node) : base("G", null, node.getID())
		{
			this.grammarNode = node;
			this.setFinalState(this.grammarNode.isFinalNode());
		}

		public override string getTypeLabel()
		{
			return "Gram";
		}

		public override int getOrder()
		{
			return 3;
		}
		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected GrammarState(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
		
		private GrammarNode grammarNode;
	}
}
