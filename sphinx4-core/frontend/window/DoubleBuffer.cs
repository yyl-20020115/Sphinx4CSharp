﻿using IKVM.Runtime;
using java.lang;
using java.util;

namespace edu.cmu.sphinx.frontend.window
{
	internal sealed class DoubleBuffer : Object
	{		
		public int append(double[] array, int num, int num2)
		{
			if (this.occupancy + num2 > this.buffer.Length)
			{
				string text = "RaisedCosineWindower: overflow-buffer: attempting to fill buffer beyond its capacity.";
				
				throw new Error(text);
			}
			ByteCodeHelper.arraycopy_primitive_8(array, num, this.buffer, this.occupancy, num2);
			this.occupancy += num2;
			return this.occupancy;
		}
		
		internal DoubleBuffer(int num)
		{
			this.buffer = new double[num];
			this.occupancy = 0;
		}

		public int getOccupancy()
		{
			return this.occupancy;
		}

		public double[] getBuffer()
		{
			return this.buffer;
		}		
		
		public int appendAll(double[] array)
		{
			return this.append(array, 0, array.Length);
		}
		
		public void padWindow(int num)
		{
			if (this.occupancy < num)
			{
				Arrays.fill(this.buffer, this.occupancy, num, (double)0f);
			}
		}

		public void reset()
		{
			this.occupancy = 0;
		}

		private double[] buffer;

		private int occupancy;
	}
}
