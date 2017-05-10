using System;

using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.fst.semiring
{
	[Implements(new string[]
	{
		"java.io.Serializable"
	})]
	[Serializable]
	public abstract class Semiring : java.lang.Object, Serializable.__Interface, ISerializable
	{
		public abstract float zero();

		public abstract float one();

		[LineNumberTable(new byte[]
		{
			4,
			100,
			98,
			99,
			98,
			110,
			98
		})]
		
		public override bool equals(object obj)
		{
			return this == obj || (obj != null && this.GetType() == obj.GetType());
		}

		public abstract float times(float f1, float f2);

		public abstract float plus(float f1, float f2);

		public abstract float divide(float f1, float f2);

		
		
		public virtual bool naturalLess(float w1, float w2)
		{
			return this.plus(w1, w2) == w1 && w1 != w2;
		}

		public abstract float reverse(float f);

		
		
		public Semiring()
		{
		}

		public abstract bool isMember(float f);

		
		
		public override string toString()
		{
			return Object.instancehelper_getClass(this).toString();
		}

		
		public static implicit operator Serializable(Semiring _<ref>)
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
		protected Semiring(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			Serialization.readObject(this, serializationInfo);
		}

		private const long serialVersionUID = 1L;

		protected internal const int accuracy = 5;
	}
}
