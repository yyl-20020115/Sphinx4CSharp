using System;
using System.ComponentModel;

using System.Runtime.Serialization;
using System.Security.Permissions;
using edu.cmu.sphinx.util.machlearn;
using IKVM.Attributes;
using IKVM.Runtime;
using java.lang;

namespace edu.cmu.sphinx.frontend
{
	[Implements(new string[]
	{
		"edu.cmu.sphinx.frontend.Data"
	})]
	[Serializable]
	public class DoubleData : OVector, Data
	{
		public virtual long getFirstSampleNumber()
		{
			return this.firstSampleNumber;
		}

		[LineNumberTable(new byte[]
		{
			159,
			188,
			137,
			103,
			122,
			103
		})]
		
		public DoubleData(double[] values, int sampleRate, long firstSampleNumber) : base(values)
		{
			this.sampleRate = sampleRate;
			long num = firstSampleNumber * (long)((ulong)1000);
			long num2 = (long)sampleRate;
			this.collectTime = ((num2 != -1L) ? (num / num2) : (-num));
			this.firstSampleNumber = firstSampleNumber;
		}

		[Throws(new string[]
		{
			"java.lang.CloneNotSupportedException"
		})]
		[LineNumberTable(new byte[]
		{
			60,
			108,
			108,
			108,
			108,
			119,
			97
		})]
		
		public virtual DoubleData clone()
		{
			DoubleData result;
			CloneNotSupportedException ex2;
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
				ex2 = ByteCodeHelper.MapException<CloneNotSupportedException>(ex, 1);
				goto IL_43;
			}
			return result;
			IL_43:
			CloneNotSupportedException ex3 = ex2;
			string text = Throwable.instancehelper_toString(ex3);
			
			throw new InternalError(text);
		}

		[LineNumberTable(new byte[]
		{
			159,
			175,
			103
		})]
		
		public DoubleData(double[] values) : base(values)
		{
		}

		[LineNumberTable(new byte[]
		{
			13,
			137,
			103,
			103,
			104
		})]
		
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

		[Throws(new string[]
		{
			"java.lang.CloneNotSupportedException"
		})]
		
		[EditorBrowsable(EditorBrowsableState.Never)]
		
		
		public virtual object <bridge>clone()
		{
			return this.clone();
		}

		
		[PermissionSet(SecurityAction.Demand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		protected DoubleData(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		private int sampleRate;

		private long firstSampleNumber;

		private long collectTime;
	}
}
