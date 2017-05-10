using System;

using edu.cmu.sphinx.util;
using edu.cmu.sphinx.util.props;
using IKVM.Attributes;
using IKVM.Runtime;
using java.io;
using java.lang;

namespace edu.cmu.sphinx.frontend.util
{
	public class StreamCepstrumSource : BaseDataProcessor
	{
		[LineNumberTable(new byte[]
		{
			159,
			125,
			130,
			104,
			102,
			103,
			103,
			104,
			111,
			112
		})]
		
		public StreamCepstrumSource(int cepstrumLength, bool binary, float frameShiftMs, float frameSizeMs, int sampleRate)
		{
			this.initLogger();
			this.cepstrumLength = cepstrumLength;
			this.binary = binary;
			this.sampleRate = sampleRate;
			this.frameShift = DataUtil.getSamplesPerWindow(sampleRate, frameShiftMs);
			this.frameSize = DataUtil.getSamplesPerShift(sampleRate, frameSizeMs);
		}

		[LineNumberTable(new byte[]
		{
			29,
			102
		})]
		
		public StreamCepstrumSource()
		{
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.util.props.PropertyException"
		})]
		[LineNumberTable(new byte[]
		{
			39,
			103,
			113,
			118,
			118,
			108,
			108,
			113,
			114,
			114
		})]
		
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.cepstrumLength = ps.getInt("cepstrumLength");
			this.binary = ps.getBoolean("binary").booleanValue();
			this.bigEndian = ps.getBoolean("bigEndianData").booleanValue();
			float @float = ps.getFloat("frameShiftInMs");
			float float2 = ps.getFloat("frameSizeInMs");
			this.sampleRate = ps.getInt("sampleRate");
			this.frameShift = DataUtil.getSamplesPerWindow(this.sampleRate, @float);
			this.frameSize = DataUtil.getSamplesPerShift(this.sampleRate, float2);
		}

		[LineNumberTable(new byte[]
		{
			54,
			102,
			103,
			104,
			103
		})]
		
		public override void initialize()
		{
			base.initialize();
			this.curPoint = -1;
			this.firstSampleNumber = 0L;
			this.bigEndian = false;
		}

		[Throws(new string[]
		{
			"java.io.IOException"
		})]
		[LineNumberTable(new byte[]
		{
			159,
			112,
			66,
			103,
			107,
			113,
			99,
			113,
			145,
			113,
			143,
			159,
			27,
			109,
			118,
			144,
			103,
			104
		})]
		
		public virtual void setInputStream(InputStream @is, bool bigEndian)
		{
			this.bigEndian = bigEndian;
			if (this.binary)
			{
				this.binaryStream = new DataInputStream(new BufferedInputStream(@is));
				if (bigEndian)
				{
					this.numPoints = this.binaryStream.readInt();
					java.lang.System.@out.println("BigEndian");
				}
				else
				{
					this.numPoints = Utilities.readLittleEndianInt(this.binaryStream);
					java.lang.System.@out.println("LittleEndian");
				}
				PrintStream @out = java.lang.System.@out;
				StringBuilder stringBuilder = new StringBuilder().append("Frames: ");
				int num = this.numPoints;
				int num2 = this.cepstrumLength;
				@out.println(stringBuilder.append((num2 != -1) ? (num / num2) : (-num)).toString());
			}
			else
			{
				this.est = new ExtendedStreamTokenizer(@is, false);
				this.numPoints = this.est.getInt("num_frames");
				this.est.expectString("frames");
			}
			this.curPoint = -1;
			this.firstSampleNumber = 0L;
		}

		[Throws(new string[]
		{
			"edu.cmu.sphinx.frontend.DataProcessingException"
		})]
		[LineNumberTable(new byte[]
		{
			103,
			105,
			108,
			115,
			113,
			105,
			223,
			0,
			118,
			114,
			186,
			167,
			104,
			141,
			139,
			191,
			3,
			2,
			98,
			146,
			115,
			135,
			141,
			142,
			104,
			104,
			146,
			178,
			149,
			191,
			3,
			2,
			98,
			242,
			51,
			233,
			82,
			148,
			180
		})]
		
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
					IOException ex2;
					try
					{
						if (this.binary)
						{
							this.binaryStream.close();
						}
						else
						{
							this.est.close();
						}
						this.curPoint++;
					}
					catch (IOException ex)
					{
						ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
						goto IL_E8;
					}
					goto IL_1E9;
					IL_E8:
					IOException ex3 = ex2;
					string message = "IOException closing cepstrum stream";
					Exception cause = ex3;
					
					throw new DataProcessingException(message, cause);
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
						IOException ex5;
						try
						{
							if (this.binary)
							{
								if (this.bigEndian)
								{
									array[i] = (double)this.binaryStream.readFloat();
								}
								else
								{
									array[i] = (double)Utilities.readLittleEndianFloat(this.binaryStream);
								}
							}
							else
							{
								array[i] = (double)this.est.getFloat("cepstrum data");
							}
							this.curPoint++;
						}
						catch (IOException ex4)
						{
							ex5 = ByteCodeHelper.MapException<IOException>(ex4, 1);
							goto IL_19E;
						}
						i++;
						continue;
						IL_19E:
						IOException ex6 = ex5;
						string message2 = "IOException reading from cepstrum stream";
						Exception cause2 = ex6;
						
						throw new DataProcessingException(message2, cause2);
					}
					obj = new DoubleData(array, this.sampleRate, this.firstSampleNumber);
					this.firstSampleNumber += (long)this.frameShift;
				}
			}
			IL_1E9:
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

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_BIG_ENDIAN_DATA = "bigEndianData";

		[S4Integer(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Integer;",
			"defaultValue",
			16000
		})]
		public const string PROP_SAMPLE_RATE = "sampleRate";

		private bool binary;

		private ExtendedStreamTokenizer est;

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
