using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	[System.Serializable]
	public class BranchState : SentenceHMMState
	{
		public BranchState(string leftContext, string rightContext, int nodeID) : base(new StringBuilder().append("B[").append(leftContext).append(",").append(rightContext).append("]").toString(), null, nodeID)
		{
		}

		public override string getTypeLabel()
		{
			return "Brnch";
		}

		public override int getOrder()
		{
			return 2;
		}
		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected BranchState(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
