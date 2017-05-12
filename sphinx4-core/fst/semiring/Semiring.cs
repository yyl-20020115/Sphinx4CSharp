using ikvm.@internal;
using java.io;

namespace edu.cmu.sphinx.fst.semiring
{
	[System.Serializable]
	public abstract class Semiring : java.lang.Object, Serializable.__Interface, System.Runtime.Serialization.ISerializable
	{
		public abstract float zero();

		public abstract float one();

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
			return java.lang.Object.instancehelper_getClass(this).toString();
		}
		
		public static implicit operator Serializable(Semiring _ref)
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
		protected Semiring(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
		{
			Serialization.readObject(this, serializationInfo);
		}

		private const long serialVersionUID = 1L;

		protected internal const int accuracy = 5;
	}
}
