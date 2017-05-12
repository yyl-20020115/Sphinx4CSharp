using edu.cmu.sphinx.frontend;
using edu.cmu.sphinx.util;
using ikvm.@internal;
using java.io;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	[System.Serializable]
	public class MixtureComponent : Object, Cloneable.__Interface, Serializable.__Interface, System.Runtime.Serialization.ISerializable
	{		
		public virtual float getScore(float[] feature)
		{
			float num = this.logPreComputedGaussianFactor;
			for (int i = 0; i < feature.Length; i++)
			{
				float num2 = feature[i] - this.meanTransformed[i];
				num += num2 * num2 * this.precisionTransformed[i];
			}
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

		public virtual float[] getMean()
		{
			return this.mean;
		}
		
		public MixtureComponent(float[] mean, float[][] meanTransformationMatrix, float[] meanTransformationVector, float[] variance, float[][] varianceTransformationMatrix, float[] varianceTransformationVector, float distFloor, float varianceFloor)
		{
			if (!MixtureComponent.assertionsDisabled && variance.Length != mean.Length)
			{
				
				throw new AssertionError();
			}
			this.mean = mean;
			this.meanTransformationMatrix = meanTransformationMatrix;
			this.meanTransformationVector = meanTransformationVector;
			this.variance = variance;
			this.varianceTransformationMatrix = varianceTransformationMatrix;
			this.varianceTransformationVector = varianceTransformationVector;
			if (!MixtureComponent.assertionsDisabled && (double)distFloor < (double)0f)
			{
				object obj = "distFloot seems to be already in log-domain";
				
				throw new AssertionError(obj);
			}
			this.distFloor = LogMath.getLogMath().linearToLog((double)distFloor);
			this.varianceFloor = varianceFloor;
			this.transformStats();
			this.logPreComputedGaussianFactor = this.precomputeDistance();
		}
		
		public virtual void transformStats()
		{
			int num = this.mean.Length;
			if (this.meanTransformationMatrix != null)
			{
				this.meanTransformed = new float[num];
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num; j++)
					{
						float[] array = this.meanTransformed;
						int num2 = i;
						float[] array2 = array;
						array2[num2] += this.mean[j] * this.meanTransformationMatrix[i][j];
					}
				}
			}
			else
			{
				this.meanTransformed = this.mean;
			}
			if (this.meanTransformationVector != null)
			{
				for (int i = 0; i < num; i++)
				{
					float[] array3 = this.meanTransformed;
					int num2 = i;
					float[] array2 = array3;
					array2[num2] += this.meanTransformationVector[i];
				}
			}
			if (this.varianceTransformationMatrix != null)
			{
				this.precisionTransformed = new float[this.variance.Length];
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num; j++)
					{
						float[] array4 = this.precisionTransformed;
						int num2 = i;
						float[] array2 = array4;
						array2[num2] += this.variance[j] * this.varianceTransformationMatrix[i][j];
					}
				}
			}
			else
			{
				this.precisionTransformed = (float[])this.variance.Clone();
			}
			if (this.varianceTransformationVector != null)
			{
				for (int i = 0; i < num; i++)
				{
					float[] array5 = this.precisionTransformed;
					int num2 = i;
					float[] array2 = array5;
					array2[num2] += this.varianceTransformationVector[i];
				}
			}
			for (int i = 0; i < num; i++)
			{
				float num3 = (this.precisionTransformed[i] >= this.varianceFloor) ? this.precisionTransformed[i] : this.varianceFloor;
				this.precisionTransformed[i] = 1f / (-2f * num3);
			}
		}
		
		public virtual float precomputeDistance()
		{
			double num = (double)0f;
			for (int i = 0; i < this.variance.Length; i++)
			{
				num += Math.log((double)(this.precisionTransformed[i] * -2f));
			}
			num = Math.log(6.2831853071795862) * (double)this.variance.Length - num;
			return -(float)num * 0.5f;
		}
		
		public new virtual MixtureComponent clone()
		{
			MixtureComponent mixtureComponent = (MixtureComponent)base.clone();
			mixtureComponent.distFloor = this.distFloor;
			mixtureComponent.varianceFloor = this.varianceFloor;
			mixtureComponent.logPreComputedGaussianFactor = this.logPreComputedGaussianFactor;
			mixtureComponent.mean = ((this.mean == null) ? null : ((float[])this.mean.Clone()));
			if (this.meanTransformationMatrix != null)
			{
				mixtureComponent.meanTransformationMatrix = (float[][])this.meanTransformationMatrix.Clone();
				for (int i = 0; i < this.meanTransformationMatrix.Length; i++)
				{
					mixtureComponent.meanTransformationMatrix[i] = (float[])this.meanTransformationMatrix[i].Clone();
				}
			}
			mixtureComponent.meanTransformationVector = ((this.meanTransformationVector == null) ? null : ((float[])this.meanTransformationVector.Clone()));
			mixtureComponent.meanTransformed = ((this.meanTransformed == null) ? null : ((float[])this.meanTransformed.Clone()));
			mixtureComponent.variance = ((this.variance == null) ? null : ((float[])this.variance.Clone()));
			if (this.varianceTransformationMatrix != null)
			{
				mixtureComponent.varianceTransformationMatrix = (float[][])this.varianceTransformationMatrix.Clone();
				for (int i = 0; i < this.varianceTransformationMatrix.Length; i++)
				{
					mixtureComponent.varianceTransformationMatrix[i] = (float[])this.varianceTransformationMatrix[i].Clone();
				}
			}
			mixtureComponent.varianceTransformationVector = ((this.varianceTransformationVector == null) ? null : ((float[])this.varianceTransformationVector.Clone()));
			mixtureComponent.precisionTransformed = ((this.precisionTransformed == null) ? null : ((float[])this.precisionTransformed.Clone()));
			return mixtureComponent;
		}
		
		public MixtureComponent(float[] mean, float[] variance) : this(mean, (float[][])null, null, variance, (float[][])null, null, 0f, 0.0001f)
		{
		}

		public MixtureComponent(float[] mean, float[][] meanTransformationMatrix, float[] meanTransformationVector, float[] variance, float[][] varianceTransformationMatrix, float[] varianceTransformationVector) : this(mean, meanTransformationMatrix, meanTransformationVector, variance, varianceTransformationMatrix, varianceTransformationVector, 0f, 0.0001f)
		{
		}

		public virtual float[] getVariance()
		{
			return this.variance;
		}		
		
		public virtual float getScore(FloatData feature)
		{
			return this.getScore(feature.getValues());
		}
				
		public override string toString()
		{
			return new StringBuilder().append("mu=").append(Arrays.toString(this.mean)).append(" cov=").append(Arrays.toString(this.variance)).toString();
		}

		
		public static implicit operator Cloneable(MixtureComponent _ref)
		{
			Cloneable result = Cloneable.Cast(_ref);
			return result;
		}

		
		public static implicit operator Serializable(MixtureComponent _ref)
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
		protected MixtureComponent(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
		{
			Serialization.readObject(this, serializationInfo);
		}

		private float[] mean;

		protected internal float[] meanTransformed;

		private float[][] meanTransformationMatrix;

		private float[] meanTransformationVector;

		private float[] variance;

		protected internal float[] precisionTransformed;

		private float[][] varianceTransformationMatrix;

		private float[] varianceTransformationVector;

		protected internal float distFloor;

		private float varianceFloor;

		public const float DEFAULT_VAR_FLOOR = 0.0001f;

		public const float DEFAULT_DIST_FLOOR = 0f;

		protected internal float logPreComputedGaussianFactor;

		internal static bool assertionsDisabled = !ClassLiteral<MixtureComponent>.Value.desiredAssertionStatus();
	}
}
