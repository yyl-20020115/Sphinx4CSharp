using edu.cmu.sphinx.linguist.dictionary;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	[System.Serializable]
	public class WordState : SentenceHMMState
	{	
		public virtual Word getWord()
		{
			return ((AlternativeState)this.getParent()).getAlternative()[this.getWhich()];
		}
		
		public WordState(AlternativeState parent, int which) : base("W", parent, which)
		{
		}
		public override string getPrettyName()
		{
			return new StringBuilder().append(this.getName()).append('(').append(this.getWord().getSpelling()).append(')').toString();
		}

		public override string getTypeLabel()
		{
			return "Word";
		}

		public override int getOrder()
		{
			return 1;
		}

		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected WordState(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
