using edu.cmu.sphinx.util;
using java.lang;

namespace edu.cmu.sphinx.frontend
{
	public class FloatData : java.lang.Object, Data, Cloneable.__Interface
	{
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
					string text = new StringBuilder().append("data type '").append(java.lang.Object.instancehelper_getClass(data)).append("' is not supported").toString();
					
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
		
		public FloatData(float[] values, int sampleRate, long collectTime, long firstSampleNumber)
		{
			init(values, sampleRate, collectTime, firstSampleNumber);
		}
		
		public FloatData(float[] values, int sampleRate, long firstSampleNumber)
		{
			long num = firstSampleNumber * (long)((ulong)1000);
			long num2 = (long)sampleRate;
			init(values, sampleRate, (num2 != -1L) ? (num / num2) : (-num), firstSampleNumber);
		}
		private void init(float[] values, int sampleRate, long collectTime, long firstSampleNumber)
		{
			this.values = values;
			this.sampleRate = sampleRate;
			this.collectTime = collectTime;
			this.firstSampleNumber = firstSampleNumber;
		}
		public new virtual FloatData clone()
		{
			FloatData result;
			try
			{
				FloatData floatData = (FloatData)base.clone();
				result = floatData;
			}
			catch (CloneNotSupportedException ex)
			{
				throw new InternalError(Throwable.instancehelper_toString(ex));
			}
			return result;
		}

		public virtual int getSampleRate()
		{
			return this.sampleRate;
		}

		public virtual long getFirstSampleNumber()
		{
			return this.firstSampleNumber;
		}
		
		public static implicit operator Cloneable(FloatData _ref)
		{
			Cloneable result = Cloneable.Cast(_ref);  

			return result;
		}

		
		private float[] values;
		
		private int sampleRate;

		private long firstSampleNumber;
		
		private long collectTime;
	}
}
