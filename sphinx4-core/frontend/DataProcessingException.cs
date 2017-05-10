using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.frontend
{
	[Serializable]
	public class DataProcessingException : RuntimeException
	{
		[LineNumberTable(new byte[]
		{
			159,
			181,
			104
		})]
		
		public DataProcessingException(string message, Exception cause) : base(message, cause)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			162,
			102
		})]
		
		public DataProcessingException()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			171,
			103
		})]
		
		public DataProcessingException(string message) : base(message)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			190,
			103
		})]
		
		public DataProcessingException(Exception cause) : base(cause)
		{
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected DataProcessingException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
