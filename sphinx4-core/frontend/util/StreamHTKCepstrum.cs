using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Runtime;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.frontend.util
{
	public class StreamHTKCepstrum : BaseDataProcessor
	{
		public static short readLittleEndianShort(DataInputStream dataStream)
		{
			int num = 0;
			for (int i = 0; i < 16; i += 8)
			{
				int num2 = 255 & (int)((sbyte)dataStream.readByte());
				num = (int)((short)(num | num2 << i));
			}
			return (short)num;
		}
		
		public StreamHTKCepstrum(float frameShiftMs, float frameSizeMs, bool bigEndian, int sampleRate)
		{
			this.initLogger();
			this.bigEndian = bigEndian;
			this.sampleRate = sampleRate;
			this.frameShift = DataUtil.getSamplesPerWindow(sampleRate, frameShiftMs);
			this.frameSize = DataUtil.getSamplesPerShift(sampleRate, frameSizeMs);
		}
		
		public StreamHTKCepstrum()
		{
		}

		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			float @float = ps.getFloat("frameShiftInMs");
			float float2 = ps.getFloat("frameSizeInMs");
			this.bigEndian = ps.getBoolean("bigEndian").booleanValue();
			this.sampleRate = ps.getInt("sampleRate");
			this.frameShift = DataUtil.getSamplesPerWindow(this.sampleRate, @float);
			this.frameSize = DataUtil.getSamplesPerShift(this.sampleRate, float2);
			this.logger = ps.getLogger();
		}
		
		public override void initialize()
		{
			base.initialize();
			this.curPoint = -1;
			this.firstSampleNumber = 0L;
		}
		
		public virtual void setInputStream(InputStream stream)
		{
			this.binaryStream = new DataInputStream(new BufferedInputStream(stream));
			if (this.bigEndian)
			{
				this.numPoints = this.binaryStream.readInt();
				int num = this.binaryStream.readInt();
				int num2 = (int)this.binaryStream.readShort();
				int num3 = (int)this.binaryStream.readShort();
				this.cepstrumLength = num2 / 4;
				this.numPoints *= this.cepstrumLength;
				this.logger.info(new StringBuilder().append("Sample period is ").append(num).toString());
				this.logger.info(new StringBuilder().append("Sample size ").append(num2).toString());
				this.logger.info(new StringBuilder().append("Parameter kind ").append(num3).toString());
				this.logger.info("BigEndian");
			}
			else
			{
				this.numPoints = Utilities.readLittleEndianInt(this.binaryStream);
				int num = Utilities.readLittleEndianInt(this.binaryStream);
				int num2 = (int)StreamHTKCepstrum.readLittleEndianShort(this.binaryStream);
				int num3 = (int)StreamHTKCepstrum.readLittleEndianShort(this.binaryStream);
				this.cepstrumLength = num2 / 4;
				this.numPoints *= this.cepstrumLength;
				this.logger.info(new StringBuilder().append("Sample period is ").append(num).toString());
				this.logger.info(new StringBuilder().append("Sample size ").append(num2).toString());
				this.logger.info(new StringBuilder().append("Parameter kind ").append(num3).toString());
				this.logger.info("LittleEndian");
			}
			PrintStream @out = java.lang.System.@out;
			StringBuilder stringBuilder = new StringBuilder().append("Frames: ");
			int num4 = this.numPoints;
			int num5 = this.cepstrumLength;
			@out.println(stringBuilder.append((num5 != -1) ? (num4 / num5) : (-num4)).toString());
			this.curPoint = -1;
			this.firstSampleNumber = 0L;
		}
		
		public override Data getData()
		{
			object obj;
			if (this.curPoint == -1)
			{
				obj = new DataStartSignal(this.sampleRate);
				this.curPoint++;
			}
			else
			{
				if (this.curPoint == this.numPoints)
				{
					if (this.numPoints > 0)
					{
						this.firstSampleNumber = this.firstSampleNumber - (long)this.frameShift + (long)this.frameSize - 1L;
					}
					int num = this.curPoint;
					int num2 = this.cepstrumLength;
					int num3 = (num2 != -1) ? (num / num2) : (-num);
					int i = (num3 - 1) * this.frameShift + this.frameSize;
					long duration = ByteCodeHelper.d2l((double)i / (double)this.sampleRate * 1000.0);
					obj = new DataEndSignal(duration);
					try
					{
						this.binaryStream.close();
						this.curPoint++;
					}
					catch (IOException ex)
					{
						throw new DataProcessingException("IOException closing cepstrum stream", ex);
					}
					goto IL_1B5;
				}
				if (this.curPoint > this.numPoints)
				{
					obj = null;
				}
				else
				{
					double[] array = new double[this.cepstrumLength];
					int i = 0;
					while (i < this.cepstrumLength)
					{
						try
						{
							if (this.bigEndian)
							{
								array[i] = (double)this.binaryStream.readFloat();
							}
							else
							{
								array[i] = (double)Utilities.readLittleEndianFloat(this.binaryStream);
							}
							this.curPoint++;
						}
						catch (IOException ex4)
						{
							throw new DataProcessingException("IOException reading from cepstrum stream", ex4);
						}
						i++;
						continue;
					}
					obj = new DoubleData(array, this.sampleRate, this.firstSampleNumber);
					this.firstSampleNumber += (long)this.frameShift;
				}
			}
			IL_1B5:
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

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_BINARY = "binary";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			true
		})]
		public const string PROP_BIGENDIAN = "bigEndian";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			25.625
		})]
		public const string PROP_FRAME_SIZE_MS = "frameSizeInMs";

		[S4Double(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Double;",
			"defaultValue",
			10.0
		})]
		public const string PROP_FRAME_SHIFT_MS = "frameShiftInMs";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			13
		})]
		public const string PROP_CEPSTRUM_LENGTH = "cepstrumLength";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			16000
		})]
		public const string PROP_SAMPLE_RATE = "sampleRate";

		private DataInputStream binaryStream;

		private int numPoints;

		private int curPoint;

		private int cepstrumLength;

		private int frameShift;

		private int frameSize;

		private int sampleRate;

		private long firstSampleNumber;

		private bool bigEndian;
	}
}
