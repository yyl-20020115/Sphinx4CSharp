using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.linguist.language.grammar;
using IKVM.Attributes;

namespace edu.cmu.sphinx.linguist.flat
{
	[Serializable]
	public class GrammarState : SentenceHMMState
	{
		
		public new static void __<clinit>()
		{
		}

		public virtual GrammarNode getGrammarNode()
		{
			return this.grammarNode;
		}

		[LineNumberTable(new byte[]
		{
			159,
			173,
			116,
			103,
			113
		})]
		
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

		
		static GrammarState()
		{
			SentenceHMMState.__<clinit>();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected GrammarState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		
		private GrammarNode grammarNode;
	}
}
