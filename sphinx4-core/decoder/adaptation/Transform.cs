#define USE_MAPACK
using edu.cmu.sphinx.linguist.acoustic.tiedstate;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using java.util;
#if USE_MAPACK
using Mapack;
#else
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
			for (int c = 0; c < this.nrOfClusters; c++)
			{
				this.As[c] = new float[this.loader.getNumStreams()][][];
				this.Bs[c] = new float[this.loader.getNumStreams()][];
				for (int i = 0; i < this.loader.getNumStreams(); i++)
				{
					int len = this.loader.getVectorLength()[i];
					float[][][] array3 = this.As[c];
					int num2 = i;
					int num3 = len;
					int num4 = len;
					int[] array4 = new int[2];
					int num5 = num4;
					array4[1] = num5;
					num5 = num3;
					array4[0] = num5;
					array3[num2] = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array4);
					this.Bs[c][i] = new float[len];

					for (int j = 0; j < len; j++)
					{
#if USE_MAPACK
						//TODO: need to check if the math is correct
						Matrix matrix = new Matrix(array[c][i][j]);

						LuDecomposition lu = new LuDecomposition(matrix);

						int rs = array2[c][i][j].Length;

						Matrix rv = new Matrix(rs, 1);

						for (int r = 0; r < rs; r++)
						{
							rv[r,0] = array2[c][i][j][r];
						}

						Matrix solved = lu.Solve(rv);

						for (int k = 0; k < len; k++)
						{
							this.As[c][i][j][k] = (float)solved[k, 0];
						}
						//TODO: there may be a problem of index out of range?
						//(index == len)?
						this.Bs[c][i][j] = (float)solved[len,0];

#else
						Array2DRowRealMatrix matrix = new Array2DRowRealMatrix(array[c][i][j], false);
						DecompositionSolver solver = new LUDecomposition(matrix).getSolver();
						ArrayRealVector rv = new ArrayRealVector(array2[c][i][j], false);
						RealVector realVector = solver.solve(rv);
						for (int k = 0; k < len; k++)
						{
							this.As[c][i][j][k] = (float)realVector.getEntry(k);
						}
						this.Bs[c][i][j] = (float)realVector.getEntry(len);
#endif

					}
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
