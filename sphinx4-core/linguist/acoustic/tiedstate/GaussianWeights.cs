using ikvm.@internal;
using IKVM.Runtime;
using java.lang;
using java.util.logging;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate
{
	public class GaussianWeights : java.lang.Object
	{
		public virtual float get(int stateId, int streamId, int gaussianId)
		{
			return this.weights[gaussianId][stateId * this.numStreams + streamId];
		}
		
		public GaussianWeights(string name, int numStates, int gauPerState, int numStreams)
		{
			this.numStates = numStates;
			this.gauPerState = gauPerState;
			this.numStreams = numStreams;
			this.name = name;
			int num = numStates * numStreams;
			int[] array = new int[2];
			int num2 = num;
			array[1] = num2;
			array[0] = gauPerState;
			this.weights = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array);
		}
		
		public virtual void put(int stateId, int streamId, float[] gauWeights)
		{
			if (!GaussianWeights.assertionsDisabled && gauWeights.Length != this.gauPerState)
			{
				
				throw new AssertionError();
			}
			for (int i = 0; i < this.gauPerState; i++)
			{
				this.weights[i][stateId * this.numStreams + streamId] = gauWeights[i];
			}
		}

		public virtual int getStatesNum()
		{
			return this.numStates;
		}

		public virtual int getGauPerState()
		{
			return this.gauPerState;
		}

		public virtual int getStreamsNum()
		{
			return this.numStreams;
		}

		public virtual string getName()
		{
			return this.name;
		}
		
		public virtual void logInfo(Logger logger)
		{
			logger.info(new StringBuilder().append("Gaussian weights: ").append(this.name).append(". Entries: ").append(this.numStates * this.numStreams).toString());
		}

		public virtual Pool convertToPool()
		{
			return null;
		}
		
		private float[][] weights;
		
		private int numStates;

		private int gauPerState;

		private int numStreams;
		
		private string name;
		
		internal static bool assertionsDisabled = !ClassLiteral<GaussianWeights>.Value.desiredAssertionStatus();
	}
}
