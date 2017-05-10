using System;

using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;
using java.util;
using org.apache.commons.math3.util;

namespace edu.cmu.sphinx.decoder.adaptation
{
	public class ClusteredDensityFileData : java.lang.Object
	{		
		public ClusteredDensityFileData(Loader loader, int numberOfClusters)
		{
			this.numberOfClusters = numberOfClusters;
			this.kMeansClustering(loader, 30);
		}
	
		private void kMeansClustering(Loader loader, int num)
		{
			Pool meansPool = loader.getMeansPool();
			ArrayList arrayList = new ArrayList(this.numberOfClusters);
			ArrayList arrayList2 = new ArrayList(this.numberOfClusters);
			int num2 = meansPool.size();
			int num3 = num;
			int[] array = new int[this.numberOfClusters];
			int num4 = this.numberOfClusters;
			int num5 = num2;
			int[] array2 = new int[2];
			int num6 = num5;
			array2[1] = num6;
			num6 = num4;
			array2[0] = num6;
			float[][][] array3 = (float[][][])ByteCodeHelper.multianewarray(typeof(float[][][]).TypeHandle, array2);
			int num7 = 0;
			java.util.Random random = new java.util.Random();
			for (int i = 0; i < this.numberOfClusters; i++)
			{
				int num8 = random.nextInt(num2);
				arrayList2.add(meansPool.get(num8));
				arrayList.add(meansPool.get(num8));
				array[i] = 0;
			}
			while (num7 == 0 && num3 > 0)
			{
				this.corespondingClass = new int[meansPool.size()];
				int num9 = this.numberOfClusters;
				int num10 = num2;
				array2 = new int[2];
				num6 = num10;
				array2[1] = num6;
				num6 = num9;
				array2[0] = num6;
				float[][][] array4 = (float[][][])ByteCodeHelper.multianewarray(typeof(float[][][]).TypeHandle, array2);
				for (int i = 0; i < this.numberOfClusters; i++)
				{
					arrayList.set(i, arrayList2.get(i));
					array[i] = 0;
				}
				for (int i = 0; i < meansPool.size(); i++)
				{
					float[] array5 = (float[])meansPool.get(i);
					double num11 = (double)this.euclidianDistance((float[])arrayList.get(0), array5);
					int num8 = 0;
					for (int j = 1; j < this.numberOfClusters; j++)
					{
						double num12 = (double)this.euclidianDistance((float[])arrayList.get(j), array5);
						if (num12 < num11)
						{
							num11 = num12;
							num8 = j;
						}
					}
					array4[num8][array[num8]] = array5;
					this.corespondingClass[i] = num8;
					int[] array6 = array;
					num6 = num8;
					array2 = array6;
					array2[num6]++;
				}
				for (int i = 0; i < this.numberOfClusters; i++)
				{
					float[] array7 = new float[((float[])meansPool.get(0)).Length];
					if (array[i] > 0)
					{
						for (int j = 0; j < array[i]; j++)
						{
							for (int k = 0; k < ((float[])meansPool.get(0)).Length; k++)
							{
								float[] array8 = array7;
								num6 = k;
								float[] array9 = array8;
								array9[num6] += array4[i][j][k];
							}
						}
						for (int j = 0; j < ((float[])meansPool.get(0)).Length; j++)
						{
							float[] array10 = array7;
							num6 = j;
							float[] array9 = array10;
							array9[num6] /= (float)array[i];
						}
						arrayList2.set(i, array7);
					}
				}
				num7 = 1;
				for (int i = 0; i < this.numberOfClusters; i++)
				{
					num7 = ((num7 == 0 || !this.isEqual((float[])arrayList2.get(i), (float[])arrayList.get(i))) ? 0 : 1);
				}
				num3 += -1;
			}
		}
	
		private float euclidianDistance(float[] array, float[] array2)
		{
			double num = (double)0f;
			for (int i = 0; i < array.Length; i++)
			{
				double num2 = (double)(array[i] - array2[i]);
				num += num2 * num2;
			}
			return (float)FastMath.sqrt(num);
		}

		private bool isEqual(float[] array, float[] array2)
		{
			if (array.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		public virtual int getNumberOfClusters()
		{
			return this.numberOfClusters;
		}

		
		public virtual int getClassIndex(int gaussian)
		{
			return this.corespondingClass[gaussian];
		}

		private int numberOfClusters;

		private int[] corespondingClass;
	}
}
