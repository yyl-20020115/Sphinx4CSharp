using System;

using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.jsgf.parser
{
	[Implements(new string[]
	{
		"java.io.Serializable"
	})]
	[Serializable]
	public class Token : java.lang.Object, Serializable.__Interface, ISerializable
	{
		
		
		public Token()
		{
		}

		[LineNumberTable(new byte[]
		{
			46,
			132
		})]
		
		public static Token newToken(int ofKind, string image)
		{
			return new Token(ofKind, image);
		}

		[LineNumberTable(new byte[]
		{
			34,
			104,
			103,
			103
		})]
		
		public Token(int kind, string image)
		{
			this.kind = kind;
			this.image = image;
		}

		public virtual object getValue()
		{
			return null;
		}

		[LineNumberTable(new byte[]
		{
			30,
			104
		})]
		
		public Token(int kind) : this(kind, null)
		{
		}

		public override string toString()
		{
			return this.image;
		}

		
		
		public static Token newToken(int ofKind)
		{
			return Token.newToken(ofKind, null);
		}

		
		public static implicit operator Serializable(Token _ref)
		{
			Serializable result;
			result.__ref = _ref;
			return result;
		}

		[SecurityCritical]
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			Serialization.writeObject(this, serializationInfo);
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected Token(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			Serialization.readObject(this, serializationInfo);
		}

		private const long serialVersionUID = 1L;

		public int kind;

		public int beginLine;

		public int beginColumn;

		public int endLine;

		public int endColumn;

		public string image;

		public Token next;

		public Token specialToken;
	}
}
