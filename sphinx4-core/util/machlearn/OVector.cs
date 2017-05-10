using System;

using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.util.machlearn
{
	[Implements(new string[]
	{
		"java.lang.Cloneable",
		"java.io.Serializable"
	})]
	[Serializable]
	public class OVector : java.lang.Object, Cloneable.__Interface, Serializable.__Interface, ISerializable
	{
		public virtual double[] getValues()
		{
			return this.__values;
		}

		public OVector(double[] values)
		{
			this.__values = values;
		}

		public OVector(double value) : this(new double[]
		{
			value
		})
		{
		}

		
		
		public virtual int dimension()
		{
			return this.getValues().Length;
		}

		
		
		public override bool equals(object obj)
		{
			return obj is OVector && Arrays.equals(this.__values, ((OVector)obj).__values);
		}

		
		
		public override int hashCode()
		{
			return Arrays.hashCode(this.__values);
		}

		
		
		public override string toString()
		{
			return Arrays.toString(this.__values);
		}

		
		public static implicit operator Cloneable(OVector _ref)
		{
			Cloneable result = 


			result.__<ref> = _ref;
			return result;
		}

		
		public static implicit operator Serializable(OVector _<ref>)
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
		protected OVector(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			Serialization.readObject(this, serializationInfo);
		}

		
		protected internal double[] values
		{
			
			get
			{
				return this.__values;
			}
			
			private set
			{
				this.__values = value;
			}
		}

		internal double[] __values;
	}
}
