using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.fst.semiring
{
	[Serializable]
	public class TropicalSemiring : Semiring
	{
		
		public static void __<clinit>()
		{
		}

		
		
		public TropicalSemiring()
		{
		}

		
		
		public override bool isMember(float w)
		{
			return !Float.isNaN(w) && w != float.NegativeInfinity;
		}

		[LineNumberTable(new byte[]
		{
			159,
			183,
			116,
			166
		})]
		
		public override float plus(float w1, float w2)
		{
			if (!this.isMember(w1) || !this.isMember(w2))
			{
				return float.NegativeInfinity;
			}
			return (w1 >= w2) ? w2 : w1;
		}

		[LineNumberTable(new byte[]
		{
			7,
			116,
			166
		})]
		
		public override float times(float w1, float w2)
		{
			if (!this.isMember(w1) || !this.isMember(w2))
			{
				return float.NegativeInfinity;
			}
			return w1 + w2;
		}

		[LineNumberTable(new byte[]
		{
			23,
			116,
			166,
			105,
			102,
			105,
			166
		})]
		
		public override float divide(float w1, float w2)
		{
			if (!this.isMember(w1) || !this.isMember(w2))
			{
				return float.NegativeInfinity;
			}
			if (w2 == TropicalSemiring.zero)
			{
				return float.NegativeInfinity;
			}
			if (w1 == TropicalSemiring.zero)
			{
				return TropicalSemiring.zero;
			}
			return w1 - w2;
		}

		public override float zero()
		{
			return TropicalSemiring.zero;
		}

		public override float one()
		{
			return TropicalSemiring.one;
		}

		public override float reverse(float w1)
		{
			return w1;
		}

		static TropicalSemiring()
		{
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected TropicalSemiring(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private const long serialVersionUID = 2711172386738607866L;

		private new static float zero = float.PositiveInfinity;

		private new static float one = 0f;
	}
}
