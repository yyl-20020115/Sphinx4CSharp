using System.Runtime.Serialization;
using System.Security.Permissions;
using java.lang;

namespace edu.cmu.sphinx.jsgf
{
	[System.Serializable]
	public class JSGFGrammarException : Exception
	{
		public JSGFGrammarException(string message) : base(message)
		{
		}
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected JSGFGrammarException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
