using IKVM.Runtime;
using java.util;

namespace edu.cmu.sphinx.frontend.frequencywarp
{
	public class LinearPredictor : java.lang.Object
	{
		public LinearPredictor(int order)
		{
			this.order = order;
			this.reflectionCoeffs = null;
			this.ARParameters = null;
			this.alpha = (double)0f;
			this.cepstra = null;
			this.bilinearCepstra = null;
		}

		public virtual double[] getARFilter(double[] autocor)
		{
			if (autocor[0] == (double)0f)
			{
				return null;
			}
			this.reflectionCoeffs = new double[this.order + 1];
			this.ARParameters = new double[this.order + 1];
			double[] array = new double[this.order + 1];
			this.alpha = autocor[0];
			this.reflectionCoeffs[1] = -autocor[1] / autocor[0];
			this.ARParameters[0] = (double)1f;
			this.ARParameters[1] = this.reflectionCoeffs[1];
			this.alpha *= (double)1f - this.reflectionCoeffs[1] * this.reflectionCoeffs[1];
			for (int i = 2; i <= this.order; i++)
			{
				for (int j = 1; j < i; j++)
				{
					array[j] = this.ARParameters[i - j];
				}
				this.reflectionCoeffs[i] = (double)0f;
				int num;
				double[] array3;
				for (int j = 0; j < i; j++)
				{
					double[] array2 = this.reflectionCoeffs;
					num = i;
					array3 = array2;
					array3[num] -= this.ARParameters[j] * autocor[i - j];
				}
				double[] array4 = this.reflectionCoeffs;
				num = i;
				array3 = array4;
				array3[num] /= this.alpha;
				for (int j = 1; j < i; j++)
				{
					double[] arparameters = this.ARParameters;
					num = j;
					array3 = arparameters;
					array3[num] += this.reflectionCoeffs[i] * array[j];
				}
				this.ARParameters[i] = this.reflectionCoeffs[i];
				this.alpha *= (double)1f - this.reflectionCoeffs[i] * this.reflectionCoeffs[i];
				if (this.alpha <= (double)0f)
				{
					return null;
				}
			}
			return this.ARParameters;
		}

		public virtual double[] reflectionCoeffsToARParameters(double[] RC, int lpcorder)
		{
			int num = lpcorder + 1;
			int num2 = lpcorder + 1;
			int[] array = new int[2];
			int num3 = num2;
			array[1] = num3;
			num3 = num;
			array[0] = num3;
			double[][] array2 = (double[][])ByteCodeHelper.multianewarray(typeof(double[][]).TypeHandle, array);
			this.order = lpcorder;
			this.reflectionCoeffs = (double[])RC.Clone();
			for (int i = 1; i <= lpcorder; i++)
			{
				for (int j = 1; j < i; j++)
				{
					array2[i][j] = array2[i - 1][j] - RC[i] * array2[i - 1][i - j];
				}
				array2[i][i] = RC[i];
			}
			this.ARParameters[0] = (double)1f;
			for (int i = 1; i <= lpcorder; i++)
			{
				this.ARParameters[i] = array2[i][i];
			}
			return this.ARParameters;
		}
		
		public virtual double[] getData(int ceporder)
		{
			if (ceporder <= 0)
			{
				return null;
			}
			this.cepstrumOrder = ceporder;
			this.cepstra = new double[this.cepstrumOrder];
			this.cepstra[0] = java.lang.Math.log(this.alpha);
			if (this.cepstrumOrder == 1)
			{
				return this.cepstra;
			}
			this.cepstra[1] = -this.ARParameters[1];
			int i;
			for (i = 2; i < java.lang.Math.min(this.cepstrumOrder, this.order + 1); i++)
			{
				double num = (double)i * this.ARParameters[i];
				for (int j = 1; j < i; j++)
				{
					num += this.ARParameters[j] * this.cepstra[i - j] * (double)(i - j);
				}
				this.cepstra[i] = -num / (double)i;
			}
			while (i < this.cepstrumOrder)
			{
				double num = (double)0f;
				for (int j = 1; j <= this.order; j++)
				{
					num += this.ARParameters[j] * this.cepstra[i - j] * (double)(i - j);
				}
				this.cepstra[i] = -num / (double)i;
				i++;
			}
			return this.cepstra;
		}
		
		public virtual double[] getBilinearCepstra(double warp, int nbilincepstra)
		{
			int num = this.cepstrumOrder;
			int[] array = new int[2];
			int num2 = num;
			array[1] = num2;
			array[0] = nbilincepstra;
			double[][] array2 = (double[][])ByteCodeHelper.multianewarray(typeof(double[][]).TypeHandle, array);
			double[] array3 = Arrays.copyOf(this.cepstra, this.cepstrumOrder);
			this.bilinearCepstra[0] = array3[0];
			array3[0] = (double)0f;
			array2[0][this.cepstrumOrder - 1] = array3[this.cepstrumOrder - 1];
			for (int i = 1; i < nbilincepstra; i++)
			{
				array2[i][this.cepstrumOrder - 1] = (double)0f;
			}
			for (int i = this.cepstrumOrder - 2; i >= 0; i += -1)
			{
				array2[0][i] = warp * array2[0][i + 1] + array3[i];
				array2[1][i] = ((double)1f - warp * warp) * array2[0][i + 1] + warp * array2[1][i + 1];
				for (int j = 2; j < nbilincepstra; j++)
				{
					array2[j][i] = warp * (array2[j][i + 1] - array2[j - 1][i]) + array2[j - 1][i + 1];
				}
			}
			for (int i = 1; i <= nbilincepstra; i++)
			{
				this.bilinearCepstra[i] = array2[i][0];
			}
			return this.bilinearCepstra;
		}

		private int order;

		private int cepstrumOrder;

		private double[] reflectionCoeffs;

		private double[] ARParameters;

		private double alpha;

		private double[] cepstra;

		private double[] bilinearCepstra;
	}
}
