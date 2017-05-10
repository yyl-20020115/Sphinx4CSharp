using System;
using System.ComponentModel;

using edu.cmu.sphinx.util;
using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.frontend
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.frontend.Data",
		"java.lang.Cloneable"
	})]
	public class FloatData : java.lang.Object, Data, Cloneable.__Interface
	{
		[LineNumberTable(new byte[]
		{
			58,
			104,
			105,
			104,
			103,
			114,
			107,
			98,
			159,
			21
		})]
		
		public static FloatData toFloatData(Data data)
		{
			FloatData result;
			if (data is FloatData)
			{
				result = (FloatData)data;
			}
			else
			{
				if (!(data is DoubleData))
				{
					string text = new StringBuilder().append("data type '").append(Object.instancehelper_getClass(data)).append("' is not supported").toString();
					
					throw new IllegalArgumentException(text);
				}
				DoubleData doubleData = (DoubleData)data;
				result = new FloatData(MatrixUtils.double2float(doubleData.getValues()), doubleData.getSampleRate(), doubleData.getFirstSampleNumber());
			}
			return result;
		}

		public virtual float[] getValues()
		{
			return this.values;
		}

		public virtual long getCollectTime()
		{
			return this.collectTime;
		}

		[LineNumberTable(new byte[]
		{
			159,
			190,
			104,
			103,
			103,
			103,
			104
		})]
		
		public FloatData(float[] values, int sampleRate, long collectTime, long firstSampleNumber)
		{
			this.values = values;
			this.sampleRate = sampleRate;
			this.collectTime = collectTime;
			this.firstSampleNumber = firstSampleNumber;
		}

		[LineNumberTable(new byte[]
		{
			159,
			178,
			125
		})]
		
		public FloatData(float[] values, int sampleRate, long firstSampleNumber)
		{
			long num = firstSampleNumber * (long)((ulong)1000);
			long num2 = (long)sampleRate;
			this..ctor(values, sampleRate, (num2 != -1L) ? (num / num2) : (-num), firstSampleNumber);
		}

		[Throws(new string[]
		{
			"java.lang.CloneNotSupportedException"
		})]
		[LineNumberTable(new byte[]
		{
			43,
			108,
			119,
			97
		})]
		
		public virtual FloatData clone()
		{
			FloatData result;
			CloneNotSupportedException ex2;
			try
			{
				FloatData floatData = (FloatData)base.clone();
				result = floatData;
			}
			catch (CloneNotSupportedException ex)
			{
				ex2 = ByteCodeHelper.MapException<CloneNotSupportedException>(ex, 1);
				goto IL_1F;
			}
			return result;
			IL_1F:
			CloneNotSupportedException ex3 = ex2;
			string text = Throwable.instancehelper_toString(ex3);
			
			throw new InternalError(text);
		}

		public virtual int getSampleRate()
		{
			return this.sampleRate;
		}

		public virtual long getFirstSampleNumber()
		{
			return this.firstSampleNumber;
		}

		[Throws(new string[]
		{
			"java.lang.CloneNotSupportedException"
		})]
		
		[EditorBrowsable(EditorBrowsableState.Never)]
		
		
		public virtual object <bridge>clone()
		{
			return this.clone();
		}

		
		public static implicit operator Cloneable(FloatData _<ref>)
		{
			Cloneable result;
			result.__<ref> = _<ref>;
			return result;
		}

		
		private float[] values;

		
		private int sampleRate;

		
		private long firstSampleNumber;

		
		private long collectTime;
	}
}
