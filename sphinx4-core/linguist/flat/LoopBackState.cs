using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace edu.cmu.sphinx.linguist.flat
{
	[Serializable]
	internal sealed class LoopBackState : SentenceHMMState
	{
		internal LoopBackState(SentenceHMMState parent) : base("CIPhonesLoopBackState", parent, 0)
		{
		}

		public override int getOrder()
		{
			return 1;
		}
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		public LoopBackState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
