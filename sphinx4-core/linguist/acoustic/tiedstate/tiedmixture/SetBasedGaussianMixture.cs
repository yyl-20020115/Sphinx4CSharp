using edu.cmu.sphinx.frontend;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.tiedmixture
{
	[System.Serializable]
	public class SetBasedGaussianMixture : GaussianMixture
	{	
		public SetBasedGaussianMixture(GaussianWeights mixtureWeights, MixtureComponentSet mixtureComponentSet, int id) : base(mixtureWeights, null, id)
		{
			this.mixtureComponentSet = mixtureComponentSet;
		}
		
		protected internal override float calculateScore(Data feature)
		{
			this.mixtureComponentSet.updateTopScores(feature);
			float num = 0f;
			for (int i = 0; i < this.mixtureWeights.getStreamsNum(); i++)
			{
				float num2 = float.MinValue;
				for (int j = 0; j < this.mixtureComponentSet.getTopGauNum(); j++)
				{
					float topGauScore = this.mixtureComponentSet.getTopGauScore(i, j);
					int topGauId = this.mixtureComponentSet.getTopGauId(i, j);
					num2 = this.logMath.addAsLinear(num2, topGauScore + this.mixtureWeights.get(this.id, i, topGauId));
				}
				num += num2;
			}
			return num;
		}
		
		public override float[] calculateComponentScore(Data feature)
		{
			this.mixtureComponentSet.updateScores(feature);
			float[] array = new float[this.mixtureComponentSet.size()];
			int num = 0;
			for (int i = 0; i < this.mixtureWeights.getStreamsNum(); i++)
			{
				for (int j = 0; j < this.mixtureComponentSet.getGauNum(); j++)
				{
					float[] array2 = array;
					int num2 = num;
					num++;
					array2[num2] = this.mixtureComponentSet.getGauScore(i, j) + this.mixtureWeights.get(this.id, i, this.mixtureComponentSet.getGauId(i, j));
				}
			}
			return array;
		}
		
		public override MixtureComponent[] getMixtureComponents()
		{
			return this.mixtureComponentSet.toArray();
		}
				
		public override int dimension()
		{
			return this.mixtureComponentSet.dimension();
		}
	
		public override int numComponents()
		{
			return this.mixtureComponentSet.size();
		}
	
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected SetBasedGaussianMixture(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private MixtureComponentSet mixtureComponentSet;
	}
}
