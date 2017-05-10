using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.jsgf
{
	[Serializable]
	public class JSGFGrammarParseException : Exception
	{
		[LineNumberTable(new byte[]
		{
			159,
			160,
			104,
			103,
			103,
			103,
			104
		})]
		
		public JSGFGrammarParseException(int lineNumber, int charNumber, string message, string details)
		{
			this.lineNumber = lineNumber;
			this.charNumber = charNumber;
			this.message = message;
			this.details = details;
		}

		[LineNumberTable(new byte[]
		{
			159,
			166,
			104,
			103
		})]
		
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
