﻿using System;

using IKVM.Attributes;
using ikvm.@internal;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.util
{
	public sealed class LogMath : java.lang.Object
	{
		
		public static void __<clinit>()
		{
		}

		[LineNumberTable(new byte[]
		{
			48,
			104,
			108,
			104,
			106,
			175
		})]
		
		public static LogMath getLogMath()
		{
			if (null == LogMath.instance)
			{
				lock (ClassLiteral<LogMath>.Value)
				{
					if (null == LogMath.instance)
					{
						LogMath.instance = new LogMath();
					}
				}
			}
			return LogMath.instance;
		}

		
		
		public double logToLinear(float logValue)
		{
			return java.lang.Math.exp((double)this.logToLn(logValue));
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		
		
		public float linearToLog(double linearValue)
		{
			return (float)java.lang.Math.log(linearValue) * this.inverseNaturalLogBase;
		}

		
		
		public static float log10(float value)
		{
			return (float)(0.4342944819 * java.lang.Math.log((double)value));
		}

		[LineNumberTable(new byte[]
		{
			99,
			99,
			231,
			71,
			104,
			99,
			131
		})]
		
		public float addAsLinear(float logVal1, float logVal2)
		{
			float num = logVal1;
			float num2 = logVal1 - logVal2;
			if (num2 < 0f)
			{
				num = logVal2;
				num2 = -num2;
			}
			return num + this.addTable(num2);
		}

		[LineNumberTable(new byte[]
		{
			160,
			232,
			99,
			102,
			45,
			166
		})]
		
		public void linearToLog(float[] vector)
		{
			int num = vector.Length;
			for (int i = 0; i < num; i++)
			{
				vector[i] = this.linearToLog((double)vector[i]);
			}
		}

		public float lnToLog(float logSource)
		{
			return logSource * this.inverseNaturalLogBase;
		}

		[LineNumberTable(new byte[]
		{
			160,
			244,
			103,
			45,
			166
		})]
		
		public void logToLinear(float[] vector, float[] @out)
		{
			for (int i = 0; i < vector.Length; i++)
			{
				@out[i] = (float)this.logToLinear(vector[i]);
			}
		}

		
		
		public float log10ToLog(float logSource)
		{
			return LogMath.logToLog(logSource, 10f, LogMath.logBase);
		}

		public float getLogBase()
		{
			return LogMath.logBase;
		}

		[LineNumberTable(new byte[]
		{
			159,
			177,
			104,
			114,
			115,
			234,
			70,
			102,
			226,
			86,
			103,
			221,
			104,
			134,
			100,
			255,
			20,
			71,
			108,
			230,
			70,
			107,
			105,
			240,
			56,
			230,
			75
		})]
		
		private LogMath()
		{
			this.naturalLogBase = (float)java.lang.Math.log((double)LogMath.logBase);
			this.inverseNaturalLogBase = 1f / this.naturalLogBase;
			if (LogMath.useTable)
			{
				int num = ByteCodeHelper.d2i(-java.lang.Math.rint((double)this.linearToLog(this.logToLinear(0.5f) - (double)1f)));
				if (num > 150000)
				{
					num = 150000;
				}
				if (num <= 0)
				{
					string text = new StringBuilder().append("The log base ").append(LogMath.logBase).append(" yields a very small addTable. Either choose not to use the addTable, or choose a logBase closer to 1.0").toString();
					
					throw new IllegalArgumentException(text);
				}
				this.theAddTable = new float[num];
				for (int i = 0; i < num; i++)
				{
					float num2 = (float)this.logToLinear((float)(-(float)i));
					num2 += 1f;
					this.theAddTable[i] = this.linearToLog((double)num2);
				}
			}
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			87,
			211,
			106,
			137
		})]
		private float addTable(float num)
		{
			int num2 = ByteCodeHelper.d2i((double)num + 0.5);
			if (num2 < this.theAddTable.Length)
			{
				return this.theAddTable[num2];
			}
			return 0f;
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			146,
			106,
			106
		})]
		
		public static float logToLog(float logSource, float sourceBase, float resultBase)
		{
			float num = (float)java.lang.Math.log((double)sourceBase);
			float num2 = (float)java.lang.Math.log((double)resultBase);
			return logSource * num / num2;
		}

		public float logToLn(float logSource)
		{
			return logSource * this.naturalLogBase;
		}

		[LineNumberTable(new byte[]
		{
			67,
			108,
			121,
			103,
			111
		})]
		
		public static void setLogBase(float logBase)
		{
			lock (ClassLiteral<LogMath>.Value)
			{
				if (!LogMath.assertionsDisabled && LogMath.instance != null)
				{
					
					throw new AssertionError();
				}
				LogMath.logBase = logBase;
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			110,
			98,
			108,
			121,
			102,
			111
		})]
		
		public static void setUseTable(bool useTable)
		{
			lock (ClassLiteral<LogMath>.Value)
			{
				if (!LogMath.assertionsDisabled && LogMath.instance != null)
				{
					
					throw new AssertionError();
				}
				LogMath.useTable = useTable;
			}
		}

		[LineNumberTable(new byte[]
		{
			160,
			68,
			107,
			105
		})]
		
		private float addTableActualComputation(float num)
		{
			double num2 = this.logToLinear(-num);
			num2 += (double)1f;
			return this.linearToLog(num2);
		}

		[Throws(new string[]
		{
			"java.lang.IllegalArgumentException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			115,
			102,
			223,
			24,
			102,
			112
		})]
		
		public float subtractAsLinear(float logMinuend, float logSubtrahend)
		{
			if (logMinuend < logSubtrahend)
			{
				string text = new StringBuilder().append("Subtraction results in log of a negative number: ").append(logMinuend).append(" - ").append(logSubtrahend).toString();
				
				throw new IllegalArgumentException(text);
			}
			double num = (double)1f;
			num -= this.logToLinear(logSubtrahend - logMinuend);
			return logMinuend + this.linearToLog(num);
		}

		public bool isUseTable()
		{
			return LogMath.useTable;
		}

		[LineNumberTable(new byte[]
		{
			159,
			162,
			245,
			71,
			106
		})]
		static LogMath()
		{
			LogMath.logBase = 1.0001f;
			LogMath.useTable = true;
		}

		public const float LOG_ZERO = -3.40282347E+38f;

		public const float LOG_ONE = 0f;

		private static LogMath instance;

		private static float logBase;

		private static bool useTable;

		private float naturalLogBase;

		private float inverseNaturalLogBase;

		private float[] theAddTable;

		
		internal static bool assertionsDisabled = !ClassLiteral<LogMath>.Value.desiredAssertionStatus();
	}
}
