using System;

using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic
{
	[Implements(new string[]
	{
		"java.io.Serializable"
	})]
	[Serializable]
	public class Context : java.lang.Object, Serializable.__Interface, ISerializable
	{
		
		public static void __<clinit>()
		{
		}

		public override string toString()
		{
			return "";
		}

		[LineNumberTable(new byte[]
		{
			159,
			168,
			102
		})]
		
		protected internal Context()
		{
		}

		public virtual bool isPartialMatch(Context context)
		{
			return true;
		}

		[LineNumberTable(new byte[]
		{
			7,
			100,
			98,
			104,
			103,
			146
		})]
		
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

		
		static Context()
		{
		}

		
		public static implicit operator Serializable(Context _<ref>)
		{
			Serializable result;
			result.__<ref> = _<ref>;
			return result;
		}

		[SecurityCritical]
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			Serialization.writeObject(this, serializationInfo);
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected Context(SerializationInfo serializationInfo, StreamingContext streamingContext)
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
