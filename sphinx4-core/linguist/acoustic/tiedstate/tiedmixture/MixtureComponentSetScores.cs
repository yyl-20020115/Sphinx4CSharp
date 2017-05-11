using IKVM.Runtime;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.tiedmixture
{
	public class MixtureComponentSetScores : java.lang.Object
	{
		public virtual long getFrameStartSample()
		{
			return this.frameStartSample;
		}
		
		public MixtureComponentSetScores(int numStreams, int gauNum, long frameStartSample)
		{
			int[] array = new int[]
			{
				0,
				gauNum
			};
			array[0] = numStreams;
			this.scores = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array);
			array = new int[]
			{
				0,
				gauNum
			};
			array[0] = numStreams;
			this.ids = (int[][])ByteCodeHelper.multianewarray(typeof(int[][]).TypeHandle, array);
			this.frameStartSample = frameStartSample;
		}

		public virtual void setScore(int featStream, int gauIdx, float score)
		{
			this.scores[featStream][gauIdx] = score;
		}

		public virtual void setGauId(int featStream, int gauIdx, int id)
		{
			this.ids[featStream][gauIdx] = id;
		}
		
		public virtual float getScore(int featStream, int gauIdx)
		{
			return this.scores[featStream][gauIdx];
		}
		
		public virtual int getGauId(int featStream, int gauIdx)
		{
			return this.ids[featStream][gauIdx];
		}

		private float[][] scores;

		private int[][] ids;

		private long frameStartSample;
	}
}
