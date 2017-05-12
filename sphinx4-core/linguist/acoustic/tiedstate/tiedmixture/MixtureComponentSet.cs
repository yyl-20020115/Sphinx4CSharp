using edu.cmu.sphinx.frontend;
using IKVM.Runtime;
using java.util;
using java.lang;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.tiedmixture
{
	public class MixtureComponentSet : Object
	{
		public MixtureComponentSet(ArrayList components, int topGauNum)
		{
			this.componentComparator = new MixtureComponentSet_1(this);
			this.components = components;
			this.numStreams = components.size();
			this.topGauNum = topGauNum;
			this.gauNum = ((PrunableMixtureComponent[])components.get(0)).Length;
			this.topComponents = new ArrayList();
			for (int i = 0; i < this.numStreams; i++)
			{
				PrunableMixtureComponent[] array = new PrunableMixtureComponent[topGauNum];
				for (int j = 0; j < topGauNum; j++)
				{
					array[j] = ((PrunableMixtureComponent[])components.get(i))[j];
				}
				this.topComponents.add(array);
			}
			this.gauCalcSampleNumber = -1L;
			this.toStoreScore = false;
			this.storedScores = new LinkedList();
			this.curScores = null;
		}
		
		public virtual void clearStoredScores()
		{
			this.storedScores.clear();
		}

		public virtual void setScoreQueueLength(int scoresQueueLen)
		{
			this.toStoreScore = (scoresQueueLen > 0);
			this.scoresQueueLen = scoresQueueLen;
		}
		
		private bool isInTopComponents(PrunableMixtureComponent[] array, PrunableMixtureComponent prunableMixtureComponent)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				PrunableMixtureComponent prunableMixtureComponent2 = array[i];
				if (prunableMixtureComponent2.getId() == prunableMixtureComponent.getId())
				{
					return true;
				}
			}
			return false;
		}
		
		private void insertTopComponent(PrunableMixtureComponent[] array, PrunableMixtureComponent prunableMixtureComponent)
		{
			for (int i = 0; i < array.Length - 1; i++)
			{
				if (prunableMixtureComponent.getPartialScore() < array[i].getPartialScore())
				{
					array[i - 1] = prunableMixtureComponent;
					return;
				}
				array[i] = array[i + 1];
			}
			if (prunableMixtureComponent.getPartialScore() < array[array.Length - 1].getPartialScore())
			{
				array[array.Length - 2] = prunableMixtureComponent;
			}
			else
			{
				array[array.Length - 1] = prunableMixtureComponent;
			}
		}
		
		private MixtureComponentSetScores getStoredScores(long num)
		{
			if (this.storedScores.isEmpty())
			{
				return null;
			}
			if (((MixtureComponentSetScores)this.storedScores.peekLast()).getFrameStartSample() < num)
			{
				return null;
			}
			Iterator iterator = this.storedScores.iterator();
			while (iterator.hasNext())
			{
				MixtureComponentSetScores mixtureComponentSetScores = (MixtureComponentSetScores)iterator.next();
				if (mixtureComponentSetScores.getFrameStartSample() == num)
				{
					return mixtureComponentSetScores;
				}
			}
			return null;
		}

		
		private void updateTopScores(float[] array)
		{
			int num = array.Length;
			int num2 = this.numStreams;
			int num3 = (num2 != -1) ? (num / num2) : (-num);
			float[] array2 = new float[num3];
			for (int i = 0; i < this.numStreams; i++)
			{
				ByteCodeHelper.arraycopy_primitive_4(array, i * num3, array2, 0, num3);
				PrunableMixtureComponent[] array3 = (PrunableMixtureComponent[])this.topComponents.get(i);
				PrunableMixtureComponent[] array4 = (PrunableMixtureComponent[])this.components.get(i);
				PrunableMixtureComponent[] array5 = array3;
				int num4 = array5.Length;
				int j;
				for (j = 0; j < num4; j++)
				{
					PrunableMixtureComponent prunableMixtureComponent = array5[j];
					prunableMixtureComponent.updateScore(array2);
				}
				Arrays.sort(array3, this.componentComparator);
				float partialScore = array3[0].getPartialScore();
				PrunableMixtureComponent[] array6 = array4;
				j = array6.Length;
				for (int k = 0; k < j; k++)
				{
					PrunableMixtureComponent prunableMixtureComponent2 = array6[k];
					if (!this.isInTopComponents(array3, prunableMixtureComponent2))
					{
						if (prunableMixtureComponent2.isTopComponent(array2, partialScore))
						{
							this.insertTopComponent(array3, prunableMixtureComponent2);
							partialScore = array3[0].getPartialScore();
						}
					}
				}
			}
		}
		
		private MixtureComponentSetScores createFromTopGau(long frameStartSample)
		{
			MixtureComponentSetScores mixtureComponentSetScores = new MixtureComponentSetScores(this.numStreams, this.topGauNum, frameStartSample);
			for (int i = 0; i < this.numStreams; i++)
			{
				for (int j = 0; j < this.topGauNum; j++)
				{
					mixtureComponentSetScores.setScore(i, j, ((PrunableMixtureComponent[])this.topComponents.get(i))[j].getStoredScore());
					mixtureComponentSetScores.setGauId(i, j, ((PrunableMixtureComponent[])this.topComponents.get(i))[j].getId());
				}
			}
			return mixtureComponentSetScores;
		}
		
		private void storeScores(MixtureComponentSetScores mixtureComponentSetScores)
		{
			this.storedScores.add(mixtureComponentSetScores);
			while (this.storedScores.size() > this.scoresQueueLen)
			{
				this.storedScores.poll();
			}
		}
		
		private void updateScores(float[] array)
		{
			int num = array.Length;
			int num2 = this.numStreams;
			int num3 = (num2 != -1) ? (num / num2) : (-num);
			float[] array2 = new float[num3];
			for (int i = 0; i < this.numStreams; i++)
			{
				ByteCodeHelper.arraycopy_primitive_4(array, i * num3, array2, 0, num3);
				PrunableMixtureComponent[] array3 = (PrunableMixtureComponent[])this.components.get(i);
				int num4 = array3.Length;
				for (int j = 0; j < num4; j++)
				{
					PrunableMixtureComponent prunableMixtureComponent = array3[j];
					prunableMixtureComponent.updateScore(array2);
				}
			}
		}
		
		private object[] concatenate(object[] array, object[] array2)
		{
			int num = array.Length;
			int num2 = array2.Length;
		
			object[] array3 = (object[])java.lang.reflect.Array.newInstance(Object.instancehelper_getClass(array).getComponentType(), num + num2);
			ByteCodeHelper.arraycopy(array, 0, array3, 0, num);
			ByteCodeHelper.arraycopy(array2, 0, array3, num, num2);
			return array3;
		}
		
		public virtual void updateTopScores(Data feature)
		{
			if (feature is DoubleData)
			{
				java.lang.System.err.println("DoubleData conversion required on mixture level!");
			}
			long firstSampleNumber = FloatData.toFloatData(feature).getFirstSampleNumber();
			if (this.toStoreScore)
			{
				this.curScores = this.getStoredScores(firstSampleNumber);
			}
			else if (this.curScores != null && this.curScores.getFrameStartSample() != firstSampleNumber)
			{
				this.curScores = null;
			}
			if (this.curScores != null)
			{
				return;
			}
			float[] values = FloatData.toFloatData(feature).getValues();
			this.updateTopScores(values);
			this.curScores = this.createFromTopGau(firstSampleNumber);
			if (this.toStoreScore)
			{
				this.storeScores(this.curScores);
			}
		}
		
		public virtual void updateScores(Data feature)
		{
			if (feature is DoubleData)
			{
				java.lang.System.err.println("DoubleData conversion required on mixture level!");
			}
			long firstSampleNumber = FloatData.toFloatData(feature).getFirstSampleNumber();
			if (this.gauCalcSampleNumber != firstSampleNumber)
			{
				float[] values = FloatData.toFloatData(feature).getValues();
				this.updateScores(values);
				this.gauCalcSampleNumber = firstSampleNumber;
			}
		}

		public virtual int getTopGauNum()
		{
			return this.topGauNum;
		}

		public virtual int getGauNum()
		{
			return this.gauNum;
		}		
		
		public virtual float getTopGauScore(int streamId, int topGauId)
		{
			return this.curScores.getScore(streamId, topGauId);
		}
		
		public virtual int getTopGauId(int streamId, int topGauId)
		{
			return this.curScores.getGauId(streamId, topGauId);
		}		
		
		public virtual float getGauScore(int streamId, int topGauId)
		{
			return ((PrunableMixtureComponent[])this.components.get(streamId))[topGauId].getStoredScore();
		}		
		
		public virtual int getGauId(int streamId, int topGauId)
		{
			return ((PrunableMixtureComponent[])this.components.get(streamId))[topGauId].getId();
		}
		
		protected internal virtual MixtureComponent[] toArray()
		{
			PrunableMixtureComponent[] array = new PrunableMixtureComponent[0];
			for (int i = 0; i < this.numStreams; i++)
			{
				this.concatenate(array, (object[])this.components.get(i));
			}
			return array;
		}
		
		protected internal virtual int dimension()
		{
			int num = 0;
			for (int i = 0; i < this.numStreams; i++)
			{
				num += ((PrunableMixtureComponent[])this.components.get(i))[0].getMean().Length;
			}
			return num;
		}
		protected internal virtual int size()
		{
			int num = 0;
			for (int i = 0; i < this.numStreams; i++)
			{
				num += ((PrunableMixtureComponent[])this.components.get(0)).Length;
			}
			return num;
		}

		private int scoresQueueLen;

		private bool toStoreScore;

		private LinkedList storedScores;

		internal MixtureComponentSetScores curScores;

		private ArrayList components;

		private ArrayList topComponents;

		private int numStreams;

		private int topGauNum;

		private int gauNum;

		private long gauCalcSampleNumber;

		private Comparator componentComparator;
	}
}
