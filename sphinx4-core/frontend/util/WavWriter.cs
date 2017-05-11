using edu.cmu.sphinx.frontend.endpoint;
using edu.cmu.sphinx.util.props;
using ikvm.@internal;
using IKVM.Runtime;
using java.io;
using java.lang;
using javax.sound.sampled;

namespace edu.cmu.sphinx.frontend.util
{
	public class WavWriter : BaseDataProcessor
	{
		public override void initialize()
		{
			base.initialize();
			if (!WavWriter.assertionsDisabled && this.outFileNamePattern == null)
			{
				
				throw new AssertionError();
			}
			this.baos = new ByteArrayOutputStream();
		}

		private static string getNextFreeIndex(string text)
		{
			int num = 0;
			string text3;
			for (;;)
			{
				string text2 = Integer.toString(num);
				text3 = new StringBuilder().append(java.lang.String.instancehelper_substring(text, 0, java.lang.Math.max(0, java.lang.String.instancehelper_length(text) - java.lang.String.instancehelper_length(text2)))).append(text2).append(".wav").toString();
				if (!new File(text3).isFile())
				{
					break;
				}
				num++;
			}
			return text3;
		}

		protected internal virtual void writeFile(string wavName)
		{
			AudioFormat audioFormat = new AudioFormat((float)this.sampleRate, this.bitsPerSample, 1, this.isSigned, true);
			AudioFileFormat.Type targetType = WavWriter.getTargetType("wav");
			byte[] array = this.baos.toByteArray();
			ByteArrayInputStream byteArrayInputStream = new ByteArrayInputStream(array);
			InputStream inputStream = byteArrayInputStream;
			AudioFormat audioFormat2 = audioFormat;
			int num = array.Length;
			int frameSize = audioFormat.getFrameSize();
			AudioInputStream audioInputStream = new AudioInputStream(inputStream, audioFormat2, (long)((frameSize != -1) ? (num / frameSize) : (-(long)num)));
			File file = new File(wavName);
			if (AudioSystem.isFileTypeSupported(targetType, audioInputStream))
			{
				IOException ex2;
				try
				{
					AudioSystem.write(audioInputStream, targetType, file);
				}
				catch (IOException ex)
				{
					ex2 = ByteCodeHelper.MapException<IOException>(ex, 1);
					goto IL_81;
				}
				return;
				IL_81:
				IOException ex3 = ex2;
				Throwable.instancehelper_printStackTrace(ex3);
			}
		}
		
		private static AudioFileFormat.Type getTargetType(string text)
		{
			AudioFileFormat.Type[] audioFileTypes = AudioSystem.getAudioFileTypes();
			AudioFileFormat.Type[] array = audioFileTypes;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				AudioFileFormat.Type type = array[i];
				if (java.lang.String.instancehelper_equals(type.getExtension(), text))
				{
					return type;
				}
			}
			return null;
		}
		
		public WavWriter(string dumpFilePath, bool isCompletePath, int bitsPerSample, bool isSigned, bool captureUtts)
		{
			this.isSigned = true;
			this.initLogger();
			this.outFileNamePattern = dumpFilePath;
			this.isCompletePath = isCompletePath;
			this.bitsPerSample = bitsPerSample;
			int num = 8;
			if (num != -1 && bitsPerSample % num != 0)
			{
				string text = "StreamDataSource: bits per sample must be a multiple of 8.";
				
				throw new Error(text);
			}
			this.isSigned = isSigned;
			this.captureUtts = captureUtts;
			this.initialize();
		}
		
		public WavWriter()
		{
			this.isSigned = true;
		}
	
		public override void newProperties(PropertySheet ps)
		{
			base.newProperties(ps);
			this.outFileNamePattern = ps.getString("outFilePattern");
			this.isCompletePath = ps.getBoolean("isCompletePath").booleanValue();
			this.bitsPerSample = ps.getInt("bitsPerSample");
			bool flag = this.bitsPerSample != 0;
			int num = 8;
			if (num != -1 && (flag ? 1 : 0) % num != 0)
			{
				string text = "StreamDataSource: bits per sample must be a multiple of 8.";
				
				throw new Error(text);
			}
			this.isSigned = ps.getBoolean("signedData").booleanValue();
			this.captureUtts = ps.getBoolean("captureUtterances").booleanValue();
			this.initialize();
		}
	
		public override Data getData()
		{
			Data data = this.getPredecessor().getData();
			if (data is DataStartSignal)
			{
				this.sampleRate = ((DataStartSignal)data).getSampleRate();
			}
			if (data is DataStartSignal || (data is SpeechStartSignal && this.captureUtts))
			{
				this.baos = new ByteArrayOutputStream();
				this.dos = new DataOutputStream(this.baos);
			}
			if ((data is DataEndSignal && !this.captureUtts) || (data is SpeechEndSignal && this.captureUtts))
			{
				string nextFreeIndex;
				if (this.isCompletePath)
				{
					nextFreeIndex = this.outFileNamePattern;
				}
				else
				{
					nextFreeIndex = WavWriter.getNextFreeIndex(this.outFileNamePattern);
				}
				this.writeFile(nextFreeIndex);
				this.isInSpeech = false;
			}
			if (data is SpeechStartSignal)
			{
				this.isInSpeech = true;
			}
			if ((data is DoubleData || data is FloatData) && (this.isInSpeech || !this.captureUtts))
			{
				DoubleData doubleData = (!(data is DoubleData)) ? DataUtil.FloatData2DoubleData((FloatData)data) : ((DoubleData)data);
				double[] values = doubleData.getValues();
				double[] array = values;
				int num = array.Length;
				int i = 0;
				while (i < num)
				{
					double num2 = array[i];
					try
					{
						this.dos.writeShort((int)new Short((short)ByteCodeHelper.d2i(num2)).shortValue());
					}
					catch (IOException ex)
					{
						Throwable.instancehelper_printStackTrace(ex);
					}
					i++;
					continue;
				}
			}
			return data;
		}

		public virtual void setOutFilePattern(string outFileNamePattern)
		{
			this.outFileNamePattern = outFileNamePattern;
		}

		public static byte[] valuesToBytes(double[] values, int bytesPerValue, bool signedData)
		{
			byte[] array = new byte[bytesPerValue * values.Length];
			int num = 0;
			int num2 = values.Length;
			for (int i = 0; i < num2; i++)
			{
				double num3 = values[i];
				int num4 = ByteCodeHelper.d2i(num3);
				for (int j = bytesPerValue - 1; j >= 0; j++)
				{
					array[num + j] = (byte)((sbyte)(num4 & 255));
					num4 >>= 8;
				}
				num += bytesPerValue;
			}
			return array;
		}

		[S4String(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4String;",
			"defaultValue",
			"seg000000"
		})]
		public const string PROP_OUT_FILE_NAME_PATTERN = "outFilePattern";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_IS_COMPLETE_PATH = "isCompletePath";

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
			true
		})]
		public const string PROP_SIGNED_DATA = "signedData";

		[S4Boolean(new object[]
		{
			64,
			"Ledu/cmu/sphinx/util/props/S4Boolean;",
			"defaultValue",
			false
		})]
		public const string PROP_CAPTURE_UTTERANCES = "captureUtterances";

		private ByteArrayOutputStream baos;

		private DataOutputStream dos;

		private int sampleRate;

		private bool isInSpeech;

		private bool isSigned;

		private int bitsPerSample;

		private string outFileNamePattern;

		protected internal bool captureUtts;

		private bool isCompletePath;
		
		internal static bool assertionsDisabled = !ClassLiteral<WavWriter>.Value.desiredAssertionStatus();
	}
}
