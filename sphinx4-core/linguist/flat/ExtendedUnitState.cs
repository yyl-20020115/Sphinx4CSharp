using edu.cmu.sphinx.linguist.acoustic;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	[System.Serializable]
	public class ExtendedUnitState : UnitState
	{		
		public ExtendedUnitState(PronunciationState parent, int which, Unit unit) : base(parent, which, unit)
		{
		}
		
		public override string getFullName()
		{
			return new StringBuilder().append(this.getName()).append(" in P").append(this.getParent().getWhich()).toString();
		}
		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected ExtendedUnitState(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
