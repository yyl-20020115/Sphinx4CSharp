using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.jsgf.parser
{
	[Serializable]
	public class ParseException : Exception
	{
		[LineNumberTable(new byte[]
		{
			159,
			173,
			232,
			121,
			245,
			8
		})]
		
		public ParseException()
		{
			this.eol = java.lang.System.getProperty("line.separator", "\n");
		}

		[LineNumberTable(new byte[]
		{
			159,
			166,
			240,
			160,
			64,
			245,
			1,
			103,
			103,
			103
		})]
		
		public ParseException(Token currentTokenVal, int[][] expectedTokenSequencesVal, string[] tokenImageVal) : base(ParseException.initialise(currentTokenVal, expectedTokenSequencesVal, tokenImageVal))
		{
			this.eol = java.lang.System.getProperty("line.separator", "\n");
			this.currentToken = currentTokenVal;
			this.expectedTokenSequences = expectedTokenSequencesVal;
			this.tokenImage = tokenImageVal;
		}

		[LineNumberTable(new byte[]
		{
			159,
			190,
			112,
			102,
			98,
			106,
			103,
			133,
			107,
			54,
			168,
			108,
			140,
			242,
			54,
			233,
			76,
			103,
			104,
			107,
			127,
			2,
			105,
			123,
			133,
			127,
			12,
			125,
			127,
			5,
			125,
			233,
			54,
			235,
			76,
			127,
			40,
			127,
			4,
			101,
			159,
			16,
			159,
			14,
			126
		})]
		
		private static string initialise(Token token, int[][] array, string[] array2)
		{
			string property = java.lang.System.getProperty("line.separator", "\n");
			StringBuffer stringBuffer = new StringBuffer();
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (num < array[i].Length)
				{
					num = array[i].Length;
				}
				for (int j = 0; j < array[i].Length; j++)
				{
					stringBuffer.append(array2[array[i][j]]).append(' ');
				}
				if (array[i][array[i].Length - 1] != 0)
				{
					stringBuffer.append("...");
				}
				stringBuffer.append(property).append("    ");
			}
			string text = "Encountered \"";
			Token next = token.next;
			for (int k = 0; k < num; k++)
			{
				if (k != 0)
				{
					text = new StringBuilder().append(text).append(" ").toString();
				}
				if (next.kind == 0)
				{
					text = new StringBuilder().append(text).append(array2[0]).toString();
					break;
				}
				text = new StringBuilder().append(text).append(" ").append(array2[next.kind]).toString();
				text = new StringBuilder().append(text).append(" \"").toString();
				text = new StringBuilder().append(text).append(ParseException.add_escapes(next.image)).toString();
				text = new StringBuilder().append(text).append(" \"").toString();
				next = next.next;
			}
			text = new StringBuilder().append(text).append("\" at line ").append(token.next.beginLine).append(", column ").append(token.next.beginColumn).toString();
			text = new StringBuilder().append(text).append(".").append(property).toString();
			if (array.Length == 1)
			{
				text = new StringBuilder().append(text).append("Was expecting:").append(property).append("    ").toString();
			}
			else
			{
				text = new StringBuilder().append(text).append("Was expecting one of:").append(property).append("    ").toString();
			}
			return new StringBuilder().append(text).append(stringBuffer.toString()).toString();
		}

		[LineNumberTable(new byte[]
		{
			41,
			134,
			110,
			191,
			67,
			133,
			108,
			133,
			108,
			133,
			108,
			133,
			108,
			133,
			108,
			133,
			108,
			133,
			108,
			133,
			108,
			133,
			114,
			127,
			3,
			127,
			21,
			98,
			232,
			30,
			233,
			103
		})]
		
		internal static string add_escapes(string text)
		{
			StringBuffer stringBuffer = new StringBuffer();
			for (int i = 0; i < java.lang.String.instancehelper_length(text); i++)
			{
				char c = java.lang.String.instancehelper_charAt(text, i);
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
					else if ((num = (int)java.lang.String.instancehelper_charAt(text, i)) < 32 || num > 126)
					{
						string text2 = new StringBuilder().append("0000").append(Integer.toString(num, 16)).toString();
						stringBuffer.append(new StringBuilder().append("\\u").append(java.lang.String.instancehelper_substring(text2, java.lang.String.instancehelper_length(text2) - 4, java.lang.String.instancehelper_length(text2))).toString());
					}
					else
					{
						stringBuffer.append((char)num);
					}
				}
			}
			return stringBuffer.toString();
		}

		[LineNumberTable(new byte[]
		{
			159,
			177,
			233,
			117,
			245,
			12
		})]
		
		public ParseException(string message) : base(message)
		{
			this.eol = java.lang.System.getProperty("line.separator", "\n");
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected ParseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private const long serialVersionUID = 1L;

		public Token currentToken;

		public int[][] expectedTokenSequences;

		public string[] tokenImage;

		protected internal string eol;
	}
}
