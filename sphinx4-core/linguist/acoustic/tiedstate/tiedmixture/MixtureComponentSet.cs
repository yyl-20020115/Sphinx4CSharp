using System;

using edu.cmu.sphinx.frontend;
using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;
using java.lang.reflect;
using java.util;

namespace edu.cmu.sphinx.linguist.acoustic.tiedstate.tiedmixture
{
	public class MixtureComponentSet : java.lang.Object
	{
		
		[LineNumberTable(new byte[]
		{
			159,
			183,
			232,
			160,
			220,
			236,
			159,
			37,
			103,
			108,
			103,
			115,
			107,
			107,
			103,
			102,
			49,
			134,
			237,
			60,
			230,
			70,
			104,
			103,
			107,
			103
		})]
		
		public MixtureComponentSet(ArrayList components, int topGauNum)
		{
			this.componentComparator = new MixtureComponentSet$1(this);
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

		[LineNumberTable(new byte[]
		{
			160,
			75,
			107
		})]
		
		public virtual void clearStoredScores()
		{
			this.storedScores.clear();
		}

		public virtual void setScoreQueueLength(int scoresQueueLen)
		{
			this.toStoreScore = (scoresQueueLen > 0);
			this.scoresQueueLen = scoresQueueLen;
		}

		[LineNumberTable(new byte[]
		{
			56,
			111,
			110,
			2,
			166
		})]
		
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

		[LineNumberTable(new byte[]
		{
			42,
			105,
			112,
			102,
			129,
			232,
			59,
			230,
			71,
			115,
			137,
			103
		})]
		
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

		[LineNumberTable(new byte[]
		{
			16,
			109,
			98,
			152,
			98,
			127,
			1,
			105,
			98,
			130
		})]
		
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

		[LineNumberTable(new byte[]
		{
			63,
			146,
			103,
			110,
			108,
			114,
			179,
			120,
			40,
			136,
			172,
			106,
			121,
			107,
			98,
			108,
			105,
			234,
			59,
			232,
			52,
			233,
			85
		})]
		
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

		[LineNumberTable(new byte[]
		{
			30,
			115,
			110,
			107,
			127,
			1,
			31,
			1,
			38,
			233,
			70
		})]
		
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

		[LineNumberTable(new byte[]
		{
			10,
			109,
			115,
			110
		})]
		
		private void storeScores(MixtureComponentSetScores mixtureComponentSetScores)
		{
			this.storedScores.add(mixtureComponentSetScores);
			while (this.storedScores.size() > this.scoresQueueLen)
			{
				this.storedScores.poll();
			}
		}

		[LineNumberTable(new byte[]
		{
			113,
			114,
			103,
			110,
			108,
			127,
			6,
			40,
			8,
			233,
			70
		})]
		
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

		
		[LineNumberTable(new byte[]
		{
			160,
			113,
			99,
			163,
			126,
			106,
			138
		})]
		
		private object[] concatenate(object[] array, object[] array2)
		{
			int num = array.Length;
			int num2 = array2.Length;
			object[] array3 = (object[])((object[])Array.newInstance(Object.instancehelper_getClass(array).getComponentType(), num + num2));
			ByteCodeHelper.arraycopy(array, 0, array3, 0, num);
			ByteCodeHelper.arraycopy(array2, 0, array3, num, num2);
			return array3;
		}

		[LineNumberTable(new byte[]
		{
			91,
			104,
			143,
			108,
			104,
			143,
			118,
			135,
			136,
			97,
			108,
			135,
			109,
			104,
			108
		})]
		
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

		[LineNumberTable(new byte[]
		{
			124,
			104,
			143,
			108,
			105,
			108,
			103,
			135
		})]
		
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

		[LineNumberTable(new byte[]
		{
			160,
			125,
			103,
			107,
			57,
			134
		})]
		
		protected internal virtual MixtureComponent[] toArray()
		{
			PrunableMixtureComponent[] array = new PrunableMixtureComponent[0];
			for (int i = 0; i < this.numStreams; i++)
			{
				this.concatenate(array, (object[])this.components.get(i));
			}
			return array;
		}

		[LineNumberTable(new byte[]
		{
			160,
			132,
			98,
			107,
			60,
			166
		})]
		
		protected internal virtual int dimension()
		{
			int num = 0;
			for (int i = 0; i < this.numStreams; i++)
			{
				num += ((PrunableMixtureComponent[])this.components.get(i))[0].getMean().Length;
			}
			return num;
		}

		[LineNumberTable(new byte[]
		{
			160,
			140,
			98,
			107,
			53,
			166
		})]
		
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
