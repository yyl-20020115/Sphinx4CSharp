using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.frontend;
using ikvm.@internal;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[Serializable]
	public class CompositeSenone : ScoreCachingSenone
	{
		public CompositeSenone(Senone[] senones, float weight)
		{
			this.senones = senones;
			this.weight = weight;
			java.lang.System.@out.print(new StringBuilder().append(" ").append(senones.Length).toString());
		}
	
		public override long getID()
		{
			long num = 1L;
			long num2 = 0L;
			Senone[] array = this.senones;
			int num3 = array.Length;
			for (int i = 0; i < num3; i++)
			{
				Senone senone = array[i];
				num2 += senone.getID() * num;
				num *= (long)((ulong)20000);
			}
			return num2;
		}
	
		public static CompositeSenone create(Collection senoneCollection, float weight)
		{
			return new CompositeSenone((Senone[])senoneCollection.toArray(new Senone[senoneCollection.size()]), weight);
		}
	
		public override void dump(string msg)
		{
			java.lang.System.@out.println(new StringBuilder().append("   CompositeSenone ").append(msg).append(": ").toString());
			Senone[] array = this.senones;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Senone senone = array[i];
				senone.dump("   ");
			}
		}
	
		protected internal override float calculateScore(Data feature)
		{
			float num = float.MinValue;
			Senone[] array = this.senones;
			int num2 = array.Length;
			for (int i = 0; i < num2; i++)
			{
				Senone senone = array[i];
				num = java.lang.Math.max(num, senone.getScore(feature));
			}
			return num + this.weight;
		}
		
		public override float[] calculateComponentScore(Data feature)
		{
			if (!CompositeSenone.assertionsDisabled)
			{
				object obj = "Not implemented!";
				
				throw new AssertionError(obj);
			}
			return null;
		}
		public virtual Senone[] getSenones()
		{
			return this.senones;
		}
		
		public override bool equals(object o)
		{
			if (!(o is Senone))
			{
				return false;
			}
			Senone senone = (Senone)o;
			return this.getID() == senone.getID();
		}

		public override int hashCode()
		{
			long id = this.getID();
			int num = (int)(id >> 32);
			int num2 = (int)id;
			return num + num2;
		}		
		
		public override string toString()
		{
			return new StringBuilder().append("senone id: ").append(this.getID()).toString();
		}

		public override MixtureComponent[] getMixtureComponents()
		{
			return null;
		}

		public override float[] getLogMixtureWeights()
		{
			return null;
		}
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected CompositeSenone(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private const int MAX_SENONES = 20000;

		private const bool wantMaxScore = true;
		
		private Senone[] senones;

		private float weight;

		internal static bool assertionsDisabled = !ClassLiteral<CompositeSenone>.Value.desiredAssertionStatus();
	}
}
