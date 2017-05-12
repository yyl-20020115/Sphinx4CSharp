using java.lang;

namespace edu.cmu.sphinx.fst.semiring
{
	[System.Serializable]
	public class TropicalSemiring : Semiring
	{
		public TropicalSemiring()
		{
		}

		public override bool isMember(float w)
		{
			return !Float.isNaN(w) && w != float.NegativeInfinity;
		}

		public override float plus(float w1, float w2)
		{
			if (!this.isMember(w1) || !this.isMember(w2))
			{
				return float.NegativeInfinity;
			}
			return (w1 >= w2) ? w2 : w1;
		}

		public override float times(float w1, float w2)
		{
			if (!this.isMember(w1) || !this.isMember(w2))
			{
				return float.NegativeInfinity;
			}
			return w1 + w2;
		}

		public override float divide(float w1, float w2)
		{
			if (!this.isMember(w1) || !this.isMember(w2))
			{
				return float.NegativeInfinity;
			}
			if (w2 == TropicalSemiring._zero)
			{
				return float.NegativeInfinity;
			}
			if (w1 == TropicalSemiring._zero)
			{
				return TropicalSemiring._zero;
			}
			return w1 - w2;
		}

		public override float zero()
		{
			return TropicalSemiring._zero;
		}

		public override float one()
		{
			return TropicalSemiring._one;
		}

		public override float reverse(float w1)
		{
			return w1;
		}

		static TropicalSemiring()
		{
		}

		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected TropicalSemiring(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private const long serialVersionUID = 2711172386738607866L;

		private static float _zero = float.PositiveInfinity;

		private static float _one = 0f;
	}
}
