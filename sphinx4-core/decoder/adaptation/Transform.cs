using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;
#if DEF_MATH
using org.apache.commons.math3.linear;
#endif
namespace edu.cmu.sphinx.decoder.adaptation
{
	public class Transform : java.lang.Object
	{
		public Transform(Sphinx3Loader loader, int nrOfClusters)
		{
			this.loader = loader;
			this.nrOfClusters = nrOfClusters;
		}
	
		public virtual void load(string filePath)
		{
			Scanner scanner = new Scanner(new File(filePath));
			int num = scanner.nextInt();
			if (!Transform.assertionsDisabled && num != 1)
			{
				
				throw new AssertionError();
			}
			int num2 = scanner.nextInt();
			int num3 = num;
			int num4 = num2;
			int[] array = new int[2];
			int num5 = num4;
			array[1] = num5;
			num5 = num3;
			array[0] = num5;
			this.As = (float[][][][])ByteCodeHelper.multianewarray(typeof(float[][][][]).TypeHandle, array);
			int num6 = num;
			int num7 = num2;
			array = new int[2];
			num5 = num7;
			array[1] = num5;
			num5 = num6;
			array[0] = num5;
			this.Bs = (float[][][])ByteCodeHelper.multianewarray(typeof(float[][][]).TypeHandle, array);
			for (int i = 0; i < num2; i++)
			{
				int num8 = scanner.nextInt();
				float[][][] array2 = this.As[0];
				int num9 = i;
				int num10 = num8;
				int num11 = num8;
				array = new int[2];
				num5 = num11;
				array[1] = num5;
				num5 = num10;
				array[0] = num5;
				array2[num9] = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array);
				this.Bs[0][i] = new float[num8];
				for (int j = 0; j < num8; j++)
				{
					for (int k = 0; k < num8; k++)
					{
						this.As[0][i][j][k] = scanner.nextFloat();
					}
				}
				for (int j = 0; j < num8; j++)
				{
					this.Bs[0][i][j] = scanner.nextFloat();
				}
				for (int j = 0; j < num8; j++)
				{
					scanner.nextFloat();
				}
			}
			scanner.close();
		}
		
		public virtual void update(Stats stats)
		{
			stats.fillRegLowerPart();
			this.As = new float[this.nrOfClusters][][][];
			this.Bs = new float[this.nrOfClusters][][];
			this.computeMllrTransforms(stats.getRegLs(), stats.getRegRs());
		}
	
		private void computeMllrTransforms(double[][][][][] array, double[][][][] array2)
		{
			for (int i = 0; i < this.nrOfClusters; i++)
			{
				this.As[i] = new float[this.loader.getNumStreams()][][];
				this.Bs[i] = new float[this.loader.getNumStreams()][];
				for (int j = 0; j < this.loader.getNumStreams(); j++)
				{
					int num = this.loader.getVectorLength()[j];
					float[][][] array3 = this.As[i];
					int num2 = j;
					int num3 = num;
					int num4 = num;
					int[] array4 = new int[2];
					int num5 = num4;
					array4[1] = num5;
					num5 = num3;
					array4[0] = num5;
					array3[num2] = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array4);
					this.Bs[i][j] = new float[num];
#if DEF_MATH
					for (int k = 0; k < num; k++)
					{
						Array2DRowRealMatrix matrix = new Array2DRowRealMatrix(array[i][j][k], false);
						DecompositionSolver solver = new LUDecomposition(matrix).getSolver();
						ArrayRealVector rv = new ArrayRealVector(array2[i][j][k], false);
						RealVector realVector = solver.solve(rv);
						for (int l = 0; l < num; l++)
						{
							this.As[i][j][k][l] = (float)realVector.getEntry(l);
						}
						this.Bs[i][j][k] = (float)realVector.getEntry(num);
					}
#endif
				}
			}
		}

		public virtual float[][][][] getAs()
		{
			return this.As;
		}

		public virtual float[][][] getBs()
		{
			return this.Bs;
		}
		
		public virtual void store(string filePath, int index)
		{
			PrintWriter printWriter = new PrintWriter(filePath, "UTF-8");
			printWriter.println("1");
			printWriter.println(this.loader.getNumStreams());
			for (int i = 0; i < this.loader.getNumStreams(); i++)
			{
				printWriter.println(this.loader.getVectorLength()[i]);
				for (int j = 0; j < this.loader.getVectorLength()[i]; j++)
				{
					for (int k = 0; k < this.loader.getVectorLength()[i]; k++)
					{
						printWriter.print(this.As[index][i][j][k]);
						printWriter.print(" ");
					}
					printWriter.println();
				}
				for (int j = 0; j < this.loader.getVectorLength()[i]; j++)
				{
					printWriter.print(this.Bs[index][i][j]);
					printWriter.print(" ");
				}
				printWriter.println();
				for (int j = 0; j < this.loader.getVectorLength()[i]; j++)
				{
					printWriter.print("1.0 ");
				}
				printWriter.println();
			}
			printWriter.close();
		}

		private float[][][][] As;

		private float[][][] Bs;

		private Sphinx3Loader loader;

		private int nrOfClusters;

		internal static bool assertionsDisabled = !ClassLiteral<Transform>.Value.desiredAssertionStatus();
	}
}
