using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.frontend.util
{
	public class StreamDataSource : BaseDataProcessor
	{
		public virtual void setInputStream(InputStream inputStream, TimeFrame timeFrame)
		{
			this.dataStream = inputStream;
			this.timeFrame = timeFrame;
			this.streamEndReached = false;
			this.utteranceEndSent = false;
			this.utteranceStarted = false;
			this.totalValuesRead = 0L;
		}

		[LineNumberTable(new byte[]
		{
			85,
			108
		})]
		
		public virtual void setInputStream(InputStream inputStream)
		{
			this.setInputStream(inputStream, TimeFrame.__INFINITE);
		}

		[LineNumberTable(new byte[]
		{
			159,
			115,
			102,
			103,
			103,
			135,
			115,
			176,
			105,
			103,
			103,
			121
		})]
		
		private void init(int num, int num2, int num3, bool flag, bool flag2)
		{
			this.sampleRate = num;
			this.bytesPerRead = num2;
			this.bitsPerSample = num3;
			bool flag3 = this.bitsPerSample != 0;
			int num4 = 8;
			if (num4 != -1 && (flag3 ? 1 : 0) % num4 != 0)
			{
				string text = "bits per sample must be a multiple of 8";
				
				throw new IllegalArgumentException(text);
			}
			this.bytesPerValue = num3 / 8;
			this.bigEndian = flag;
			this.signedData = flag2;
			int num5 = this.bytesPerRead;
			int num6 = 2;
			this.bytesPerRead = num5 + ((num6 != -1) ? (num2 % num6) : 0);
		}

		private long getDuration()
		{
			return ByteCodeHelper.d2l((double)this.totalValuesRead / (double)this.sampleRate * 1000.0);
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			95,
			98,
			103,
			108,
			167,
			146,
			101,
			133,
			105,
			100,
			102,
			168,
			126,
			100,
			183,
			104,
			102,
			133,
			99,
			221,
			2,
			98,
			210,
			104,
			184,
			246,
			70
		})]
		
		private DoubleData readNextFrame()
		{
			int num = 0;
			int num2 = this.bytesPerRead;
			byte[] array = new byte[this.bytesPerRead];
			long firstSampleNumber = this.totalValuesRead;
			DoubleData result;
			IOException ex2;
			try
			{
				int num3;
				do
				{
					num3 = this.dataStream.read(array, num, num2 - num);
					if (num3 > 0)
					{
						num += num3;
					}
				}
				while (num3 != -1 && num < num2);
				if (num > 0)
				{
					long num4 = this.totalValuesRead;
					int num5 = num;
					int num6 = this.bytesPerValue;
					this.totalValuesRead = num4 + (long)((num6 != -1) ? (num5 / num6) : (-(long)num5));
					if (num < num2)
					{
						bool flag = num != 0;
						int num7 = 2;
						num = ((num7 != -1 && (flag ? 1 : 0) % num7 != 0) ? (num + 3) : (num + 2));
						byte[] array2 = new byte[num];
						ByteCodeHelper.arraycopy_primitive_1(array, 0, array2, 0, num);
						array = array2;
						this.closeDataStream();
					}
					goto IL_B9;
				}
				this.closeDataStream();
				result = null;
			}
			catch (IOException ex)
			{
				ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
				goto IL_BB;
			}
			return result;
			IL_B9:
			double[] values;
			if (this.bigEndian)
			{
				values = DataUtil.bytesToValues(array, 0, num, this.bytesPerValue, this.signedData);
			}
			else
			{
				values = DataUtil.littleEndianBytesToValues(array, 0, num, this.bytesPerValue, this.signedData);
			}
			return new DoubleData(values, this.sampleRate, firstSampleNumber);
			IL_BB:
			IOException ex3 = ex2;
			string message = "Error reading data";
			Exception cause = ex3;
			
			throw new DataProcessingException(message, cause);
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			160,
			143,
			103,
			104,
			139
		})]
		
		private void closeDataStream()
		{
			this.streamEndReached = true;
			if (this.dataStream != null)
			{
				this.dataStream.close();
			}
		}

		[LineNumberTable(new byte[]
		{
			159,
			123,
			134,
			232,
			61,
			203,
			102,
			107
		})]
		
		public StreamDataSource(int sampleRate, int bytesPerRead, int bitsPerSample, bool bigEndian, bool signedData)
		{
			this.timeFrame = TimeFrame.__INFINITE;
			this.initLogger();
			this.init(sampleRate, bytesPerRead, bitsPerSample, bigEndian, signedData);
		}

		[LineNumberTable(new byte[]
		{
			33,
			232,
			56,
			235,
			74
		})]
		
		public StreamDataSource()
		{
			this.timeFrame = TimeFrame.__INFINITE;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			45,
			103,
			103,
			107,
			107,
			107,
			112,
			234,
			59,
			229,
			70
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.init(ps.getInt("sampleRate"), ps.getInt("bytesPerRead"), ps.getInt("bitsPerSample"), ps.getBoolean("bigEndianData").booleanValue(), ps.getBoolean("signedData").booleanValue());
		}

		[LineNumberTable(new byte[]
		{
			81,
			102
		})]
		
		public override void initialize()
		{
			base.initialize();
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			113,
			98,
			104,
			171,
			108,
			172,
			104,
			103,
			145,
			139,
			103,
			155,
			159,
			4,
			108,
			103,
			169,
			112,
			104,
			108,
			231,
			69
		})]
		
		public override Data getData()
		{
			object obj = null;
			if (this.streamEndReached)
			{
				if (!this.utteranceEndSent)
				{
					obj = new DataEndSignal(this.getDuration());
					this.utteranceEndSent = true;
				}
			}
			else if (!this.utteranceStarted)
			{
				this.utteranceStarted = true;
				obj = new DataStartSignal(this.sampleRate);
			}
			else if (this.dataStream != null)
			{
				do
				{
					obj = this.readNextFrame();
				}
				while ((DoubleData)obj != null && this.getDuration() < this.timeFrame.getStart());
				if (((DoubleData)obj == null || this.getDuration() > this.timeFrame.getEnd()) && !this.utteranceEndSent)
				{
					obj = new DataEndSignal(this.getDuration());
					this.utteranceEndSent = true;
					this.streamEndReached = true;
				}
			}
			else
			{
				this.logger.warning("Input stream is not set");
				if (!this.utteranceEndSent)
				{
					obj = new DataEndSignal(this.getDuration());
					this.utteranceEndSent = true;
				}
			}
			object obj2 = obj;
			Data result;
			if (obj2 != null)
			{
				if ((result = (obj2 as Data)) == null)
				{
					throw new IncompatibleClassChangeError();
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			16000
		})]
		public const string PROP_SAMPLE_RATE = "sampleRate";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			3200
		})]
		public const string PROP_BYTES_PER_READ = "bytesPerRead";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			16
		})]
		public const string PROP_BITS_PER_SAMPLE = "bitsPerSample";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_BIG_ENDIAN_DATA = "bigEndianData";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_SIGNED_DATA = "signedData";

		private InputStream dataStream;

		protected internal int sampleRate;

		private int bytesPerRead;

		private int bytesPerValue;

		private long totalValuesRead;

		private bool bigEndian;

		private bool signedData;

		private bool streamEndReached;

		private bool utteranceEndSent;

		private bool utteranceStarted;

		protected internal int bitsPerSample;

		private TimeFrame timeFrame;
	}
}
