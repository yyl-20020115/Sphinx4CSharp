using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using java.lang;

namespace edu.cmu.sphinx.jsgf.parser
{
	[Serializable]
	public class TokenMgrError : Error
	{		
		public TokenMgrError(string message, int reason) : base(message)
		{
			this.errorCode = reason;
		}
	
		public TokenMgrError(bool EOFSeen, int lexState, int errorLine, int errorColumn, string errorAfter, char curChar, int reason) : this(TokenMgrError.LexicalError(EOFSeen, lexState, errorLine, errorColumn, errorAfter, curChar), reason)
		{
		}
		
		protected internal static string addEscapes(string str)
		{
			StringBuffer stringBuffer = new StringBuffer();
			for (int i = 0; i < java.lang.String.instancehelper_length(str); i++)
			{
				char c = java.lang.String.instancehelper_charAt(str, i);
				if (c != '\0')
				{
					int num;
					if (c == '\b')
					{
						stringBuffer.append("\\b");
					}
					else if (c == '\t')
					{
						stringBuffer.append("\\t");
					}
					else if (c == '\n')
					{
						stringBuffer.append("\\n");
					}
					else if (c == '\f')
					{
						stringBuffer.append("\\f");
					}
					else if (c == '\r')
					{
						stringBuffer.append("\\r");
					}
					else if (c == '"')
					{
						stringBuffer.append("\\\"");
					}
					else if (c == '\'')
					{
						stringBuffer.append("\\'");
					}
					else if (c == '\\')
					{
						stringBuffer.append("\\\\");
					}
					else if ((num = (int)java.lang.String.instancehelper_charAt(str, i)) < 32 || num > 126)
					{
						string text = new StringBuilder().append("0000").append(Integer.toString(num, 16)).toString();
						stringBuffer.append(new StringBuilder().append("\\u").append(java.lang.String.instancehelper_substring(text, java.lang.String.instancehelper_length(text) - 4, java.lang.String.instancehelper_length(text))).toString());
					}
					else
					{
						stringBuffer.append((char)num);
					}
				}
			}
			return stringBuffer.toString();
		}
		
		protected internal static string LexicalError(bool EOFSeen, int lexState, int errorLine, int errorColumn, string errorAfter, char curChar)
		{
			return new StringBuilder().append("Lexical error at line ").append(errorLine).append(", column ").append(errorColumn).append(".  Encountered: ").append((!EOFSeen) ? new StringBuilder().append("\"").append(TokenMgrError.addEscapes(java.lang.String.valueOf(curChar))).append("\"").append(" (").append((int)curChar).append("), ").toString() : "<EOF> ").append("after : \"").append(TokenMgrError.addEscapes(errorAfter)).append("\"").toString();
		}
		
		public override string getMessage()
		{
			return base.getMessage();
		}
		
		public TokenMgrError()
		{
		}

		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected TokenMgrError(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private const long serialVersionUID = 1L;

		internal const int LEXICAL_ERROR = 0;

		internal const int STATIC_LEXER_ERROR = 1;

		internal const int INVALID_LEXICAL_STATE = 2;

		internal const int LOOP_DETECTED = 3;

		internal int errorCode;
	}
}
