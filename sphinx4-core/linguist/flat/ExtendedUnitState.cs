using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.linguist.acoustic;
using java.lang;

namespace edu.cmu.sphinx.linguist.flat
{
	[Serializable]
	public class ExtendedUnitState : UnitState
	{		
		public ExtendedUnitState(PronunciationState parent, int which, Unit unit) : base(parent, which, unit)
		{
		}
		
		public override string getFullName()
		{
			return new StringBuilder().append(this.getName()).append(" in P").append(this.getParent().getWhich()).toString();
		}
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected ExtendedUnitState(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
