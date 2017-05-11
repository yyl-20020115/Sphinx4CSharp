using System.Runtime.Serialization;
using System.Security.Permissions;
using System;

namespace edu.cmu.sphinx.util.props
{
	[Serializable]
	public class InternalConfigurationException : PropertyException
	{		
		internal InternalConfigurationException(string instanceName, string propertyName, string msg) : base(instanceName, propertyName, msg)
		{
		}
		
		internal InternalConfigurationException(Exception cause, string instanceName, string propertyName, string msg) : base(cause, instanceName, propertyName, msg)
		{
		}
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected InternalConfigurationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}
	}
}
