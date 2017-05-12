using edu.cmu.sphinx.linguist.dictionary;
using java.io;

namespace edu.cmu.sphinx.linguist.flat
{
	[System.Serializable]
	public class AlternativeState : SentenceHMMState, Serializable.__Interface
	{		
		public AlternativeState(GrammarState parent, int which) : base("A", parent, which)
		{
		}
		
		public virtual Word[] getAlternative()
		{
			return ((GrammarState)this.getParent()).getGrammarNode().getWords(this.getWhich());
		}

		public override string getTypeLabel()
		{
			return "Alt";
		}

		public override int getOrder()
		{
			return 1;
		}
				
		public static implicit operator Serializable(AlternativeState _ref)
		{
			Serializable result = Serializable.Cast(_ref);
			return result;
		}

		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected AlternativeState(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
