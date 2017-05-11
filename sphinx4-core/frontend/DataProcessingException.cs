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
		public DataProcessingException(string message, System.Exception cause) : base(message, cause)
		{
		}
		
		public DataProcessingException()
		{
		}
		
		public DataProcessingException(string message) : base(message)
		{
		}
		
		public DataProcessingException(System.Exception cause) : base(cause)
		{
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected DataProcessingException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
