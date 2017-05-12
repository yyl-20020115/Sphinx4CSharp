using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[System.Serializable]
	public class SenoneSequence : java.lang.Object, Serializable.__Interface, System.Runtime.Serialization.ISerializable
	{		
		public SenoneSequence(Senone[] sequence)
		{
			this.senones = sequence;
		}

		public virtual Senone[] getSenones()
		{
			return this.senones;
		}
		
		public override int hashCode()
		{
			int num = 31;
			Senone[] array = this.senones;
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				Senone senone = array[i];
				num = num * 91 + java.lang.Object.instancehelper_hashCode(senone);
			}
			return num;
		}
		
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
				if (!java.lang.Object.instancehelper_equals(this.senones[i], senoneSequence.senones[i]))
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
		
		public static implicit operator Serializable(SenoneSequence _ref)
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
		protected SenoneSequence(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
		{
			Serialization.readObject(this, serializationInfo);
		}

		private Senone[] senones;
	}
}
