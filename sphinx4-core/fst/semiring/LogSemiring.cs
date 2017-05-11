using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using java.lang;

namespace edu.cmu.sphinx.fst.semiring
{
	[Serializable]
	public class LogSemiring : Semiring
	{
		public override bool isMember(float w)
		{
			return !Float.isNaN(w) && w != float.NegativeInfinity;
		}
		
		public LogSemiring()
		{
		}
		
		public override float plus(float w1, float w2)
		{
			if (!this.isMember(w1) || !this.isMember(w2))
			{
				return float.NegativeInfinity;
			}
			if (w1 == float.PositiveInfinity)
			{
				return w2;
			}
			if (w2 == float.PositiveInfinity)
			{
				return w1;
			}
			return (float)(-(float)java.lang.Math.log(java.lang.Math.exp((double)(-(double)w1)) + java.lang.Math.exp((double)(-(double)w2))));
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
			if (w2 == LogSemiring.zero)
			{
				return float.NegativeInfinity;
			}
			if (w1 == LogSemiring.zero)
			{
				return LogSemiring.zero;
			}
			return w1 - w2;
		}

		public override float zero()
		{
			return LogSemiring._zero;
		}

		public override float one()
		{
			return LogSemiring._one;
		}
		
		public override float reverse(float w1)
		{
			java.lang.System.@out.println("Not Implemented");
			return float.NegativeInfinity;
		}
		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected LogSemiring(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private const long serialVersionUID = 5212106775584311083L;

		private static float _zero = float.PositiveInfinity;

		private static float _one = 0f;
	}
}
