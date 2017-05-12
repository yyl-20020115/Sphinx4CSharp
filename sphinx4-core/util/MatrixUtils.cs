using IKVM.Runtime;
using java.lang;
using java.text;

namespace edu.cmu.sphinx.util
{
	public class MatrixUtils : Object
	{
		public static float[] double2float(double[] values)
		{
			float[] array = new float[values.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (float)values[i];
			}
			return array;
		}

		public static double[] float2double(float[] values)
		{
			double[] array = new double[values.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (double)values[i];
			}
			return array;
		}
		
		public static string toString(double[] m)
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			int num = m.Length;
			for (int i = 0; i < num; i++)
			{
				double num2 = m[i];
				stringBuilder.append(' ').append(MatrixUtils.__df.format(num2));
			}
			return stringBuilder.append(" ]").toString();
		}
		
		public static double[][] float2double(float[][] array)
		{
			int num = array.Length;
			int num2 = array[0].Length;
			int[] array2 = new int[2];
			int num3 = num2;
			array2[1] = num3;
			num3 = num;
			array2[0] = num3;
			double[][] array3 = (double[][])ByteCodeHelper.multianewarray(typeof(double[][]).TypeHandle, array2);
			for (int i = 0; i < array.Length; i++)
			{
				array3[i] = MatrixUtils.float2double(array[i]);
			}
			return array3;
		}
		
		public static string toString(double[][] m)
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			int num = m.Length;
			for (int i = 0; i < num; i++)
			{
				double[] m2 = m[i];
				stringBuilder.append(MatrixUtils.toString(m2));
				stringBuilder.append('\n');
			}
			return stringBuilder.append(" ]").toString();
		}

		public MatrixUtils()
		{
		}
		
		public static int numCols(double[][] m)
		{
			return m[0].Length;
		}
		
		public static string toString(float[][] matrix)
		{
			return MatrixUtils.toString(MatrixUtils.float2double(matrix));
		}
		
		public static float[][] double2float(double[][] array)
		{
			int num = array.Length;
			int num2 = array[0].Length;
			int[] array2 = new int[2];
			int num3 = num2;
			array2[1] = num3;
			num3 = num;
			array2[0] = num3;
			float[][] array3 = (float[][])ByteCodeHelper.multianewarray(typeof(float[][]).TypeHandle, array2);
			for (int i = 0; i < array.Length; i++)
			{
				array3[i] = MatrixUtils.double2float(array[i]);
			}
			return array3;
		}
		
		public static DecimalFormat df
		{
			
			get
			{
				return MatrixUtils.__df;
			}
		}

		internal static DecimalFormat __df = new DecimalFormat("0.00");
	}
}
