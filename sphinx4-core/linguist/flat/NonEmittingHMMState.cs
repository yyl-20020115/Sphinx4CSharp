using edu.cmu.sphinx.linguist.acoustic;

namespace edu.cmu.sphinx.linguist.flat
{
	[System.Serializable]
	public class NonEmittingHMMState : HMMStateState
	{
		public NonEmittingHMMState(SentenceHMMState parent, HMMState hmmState) : base(parent, hmmState)
		{
		}

		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected NonEmittingHMMState(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
