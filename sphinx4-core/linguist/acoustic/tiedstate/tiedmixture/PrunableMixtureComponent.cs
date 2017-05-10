using System;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.util;
using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.tiedmixture
{
	[Serializable]
	public class PrunableMixtureComponent : MixtureComponent
	{
		
		public new static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			159,
			179,
			247,
			50,
			107,
			235,
			78,
			104
		})]
		
		public PrunableMixtureComponent(float[] mean, float[][] meanTransformationMatrix, float[] meanTransformationVector, float[] variance, float[][] varianceTransformationMatrix, float[] varianceTransformationVector, float distFloor, float varianceFloor, int id) : base(mean, meanTransformationMatrix, meanTransformationVector, variance, varianceTransformationMatrix, varianceTransformationVector, distFloor, varianceFloor)
		{
			this.score = float.MinValue;
			this.partScore = float.MinValue;
			this.id = id;
		}

		public virtual float getStoredScore()
		{
			return this.score;
		}

		public virtual int getId()
		{
			return this.id;
		}

		public virtual float getPartialScore()
		{
			return this.partScore;
		}

		[LineNumberTable(new byte[]
		{
			31,
			231,
			70,
			103,
			110,
			18,
			230,
			69,
			103,
			109
		})]
		
		public virtual void updateScore(float[] feature)
		{
			float num = this.logPreComputedGaussianFactor;
			for (int i = 0; i < feature.Length; i++)
			{
				float num2 = feature[i] - this.meanTransformed[i];
				num += num2 * num2 * this.precisionTransformed[i];
			}
			this.partScore = num;
			this.score = this.convertScore(num);
		}

		[LineNumberTable(new byte[]
		{
			11,
			231,
			70,
			103,
			110,
			114,
			101,
			226,
			60,
			230,
			71,
			103,
			109
		})]
		
		public virtual bool isTopComponent(float[] feature, float threshold)
		{
			float num = this.logPreComputedGaussianFactor;
			for (int i = 0; i < feature.Length; i++)
			{
				float num2 = feature[i] - this.meanTransformed[i];
				num += num2 * num2 * this.precisionTransformed[i];
				if (num < threshold)
				{
					return false;
				}
			}
			this.partScore = num;
			this.score = this.convertScore(num);
			return true;
		}

		[LineNumberTable(new byte[]
		{
			159,
			185,
			206,
			105,
			111,
			167,
			106,
			168
		})]
		
		private float convertScore(float num)
		{
			num = LogMath.getLogMath().lnToLog(num);
			if (Float.isNaN(num))
			{
				java.lang.System.@out.println("gs is Nan, converting to 0");
				num = float.MinValue;
			}
			if (num < this.distFloor)
			{
				num = this.distFloor;
			}
			return num;
		}

		
		static PrunableMixtureComponent()
		{
			MixtureComponent.__<clinit>();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected PrunableMixtureComponent(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private float score;

		private float partScore;

		private int id;
	}
}
