using edu.cmu.sphinx.util.machlearn;
using java.lang;

namespace edu.cmu.sphinx.frontend
{
	public class DoubleData : OVector, Data
	{
		public virtual long getFirstSampleNumber()
		{
			return this.firstSampleNumber;
		}
		
		public DoubleData(double[] values, int sampleRate, long firstSampleNumber) : base(values)
		{
			this.sampleRate = sampleRate;
			long num = firstSampleNumber * (long)((ulong)1000);
			long num2 = (long)sampleRate;
			this.collectTime = ((num2 != -1L) ? (num / num2) : (-num));
			this.firstSampleNumber = firstSampleNumber;
		}
		
		public new virtual DoubleData clone()
		{
			DoubleData result;
			try
			{
				DoubleData doubleData = (DoubleData)base.clone();
				doubleData.sampleRate = this.sampleRate;
				doubleData.collectTime = this.collectTime;
				doubleData.firstSampleNumber = this.firstSampleNumber;
				result = doubleData;
			}
			catch (CloneNotSupportedException ex)
			{
				throw new InternalError(Throwable.instancehelper_toString(ex));
			}
			return result;
		}
		
		public DoubleData(double[] values) : base(values)
		{
		}
		
		public DoubleData(double[] values, int sampleRate, long collectTime, long firstSampleNumber) : base(values)
		{
			this.sampleRate = sampleRate;
			this.collectTime = collectTime;
			this.firstSampleNumber = firstSampleNumber;
		}
		
		public override string toString()
		{
			return new StringBuilder().append("DoubleData: ").append(this.sampleRate).append("Hz, first sample #: ").append(this.firstSampleNumber).append(", collect time: ").append(this.collectTime).toString();
		}

		public virtual int getSampleRate()
		{
			return this.sampleRate;
		}

		public virtual long getCollectTime()
		{
			return this.collectTime;
		}
		
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected DoubleData(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private int sampleRate;

		private long firstSampleNumber;

		private long collectTime;
	}
}
