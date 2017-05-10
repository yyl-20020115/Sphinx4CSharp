using System;

using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[Implements(new string[]
	{
		"java.io.Serializable"
	})]
	[Serializable]
	public class SenoneSequence : java.lang.Object, Serializable.__Interface, ISerializable
	{
		[LineNumberTable(new byte[]
		{
			159,
			183,
			104,
			103
		})]
		
		public SenoneSequence(Senone[] sequence)
		{
			this.senones = sequence;
		}

		public virtual Senone[] getSenones()
		{
			return this.senones;
		}

		[LineNumberTable(new byte[]
		{
			13,
			99,
			117,
			45,
			166
		})]
		
		public override int hashCode()
		{
			int num = 31;
			Senone[] array = this.senones;
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				Senone senone = array[i];
				num = num * 91 + Object.instancehelper_hashCode(senone);
			}
			return num;
		}

		[LineNumberTable(new byte[]
		{
			28,
			100,
			130,
			104,
			103,
			112,
			130,
			108,
			119,
			2,
			230,
			69,
			162
		})]
		
		public override bool equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is SenoneSequence))
			{
				return false;
			}
			SenoneSequence senoneSequence = (SenoneSequence)o;
			if (this.senones.Length != senoneSequence.senones.Length)
			{
				return false;
			}
			for (int i = 0; i < this.senones.Length; i++)
			{
				if (!Object.instancehelper_equals(this.senones[i], senoneSequence.senones[i]))
				{
					return false;
				}
			}
			return true;
		}

		
		
		
		public static SenoneSequence create(List senoneList)
		{
			return new SenoneSequence((Senone[])senoneList.toArray(new Senone[senoneList.size()]));
		}

		[LineNumberTable(new byte[]
		{
			55,
			127,
			12,
			116,
			43,
			166
		})]
		
		public virtual void dump(string msg)
		{
			java.lang.System.@out.println(new StringBuilder().append(" SenoneSequence ").append(msg).append(':').toString());
			Senone[] array = this.senones;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Senone senone = array[i];
				senone.dump("  seq:");
			}
		}

		
		public static implicit operator Serializable(SenoneSequence _<ref>)
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
		protected SenoneSequence(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			Serialization.readObject(this, serializationInfo);
		}

		
		private Senone[] senones;
	}
}
