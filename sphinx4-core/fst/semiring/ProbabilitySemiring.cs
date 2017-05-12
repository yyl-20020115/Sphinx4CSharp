using java.lang;

namespace edu.cmu.sphinx.fst.semiring
{
	[System.Serializable]
	public class ProbabilitySemiring : Semiring
	{
		public override bool isMember(float w)
		{
			return !Float.isNaN(w) && w >= 0f;
		}
		
		public ProbabilitySemiring()
		{
		}
		
		public override float plus(float w1, float w2)
		{
			if (!this.isMember(w1) || !this.isMember(w2))
			{
				return float.NegativeInfinity;
			}
			return w1 + w2;
		}
		
		public override float times(float w1, float w2)
		{
			if (!this.isMember(w1) || !this.isMember(w2))
			{
				return float.NegativeInfinity;
			}
			return w1 * w2;
		}

		public override float divide(float w1, float w2)
		{
			return float.NegativeInfinity;
		}

		public override float zero()
		{
			return ProbabilitySemiring._zero;
		}

		public override float one()
		{
			return ProbabilitySemiring._one;
		}
		
		public override float reverse(float w1)
		{
			java.lang.System.@out.println("Not Implemented");
			return float.NegativeInfinity;
		}

		static ProbabilitySemiring()
		{
			// Note: this type is marked as 'beforefieldinit'.
		}

		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected ProbabilitySemiring(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private const long serialVersionUID = 5592668313009971909L;

		private static float _zero = 0f;

		private static float _one = 1f;
	}
}
