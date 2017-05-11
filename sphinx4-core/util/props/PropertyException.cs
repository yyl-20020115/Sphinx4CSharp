using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.util.props
{
	[Serializable]
	public class PropertyException : RuntimeException
	{
		[LineNumberTable(new byte[]
		{
			159,
			174,
			106
		})]
		
		public PropertyException(string instanceName, string propertyName, string msg) : this(null, instanceName, propertyName, msg)
		{
		}

		[LineNumberTable(new byte[]
		{
			4,
			103
		})]
		
		public PropertyException(java.lang.Exception e) : base(e)
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			188,
			139,
			103,
			103
		})]
		
		public PropertyException(Exception cause, string instanceName, string propertyName, string msg) : base(msg, cause)
		{
			this.instanceName = instanceName;
			this.propertyName = propertyName;
		}

		public virtual string getProperty()
		{
			return this.propertyName;
		}

		[LineNumberTable(new byte[]
		{
			25,
			127,
			45,
			47
		})]
		
		public override string toString()
		{
			return new StringBuilder().append("Property exception component:'").append(this.instanceName).append("' property:'").append(this.propertyName).append("' - ").append(Throwable.instancehelper_getMessage(this)).append('\n').append(base.toString()).toString();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected PropertyException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private string instanceName;

		private string propertyName;
	}
}
