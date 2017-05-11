using System.Runtime.Serialization;
using System.Security.Permissions;
using java.lang;

namespace edu.cmu.sphinx.jsgf
{
	[System.Serializable]
	public class JSGFGrammarParseException : Exception
	{		
		public JSGFGrammarParseException(int lineNumber, int charNumber, string message, string details)
		{
			this.lineNumber = lineNumber;
			this.charNumber = charNumber;
			this.message = message;
			this.details = details;
		}
		
		public JSGFGrammarParseException(string message)
		{
			this.message = message;
		}
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected JSGFGrammarParseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		public int lineNumber;

		public int charNumber;

		public string message;

		public string details;
	}
}
