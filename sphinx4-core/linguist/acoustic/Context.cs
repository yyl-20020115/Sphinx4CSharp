using ikvm.@internal;
using java.io;

namespace edu.cmu.sphinx.linguist.acoustic
{
	[System.Serializable]
	public class Context : java.lang.Object, Serializable.__Interface, System.Runtime.Serialization.ISerializable
	{
		public override string toString()
		{
			return "";
		}
		
		protected internal Context()
		{
		}

		public virtual bool isPartialMatch(Context context)
		{
			return true;
		}
		
		public override bool equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (o is Context)
			{
				Context context = (Context)o;
				return java.lang.String.instancehelper_equals(this.toString(), context.toString());
			}
			return false;
		}
		
		public override int hashCode()
		{
			return java.lang.String.instancehelper_hashCode(this.toString());
		}
		
		public static implicit operator Serializable(Context _ref)
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
		protected Context(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
		{
			Serialization.readObject(this, serializationInfo);
		}
		
		public static Context EMPTY_CONTEXT
		{
			
			get
			{
				return Context.__EMPTY_CONTEXT;
			}
		}

		internal static Context __EMPTY_CONTEXT = new Context();
	}
}
