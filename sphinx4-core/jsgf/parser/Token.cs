using ikvm.@internal;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.jsgf.parser
{
	[System.Serializable]
	public class Token : Object, Serializable.__Interface, System.Runtime.Serialization.ISerializable
	{		
		public Token()
		{
		}
		
		public static Token newToken(int ofKind, string image)
		{
			return new Token(ofKind, image);
		}
		
		public Token(int kind, string image)
		{
			this.kind = kind;
			this.image = image;
		}

		public virtual object getValue()
		{
			return null;
		}
		
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
			Serializable result = Serializable.Cast(_ref);
			return result;
		}

		[System.Security.SecurityCritical]		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
		{
			Serialization.writeObject(this, serializationInfo);
		}
		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected Token(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
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
