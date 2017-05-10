using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using IKVM.Attributes;

namespace edu.cmu.sphinx.util.props
{
	[Serializable]
	public class InternalConfigurationException : PropertyException
	{
		[LineNumberTable(new byte[]
		{
			159,
			163,
			105
		})]
		
		internal InternalConfigurationException(string instanceName, string propertyName, string msg) : base(instanceName, propertyName, msg)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			168,
			107
		})]
		
		internal InternalConfigurationException(Exception cause, string instanceName, string propertyName, string msg) : base(cause, instanceName, propertyName, msg)
		{
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected InternalConfigurationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
