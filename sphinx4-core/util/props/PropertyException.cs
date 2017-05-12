using java.lang;

namespace edu.cmu.sphinx.util.props
{
	[System.Serializable]
	public class PropertyException : RuntimeException
	{
		public PropertyException(string instanceName, string propertyName, string msg) : this(null, instanceName, propertyName, msg)
		{
		}
	
		public PropertyException(Exception e) : base(e)
		{
		}
	
		public PropertyException(System.Exception cause, string instanceName, string propertyName, string msg) : base(msg, cause)
		{
			this.instanceName = instanceName;
			this.propertyName = propertyName;
		}

		public virtual string getProperty()
		{
			return this.propertyName;
		}
	
		public override string toString()
		{
			return new StringBuilder().append("Property exception component:'").append(this.instanceName).append("' property:'").append(this.propertyName).append("' - ").append(Throwable.instancehelper_getMessage(this)).append('\n').append(base.toString()).toString();
		}
		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected PropertyException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private string instanceName;

		private string propertyName;
	}
}
