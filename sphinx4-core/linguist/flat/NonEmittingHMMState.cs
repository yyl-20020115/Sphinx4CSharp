using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.linguist.acoustic;

namespace edu.cmu.sphinx.linguist.flat
{
	[Serializable]
	public class NonEmittingHMMState : HMMStateState
	{
		public NonEmittingHMMState(SentenceHMMState parent, HMMState hmmState) : base(parent, hmmState)
		{
		}

		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected NonEmittingHMMState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
